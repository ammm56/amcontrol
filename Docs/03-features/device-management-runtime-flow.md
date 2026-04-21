# 设备管理后台链路实现说明

**文档编号**：FEAT-DEVICE-002  
**版本**：1.1.0  
**状态**：已实现  
**最后更新**：2026-04-21  
**维护人**：Am

---

## 1. 文档目标

本文档面向当前仓库中的真实实现，集中说明以下三条后台链路：

1. 设备注册；
2. 设备心跳；
3. 设备信息上报。

本文档不再从接口联调约定角度展开，而是从设备软件当前代码实现角度回答以下问题：

- 应用启动后是谁在驱动这三条链路；
- 每条链路的入口类、入口方法和触发时机；
- 请求体中各字段的来源；
- 成功与失败返回如何被本地处理；
- 本地如何缓存 `DeviceId`、长期 `DeviceToken`、`DeviceAppSecret` 和待上传数据；
- 后续排查问题时应优先看哪些类和方法。

当前实现已经按最新后端协议切换为：

1. `register`：`AES-GCM envelope`，不携带 `X-Device-Token`；
2. `heartbeat`：`X-Device-Token + AES-GCM envelope`；
3. `report`：`X-Device-Token + AES-GCM envelope`；
4. `refresh-token`：继续只带 `X-Device-Token`，不走 AES-GCM。

---

## 2. 文档范围

本文档覆盖以下当前实现文件：

- `AM.App/Bootstrap/AppBootstrap.cs`
- `AM.DBService/Services/System/UsageUploadWorker.cs`
- `AM.DBService/Services/System/DeviceRegisterClient.cs`
- `AM.DBService/Services/System/DeviceHeartbeatClient.cs`
- `AM.DBService/Services/System/DeviceReportClient.cs`
- `AM.DBService/Services/System/DeviceRequestCryptoService.cs`
- `AM.DBService/Services/System/UsageReportService.cs`
- `AM.DBService/Services/System/UsageEventBufferService.cs`
- `AM.DBService/Services/System/DeviceReportBufferService.cs`
- `AM.DBService/Services/System/ClientIdentityService.cs`
- `AM.DBService/Services/System/BackendServiceConfigHelper.cs`
- `AM.Model/Device/DeviceRegistrationModels.cs`
- `AM.Model/Device/DeviceRuntimeRequestModels.cs`
- `AM.Model/Device/DeviceApiResponse.cs`

本文档与 [device-license-and-reporting.md](device-license-and-reporting.md) 的关系如下：

1. `device-license-and-reporting.md` 偏接口与联调约定；
2. 本文档偏设备端当前代码实现、运行时流程和问题定位。

---

## 3. 启动到上报的总时序

当前三条链路统一由 `UsageUploadWorker` 调度，设备写请求的加密封装统一由 `DeviceRequestCryptoService` 处理。

启动总入口：

1. `AppBootstrap.Initialize()` 完成基础设施初始化；
2. `RegisterOptionalRuntimeWorker(runtimeTaskManager, new UsageUploadWorker(reporter), true, "后台上报工作单元")` 注册后台工作单元；
3. `UsageUploadWorker.Start()` 启动后台循环；
4. 后台循环按上传周期和心跳周期执行设备管理动作。

```mermaid
sequenceDiagram
    autonumber
    participant App as AppBootstrap
    participant Worker as UsageUploadWorker
    participant Config as ConfigContext/Setting
    participant Identity as ClientIdentityService
    participant Register as DeviceRegisterClient
    participant Heartbeat as DeviceHeartbeatClient
    participant Crypto as DeviceRequestCryptoService
    participant Usage as UsageReportService
    participant UsageBuf as UsageEventBufferService
    participant ReportBuf as DeviceReportBufferService
    participant Report as DeviceReportClient
    participant License as LicenseRuntimeContext
    participant User as UserContext
    participant Backend as Backend API

    App->>Worker: RegisterOptionalRuntimeWorker(new UsageUploadWorker, autoStart=true)
    App->>Worker: Start()
    Worker->>Worker: 延迟 5 秒后进入循环

    loop 每 1 秒执行一次调度检查
        Worker->>Worker: ExecuteCycleAsync()

        alt 首次循环
            Worker->>ReportBuf: EnqueueAppStart()
        end

        alt 到达使用事件上传周期
            Worker->>Worker: EnsureDeviceSessionAsync()
            alt 已有有效设备会话
                Worker->>UsageBuf: QueryPending(BatchSize)
                Worker->>Usage: UploadBatchAsync(events)
                Usage->>Report: ReportAsync(DeviceReportRequest)
                Report->>Crypto: BuildReportPackage(request)
                Crypto->>Backend: 生成 AES-GCM envelope + 安全头
                Report->>Backend: POST /api/devices/{id}/report
                alt 全部成功
                    Worker->>UsageBuf: MarkUploaded(ids)
                else 任一失败
                    Worker->>UsageBuf: MarkUploadFailed(ids, message)
                end
            else 无有效设备会话
                Worker-->>Worker: 记录节流日志，等待下轮重试
            end
        end

        alt 到达设备心跳周期
            Worker->>Worker: EnsureDeviceSessionAsync()
            alt 当前会话可用
                Worker->>Heartbeat: SendHeartbeatAsync(statusJson)
                Heartbeat->>Crypto: BuildHeartbeatPackage(request)
                Crypto->>Backend: 生成 AES-GCM envelope + 安全头
                Heartbeat->>Backend: POST /api/devices/{id}/heartbeat
                Worker->>ReportBuf: DequeueBatch(20)
                loop 逐条发送结构化 report
                    Worker->>Report: ReportAsync(DeviceReportRequest)
                    Report->>Crypto: BuildReportPackage(request)
                    Crypto->>Backend: 生成 AES-GCM envelope + 安全头
                    Report->>Backend: POST /api/devices/{id}/report
                    alt 某条失败
                        Worker->>ReportBuf: EnqueueMany(剩余未发送项)
                        break 中断本轮 flush
                    end
                end
            else 无有效设备会话
                Worker-->>Worker: 本轮跳过心跳和设备 report
            end
        end
    end
```

---

## 4. 统一驱动者：UsageUploadWorker

### 4.1 类职责

`UsageUploadWorker` 是当前设备管理后台链路的统一调度器，职责如下：

1. 周期性读取并上报本地使用事件；
2. 维护设备注册状态和 `DeviceToken`；
3. 周期性发送设备心跳；
4. 周期性发送结构化设备 report；
5. 对后台失败做节流日志输出，不影响 UI 主流程。

### 4.2 启动行为

`Start()` 的行为：

1. 检查是否已在运行；
2. 检查是否配置 `BackendServiceUrl`；
3. 重置 `_deviceSessionReady`、`_startupReportQueued`、调度时间戳；
4. 创建 `CancellationTokenSource`；
5. 异步启动 `WorkerLoopAsync()`。

### 4.3 后台循环行为

`WorkerLoopAsync()` 的行为：

1. 首次启动延迟 5 秒，避免程序刚启动时大量后台初始化并发；
2. 之后每隔 1 秒执行一次 `ExecuteCycleAsync()`；
3. 循环中任何异常都只写节流日志，不让后台线程退出。

### 4.4 单轮调度行为

`ExecuteCycleAsync()` 的行为：

1. 首次循环时向 `DeviceReportBufferService` 写入一条应用启动 report；
2. 若已到使用事件上传时间，则执行 `ExecuteUsageUploadOnceAsync()`；
3. 若已到设备心跳时间，则执行 `ExecuteDeviceRuntimeOnceAsync()`。

### 4.5 当前密码学实现口径

当前客户端实现已经明确切换到 BouncyCastle 版 `HKDF + AES-GCM`，不再使用此前的自实现或平台专有加密路径。

当前实现位置：

1. `AM.DBService/Services/System/DeviceRequestCryptoService.cs`
2. `AM.DBService/AM.DBService.csproj` 中引用 `Libs/bouncycastle/461/BouncyCastle.Cryptography.dll`

当前关键实现口径：

1. HKDF：使用 `HkdfBytesGenerator(new Sha256Digest())`
2. AES-GCM：使用 `GcmBlockCipher(new AesEngine())`
3. 随机数：使用 `SecureRandom`
4. tag 长度：固定 16 字节，即 128 bit
5. nonce 长度：固定 12 字节

这样文档和当前客户端代码保持一致：

1. 协议口径仍然是后端文档定义的 `HKDF-SHA256 + AES-256-GCM`
2. 具体实现库则明确为 BouncyCastle
3. 后续如果联调出现 `DEVICE_ENVELOPE_DECRYPT_FAILED`，优先排查 AAD、Base64Url、appSecret、keyVersion 和 appCode/deviceId 是否一致，而不是先怀疑客户端使用了不同算法族

---

## 5. 设备注册链路

### 5.1 入口与触发时机

当前设备注册不是 UI 主动调用，而是后台 worker 在以下场景触发：

1. 使用事件上传前，调用 `EnsureDeviceSessionAsync()`；
2. 心跳与设备 report 发送前，也调用 `EnsureDeviceSessionAsync()`；
3. 仅当当前内存状态 `_deviceSessionReady == false` 时，才真正发起注册或刷新 token。

### 5.2 会话准备分支

`EnsureDeviceSessionAsync()` 当前按以下顺序工作：

1. 若 `_deviceSessionReady == true`，直接返回“设备会话已就绪”；
2. 若本地 `Setting.DeviceId` 和 `Setting.DeviceToken` 都有值，先调用 `RefreshTokenAsync()`；
3. 若 token 刷新成功，则本轮不再重新注册；
4. 若 token 刷新失败，则回退到 `RegisterCurrentDeviceAsync()`；
5. 若本地一开始就没有 `DeviceId` / `DeviceToken`，直接走 `RegisterCurrentDeviceAsync()`；
6. 注册成功后，设置 `_deviceSessionReady = true`。

### 5.3 注册请求

客户端实现类：`DeviceRegisterClient`

加密封装实现类：`DeviceRequestCryptoService`

请求信息：

1. URL：`POST /api/devices/register`
2. Header：
    - `X-Device-AppCode`
    - `X-Device-Id`
    - `X-Device-Nonce`
    - `X-Device-Alg=A256GCM`
    - `X-Device-KeyVersion`
3. 超时：15 秒
4. Body：`DeviceEncryptedEnvelope { ciphertext, tag }`

请求体模型：`DeviceRegisterRequest`

| 字段 | 来源 | 说明 |
|------|------|------|
| `deviceId` | `Setting.DeviceId` > `identity.MachineCode` > `identity.ClientId` | 后续设备管理接口路径标识 |
| `name` | `identity.MachineName` 或 `Environment.MachineName` | 设备或软件实例名称 |
| `deviceType` | 固定值 `amcontrol` | 当前客户端类型标识 |
| `appCode` | `identity.AppCode` 或 `DesktopAppCode` | 设备接入安全上下文 |
| `machineCode` | `SysClientIdentityEntity.MachineCode` | 设备编码 |
| `ipAddress` | 本机首个可用 IPv4 | 仅用于诊断与展示 |
| `extra.clientId` | `SysClientIdentityEntity.ClientId` | 客户端实例标识 |
| `extra.machineCode` | `SysClientIdentityEntity.MachineCode` | 设备编码 |
| `extra.machineName` | 当前设备名称 | 与 `name` 一致 |
| `extra.appCode` | `identity.AppCode` 或 `DesktopAppCode` | 应用编码 |
| `extra.siteCode` | `LicenseSiteCode` | 站点辅助定位 |

注册加密上下文来源：

1. `appSecret`：`BackendServiceConfigHelper.GetDeviceAppSecret()`；
2. `keyVersion`：`BackendServiceConfigHelper.GetDeviceKeyVersion()`；
3. `nonce`：每次请求随机生成 12 字节；
4. `AAD`：`appCode + "\n" + deviceId + "\n" + nonce + "\n" + alg + "\n" + keyVersion`；
5. `requestKey`：`HKDF-SHA256(appSecret, appCode, "autoinboomgate:device-request:v1:{deviceId}")`。

### 5.4 注册返回与本地处理

返回包装：`DeviceApiResponse<DeviceRegisterResponse>`

当前本地关心的字段：

1. `data.deviceId`
2. `data.deviceToken`
3. `data.registrationAction`
4. `data.registeredAt`

成功后的处理：

1. 调用 `SaveDeviceRegistrationToConfig()`；
2. 将 `DeviceId` 和 `DeviceToken` 写回 `ConfigContext.Instance.Config.Setting`；
3. 立即调用 `AM.Tools.Tools.SaveConfig("config.json", ConfigContext.Instance.Config)` 落盘。

失败后的处理：

1. 返回 `FailSilent`；
2. 由 `UsageUploadWorker` 负责节流记录日志；
3. 下一轮调度再次尝试。

### 5.5 刷新 token 请求

刷新 token 与设备注册复用 `DeviceRegisterClient`：

1. URL：`POST /api/devices/{id}/refresh-token`
2. Header：`X-Device-Token`
3. 返回模型：`DeviceApiResponse<DeviceTokenRefreshResponse>`
4. 首版 refresh-token 不走 AES-GCM；
5. 成功后同样把新的长期 `DeviceToken` 回写 config.json。

---

## 6. 设备心跳链路

### 6.1 入口与触发时机

当前设备心跳由 `UsageUploadWorker.ExecuteDeviceRuntimeOnceAsync()` 调用 `SendHeartbeatAsync()` 触发。

前置条件：

1. 已到心跳周期；
2. `EnsureDeviceSessionAsync()` 成功；
3. 本地已有 `DeviceId` 与 `DeviceToken`。

### 6.2 心跳请求

客户端实现类：`DeviceHeartbeatClient`

请求信息：

1. URL：`POST /api/devices/{id}/heartbeat`
2. Header：
    - `X-Device-Token`
    - `X-Device-AppCode`
    - `X-Device-Id`
    - `X-Device-Nonce`
    - `X-Device-Alg=A256GCM`
    - `X-Device-KeyVersion`
3. 请求体模型：`DeviceHeartbeatRequest`
4. 请求体唯一字段：`statusJson`
5. 实际 HTTP body：先将 `DeviceHeartbeatRequest` 序列化为明文 JSON，再封装成 `DeviceEncryptedEnvelope`

### 6.3 statusJson 组成

`statusJson` 由 `UsageUploadWorker.BuildHeartbeatStatusJson()` 生成，当前包含：

| 字段 | 来源 | 说明 |
|------|------|------|
| `appCode` | `BackendServiceConfigHelper.GetDesktopAppCode()` | 应用编码 |
| `appVersion` | `AM.Tools.Tools.GetAppVersionText()` | 当前程序集版本 |
| `clientId` | `Setting.ClientId` | 客户端身份 |
| `deviceId` | `Setting.DeviceId` | 后端设备标识 |
| `machineCode` | `Setting.MachineCode` | 当前设备编码 |
| `machineName` | `Setting.MachineName` 或 `Environment.MachineName` | 当前设备名称 |
| `licenseValid` | `LicenseRuntimeContext.Current.IsValid` | 当前本地授权是否有效 |
| `licenseId` | `LicenseRuntimeContext.Current.LicenseId` | 当前授权标识 |
| `loggedIn` | `UserContext.CurrentUser != null` | 当前是否有登录用户 |
| `loginName` | `UserContext.CurrentUser.LoginName` | 当前登录用户名 |
| `sampledAt` | `DateTime.UtcNow` | 状态采样时间 |

### 6.4 心跳返回与处理

返回包装：`DeviceApiResponse<object>`

当前本地只关心：

1. HTTP 状态是否成功；
2. 返回包装是否可解析；
3. `success` 是否为 `true`；
4. `message` / `errorCode` / `traceId` 用于日志定位。

失败后的特殊处理：

1. 若失败消息中包含 `DEVICE_TOKEN_EXPIRED`、`DEVICE_TOKEN_REVOKED`、`DEVICE_TOKEN_INVALID`、其它 `DEVICE_TOKEN_*` 或 `DEVICE_ID_MISMATCH`；
2. `UsageUploadWorker` 会把 `_deviceSessionReady` 重新置为 `false`；
3. 节流日志会明确标记“当前设备 token 已失效，将在下轮重建设备会话”；
4. 下一轮再走 token 刷新或重新注册。

---

## 7. 设备信息上报链路

当前设备信息上报分为两类，但最终都落到同一个后端接口：

1. 使用事件上报；
2. 结构化设备 report 上报。

统一 HTTP 写入口：

- `POST /api/devices/{id}/report`

统一客户端实现：

- `DeviceReportClient.ReportAsync(DeviceReportRequest request)`

加密封装实现：

- `DeviceRequestCryptoService.BuildReportPackage()`

### 7.1 使用事件上报

#### 7.1.1 触发路径

路径如下：

1. `UsageUploadWorker.ExecuteUsageUploadOnceAsync()`
2. `UsageEventBufferService.QueryPending(BatchSize)`
3. `UsageReportService.UploadBatchAsync(list)`
4. 每条使用事件映射为一条 `DeviceReportRequest`
5. 交给 `DeviceReportClient.ReportAsync()` 发送

#### 7.1.2 请求字段来源

使用事件上报请求模型：`DeviceReportRequest`

当前映射字段如下：

| 字段 | 来源 |
|------|------|
| `eventId` | `SysUsageEventBufferEntity.EventId` |
| `reportType` | 由 `ResolveReportType(eventType)` 推导 |
| `appCode` | `SysUsageEventBufferEntity.AppCode` |
| `appVersion` | `SysUsageEventBufferEntity.AppVersion` |
| `clientId` | `SysUsageEventBufferEntity.ClientId` |
| `machineCode` | `SysUsageEventBufferEntity.MachineCode` |
| `machineName` | `SysUsageEventBufferEntity.MachineName` |
| `userId` | `SysUsageEventBufferEntity.UserId` |
| `loginName` | `SysUsageEventBufferEntity.LoginName` |
| `pageKey` | `SysUsageEventBufferEntity.PageKey` |
| `isSuccess` | `SysUsageEventBufferEntity.IsSuccess` |
| `failReasonCode` | `SysUsageEventBufferEntity.FailReasonCode` |
| `traceId` | `SysUsageEventBufferEntity.TraceId` |
| `occurredAt` | `SysUsageEventBufferEntity.OccurredTime` |
| `payload` | 当前构造的匿名对象 |

`payload` 当前包含：

1. `source = UsageEventBuffer`
2. `eventType`
3. `uploadCategory = DesktopUsage`
4. `localOccurredTime`
5. `localBufferId`

#### 7.1.3 上报结果处理

当前策略是整批处理：

1. 若整批全部成功，则 `MarkUploaded(uploadedIds)`；
2. 若其中任一条失败，则整批 `MarkUploadFailed(failIds, message)`；
3. 失败时不影响主线程，仅由后台记录节流日志。

### 7.2 结构化设备 report 上报

#### 7.2.1 本地缓冲模型

结构化 report 不落数据库，当前使用 `DeviceReportBufferService` 维护进程内队列：

1. `PendingQueue`：静态队列，存放待上传 `DeviceReportRequest`；
2. `Enqueue()`：写入通用 report；
3. `EnqueueAppStart()`：写入应用启动事件；
4. `EnqueueLicenseApplied()`：写入授权申请成功事件；
5. `DequeueBatch()`：按批次取出待上传 report；
6. `EnqueueMany()`：上传失败时把剩余 report 重新入队。

#### 7.2.2 触发路径

路径如下：

1. `UsageUploadWorker.ExecuteDeviceRuntimeOnceAsync()`
2. `FlushDeviceReportsAsync()`
3. `DeviceReportBufferService.DequeueBatch(DeviceReportBatchSize)`
4. 逐条调用 `DeviceReportClient.ReportAsync()`
5. 若中途失败，则 `EnqueueMany(剩余未发送项)`

#### 7.2.3 结构化 report 请求来源

`DeviceReportBufferService.BuildRequest()` 当前自动填充以下字段：

| 字段 | 来源 |
|------|------|
| `eventId` | `BuildEventId()` |
| `reportType` | 调用方传入 |
| `appCode` | `identity.AppCode` 或 `DesktopAppCode` |
| `appVersion` | 当前程序集版本 |
| `clientId` | `SysClientIdentityEntity.ClientId` |
| `machineCode` | `SysClientIdentityEntity.MachineCode` |
| `machineName` | `SysClientIdentityEntity.MachineName` |
| `userId` | `UserContext.CurrentUser.Id` |
| `loginName` | `UserContext.CurrentUser.LoginName` |
| `pageKey` | 调用方传入 |
| `isSuccess` | 调用方传入 |
| `traceId` | `AM.Tools.Tools.Guid(16)` |
| `occurredAt` | `DateTime.UtcNow` |
| `payload` | 调用方传入 |

### 7.3 report 接口返回与处理

`DeviceReportClient` 对使用事件上报和结构化 report 上报使用同一套返回处理逻辑：

1. HTTP 状态非成功，判定失败；
2. 返回 JSON 无法解析，判定失败；
3. `DeviceApiResponse.success == false`，判定失败；
4. 成功时不再额外处理返回 `data`，只记成功消息；
5. 实际发送前统一先做 AES-GCM envelope 封装，再附加安全头与长期 `DeviceToken`。

---

## 8. 配置、身份与关键运行时状态

### 8.1 BackendServiceUrl

`BackendServiceUrl` 是三条链路的统一根地址，由 `BackendServiceConfigHelper.GetBackendServiceUrl()` 读取。

### 8.2 ClientId / MachineCode / MachineName

这些身份字段由 `ClientIdentityService.GetCurrent()` 统一解析，优先级如下：

1. 内存缓存；
2. `config.json`；
3. 本地身份表 `sys_client_identity`；
4. 自动创建默认身份。

### 8.3 DeviceId / DeviceToken

当前两者都保存在 `ConfigContext.Instance.Config.Setting` 中：

1. `DeviceId`：设备管理接口中的设备路径标识；
2. `DeviceToken`：设备管理接口中的长期鉴权 token；
3. 注册与刷新 token 成功后都会重新写回 `config.json`。

### 8.4 DeviceAppSecret / DeviceKeyVersion

设备写接口的加密配置当前同样保存在 `ConfigContext.Instance.Config.Setting` 中：

1. `DeviceAppSecret`：设备端与后端共享的应用密钥明文；
2. `DeviceKeyVersion`：当前应用密钥版本；
3. 两者由 `BackendServiceConfigHelper` 统一读取；
4. register、heartbeat、report 的 AES-GCM 请求封装都依赖这两个值。

### 8.5 _deviceSessionReady

`_deviceSessionReady` 是 `UsageUploadWorker` 的内存态标记，不会持久化。它的含义是：

1. 当前进程是否认为设备会话已准备好；
2. 为 `true` 时，本轮跳过注册/刷新；
3. 为 `false` 时，下轮重新走刷新或注册；
4. 心跳/report 返回 token 相关错误时会被重置为 `false`。

---

## 9. 失败处理与重试策略

### 9.1 统一原则

当前后台链路统一遵循以下原则：

1. 失败只记录日志，不中断主流程；
2. 无 UI 阻塞；
3. 无同步重试风暴；
4. 依赖下一轮周期自动恢复；
5. 对重复错误做节流日志输出。

### 9.2 注册 / 刷新 token

1. 刷新失败时，当前实现会回退到重新注册；
2. 注册失败时，由下一轮周期再尝试；
3. 对明确的 `DEVICE_TOKEN_EXPIRED`、`DEVICE_TOKEN_REVOKED`、`DEVICE_TOKEN_INVALID` 会在日志中标记为“令牌已失效，需要重建设备会话”；
4. 不做本轮多次立即重试。

### 9.3 使用事件上报

1. 一批事件里任意一条失败，整批都标记为上传失败；
2. 后续由外层周期继续处理；
3. 当前不做“批内部分成功”的拆分确认。

### 9.4 结构化 report 上报

1. 当前批次按顺序逐条上传；
2. 中途失败时，把当前失败项及其后的剩余项重新入队；
3. 已成功发送的前半段不再重复入队。

---

## 10. 问题定位建议

### 10.1 设备始终未注册

优先检查：

1. `BackendServiceUrl` 是否已配置；
2. `UsageUploadWorker.Start()` 是否实际启动；
3. `EnsureDeviceSessionAsync()` 是否持续失败；
4. `DeviceRegisterClient.RegisterAsync()` 的返回消息与后端 `traceId`。

### 10.2 心跳不上报

优先检查：

1. `HeartbeatIntervalMs` 是否已到期；
2. `DeviceId` / `DeviceToken` 是否为空；
3. `BuildHeartbeatStatusJson()` 是否生成了有效 JSON；
4. 是否出现 `DEVICE_TOKEN_*` 或 `DEVICE_ID_MISMATCH`。

### 10.3 使用事件不上传

优先检查：

1. `EnableUsageReport` 是否为 `true`；
2. `UsageEventBufferService.QueryPending()` 是否查到数据；
3. `UsageReportService.BuildDeviceReportRequest()` 是否成功；
4. `MarkUploadFailed()` 的失败信息。

### 10.4 结构化 report 丢失

优先检查：

1. 是否已成功调用 `DeviceReportBufferService.Enqueue()`；
2. `PendingQueue` 中是否确实有待发送项；
3. `FlushDeviceReportsAsync()` 是否中途失败并重入队；
4. 进程退出前是否还有未 flush 的内存队列数据。

---

## 11. 关键代码定位清单

| 主题 | 文件 | 关键方法 |
|------|------|----------|
| 后台工作单元注册 | `AM.App/Bootstrap/AppBootstrap.cs` | `Initialize()` |
| 后台统一调度 | `AM.DBService/Services/System/UsageUploadWorker.cs` | `Start` / `ExecuteCycleAsync` / `EnsureDeviceSessionAsync` |
| 设备写请求加密封装 | `AM.DBService/Services/System/DeviceRequestCryptoService.cs` | `BuildRegisterPackage` / `BuildHeartbeatPackage` / `BuildReportPackage` |
| 设备注册 | `AM.DBService/Services/System/DeviceRegisterClient.cs` | `RegisterCurrentDeviceAsync` / `RegisterAsync` / `RefreshTokenAsync` |
| 设备心跳 | `AM.DBService/Services/System/DeviceHeartbeatClient.cs` | `SendHeartbeatAsync` |
| report HTTP 上报 | `AM.DBService/Services/System/DeviceReportClient.cs` | `ReportAsync` |
| 使用事件映射为 report | `AM.DBService/Services/System/UsageReportService.cs` | `UploadBatchAsync` / `BuildDeviceReportRequest` |
| 结构化 report 本地缓冲 | `AM.DBService/Services/System/DeviceReportBufferService.cs` | `Enqueue` / `DequeueBatch` / `EnqueueMany` |
| 客户端身份来源 | `AM.DBService/Services/System/ClientIdentityService.cs` | `GetCurrent` |
| 后端地址与应用配置 | `AM.DBService/Services/System/BackendServiceConfigHelper.cs` | `GetBackendServiceUrl` / `GetDesktopAppCode` |

---

## 12. 与其他文档的关系

建议阅读顺序：

1. [software-license-design.md](software-license-design.md)
2. [license-file-validation-flow.md](license-file-validation-flow.md)
3. [license-runtime-integration.md](license-runtime-integration.md)
4. [device-license-and-reporting.md](device-license-and-reporting.md)
5. 本文档

这样可以先理解授权，再理解设备注册、心跳和 report 的后台实现闭环。