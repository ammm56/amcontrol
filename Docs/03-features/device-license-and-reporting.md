# 设备软件授权申请与设备信息上报说明

**文档编号**：FEAT-DEVICE-001  
**版本**：1.0.0  
**状态**：实现前最终版  
**最后更新**：2026-04-17  
**维护人**：Am

---

## 1. 文档目标

本文档用于从设备软件联调视角，统一说明以下两条链路：

1. 设备软件如何调用后端授权接口申请 License；
2. 设备软件如何调用设备管理接口进行注册、心跳与信息上报。

本文档面向接下来的设备端开发、后端接口联调和问题排查，重点强调：

- 当前已经落地的真实接口；
- 设备软件请求与响应格式；
- 设备侧收到授权响应后，仅保存 `data.licenseText` 的本地落盘方式；
- 设备管理链路与授权链路之间的关系；
- 当前最小闭环范围与非目标。

---

## 2. 当前真实范围

### 2.1 已落地的设备软件授权最小闭环

当前设备软件授权侧的真实范围包括：

1. 软件应用配置；
2. 应用页面配置；
3. 默认授权模板配置；
4. 授权记录保存与查询；
5. 设备软件调用 `/api/license/apply` 申请授权；
6. 命中默认模板时直接生成授权数据；
7. 未命中模板时返回待管理员处理结果。

当前后端已存在的核心接口包括：

- `/api/software-apps*`
- `/api/software-license-templates`
- `/api/software-license-records`
- `/api/license/apply`

### 2.2 已落地的设备管理接口

当前设备管理侧已落地：

1. 设备注册；
2. 显式刷新 `DeviceToken`；
3. 设备心跳；
4. 设备结构化信息上报；
5. 设备分页查询；
6. 状态事件查询；
7. 上报历史查询；
8. 心跳历史查询；
9. 离线检测；
10. 月归档与归档查询。

当前已存在的核心接口包括：

- `/api/devices/register`
- `/api/devices/{id}/refresh-token`
- `/api/devices/{id}/heartbeat`
- `/api/devices/{id}/report`
- `/api/devices*`

---

## 3. 设备软件授权申请链路

## 3.1 目标

设备软件启动后，应能够向后端授权服务提交软件信息、硬件信息和环境信息，请求一份当前设备可用的 License 数据。

当前阶段设备侧的处理约定为：

1. 设备调用 `/api/license/apply`；
2. 后端返回统一授权响应包装；
3. 设备将成功授权响应中的 `data.licenseText` 保存到根目录 `license.lic`；
4. 下次启动时读取 `license.lic`；
5. 解密后得到 JSON 明文；
6. 验签并校验硬件信息；
7. 校验通过则启用授权页面；
8. 校验失败则退回默认最小功能模式。

---

## 3.2 授权申请接口

### 请求路径

- `POST /api/license/apply`

### 请求体示例

```json
{
  "requestId": "apply-20260417-0001",
  "requestTime": "2026-04-17T09:30:00Z",
  "licenseProtocolVersion": "1.0",
  "software": {
    "appCategory": "MotionControl",
    "appCode": "AMControlWinF",
    "appName": "AM Motion Control",
    "appEdition": "Professional",
    "appVersion": "1.2.3",
    "targetFramework": ".NET Framework 4.6.1",
    "uiPlatform": "WinForms",
    "vendor": "AM"
  },
  "device": {
    "clientId": "client-001",
    "machineCode": "MC-001",
    "machineName": "ASSEMBLY-LINE-01",
    "machineModel": "AM-STD-01",
    "osVersion": "Windows 10",
    "cpuId": "BFEBFBFF000906EA",
    "biosSerialNumber": "ABC123456789",
    "mainboardSerialNumber": "MB-987654321",
    "diskSerialNumber": "SSD-1234567890",
    "macAddress": "00-1A-2B-3C-4D-5E"
  },
  "environment": {
    "siteCode": "SZ01",
    "customerCode": "CUS-10001",
    "networkMode": "Online"
  },
  "signature": {
    "algorithm": "SHA256",
    "contentSha256": "5d41402abc4b2a76b9719d911017c592"
  }
}
```

### 关键字段说明

1. `requestId`：必填，设备重试时必须保持不变；
2. `software.appCode`：必填，是模板匹配主键之一；
3. `software.appEdition`：建议传，用于匹配默认模板；
4. `software.appVersion`：建议稳定传递，作为后端判断模板 `minAppVersion` / `maxAppVersion` 是否命中的当前运行版本；
5. `device.clientId`：必填，是设备软件实例唯一标识；
6. `device.machineModel`：建议传，用于模板匹配；
7. `environment.customerCode` / `siteCode`：建议传，用于模板匹配；
8. `signature.contentSha256`：当前阶段可先按最小闭环透传摘要值。

授权版本口径补充：

1. license 明文中的 `software.minAppVersion` / `software.maxAppVersion` 用于描述允许的软件版本区间；
2. 设备侧运行时版本取自 `AM.Tools.Tools.GetAppVersionText()`，当前格式为 `major.minor.build`；
3. 所有授权版型统一按 `[minAppVersion, maxAppVersion]` 闭区间判断当前程序版本是否命中；
4. 后端签发的 `licenseText.software` 应只下发 `minAppVersion` / `maxAppVersion` 作为授权版本约束，不再下发用于授权判定的单值 `appVersion`；
5. 设备侧不再回退旧的 `software.appVersion` 精确匹配；缺失 `minAppVersion` 或 `maxAppVersion` 时直接判定授权无效。

### requestId 规则

设备侧实现前应固定以下 `requestId` 规则：

1. 同一次申请的重试必须复用同一个 `requestId`；
2. 新的一次申请才生成新的 `requestId`；
3. 推荐格式：`apply-{yyyyMMddHHmmss}-{随机短码}`；
4. `requestId` 是授权申请幂等键，不等同于 `traceId`；
5. 当后端返回 `LicensePending` 时，设备侧应保留 `requestId` 便于联调和审计。

### 当前真实成功响应

当前设备端必须按真实返回结构处理成功响应：

```json
{
  "success": true,
  "data": {
    "licenseId": "LIC-20260417111557-636f844c28",
    "status": "Issued",
    "licenseText": "{\"licenseId\":\"LIC-20260417111557-636f844c28\",...}",
    "issuedAt": "2026-04-17T11:15:57.7315933Z",
    "expiresAt": "2027-04-17T11:15:57.7315933Z"
  },
  "message": null,
  "errorCode": null,
  "traceId": "0HNKSD62133I5:00000001",
  "errors": null
}
```

设备侧固定处理约束：

1. 顶层是统一 API 包装；
2. 真正授权结果位于 `data`；
3. `data.licenseText` 是字符串；
4. `license.lic` 固定保存 `data.licenseText`；
5. `issuedAt`、`expiresAt` 可用于提示，但正式校验仍以 `licenseText.validity` 为准。

---

## 3.3 授权申请结果语义

### 模板命中时

后端返回：

- `success = true`
- 含 `licenseText`
- 授权记录状态为 `Issued`

模板命中后的签发口径：

1. 后端用设备申请中的 `software.appVersion` 与模板 `minAppVersion` / `maxAppVersion` 做范围匹配；
2. 命中后写入 `licenseText.software.minAppVersion` / `licenseText.software.maxAppVersion`；
3. 签发结果不再写入用于授权判定的单值 `licenseText.software.appVersion`。

### 模板未命中时

后端返回：

- `success = false`
- `errorCode = LicensePending`
- 授权记录状态为 `Pending`

设备端应把 `LicensePending` 当作业务待处理结果，而不是系统崩溃错误。

当前推荐错误处理方式：

1. 保留本次 `requestId`、`clientId`、`machineCode`、`traceId` 到日志；
2. 不写入新的 `license.lic`；
3. 程序继续使用本地旧授权或退回最小功能模式；
4. 前端或日志消息统一提示“待管理员配置默认模板”。

---

## 3.4 设备侧收到授权后的处理

当前阶段设备侧必须遵守以下规则：

1. 成功收到授权数据后，将 `data.licenseText` 保存到根目录 `license.lic`；
2. 当前阶段不把授权信息写入本地数据库；
3. 程序启动时优先读取 `license.lic`；
4. 先解密，再得到 JSON 明文；
5. 验证 RSA 签名；
6. 重新读取当前硬件信息并与授权绑定信息比较；
7. 校验有效期；
8. 成功则载入运行时授权上下文；
9. 失败则退回默认最小功能页面集。

首版校验定稿如下：

1. 启动期正式启用 `RSA-SHA256` 验签；
2. 验签公钥固定为程序代码内置的 PEM 文本默认值；
3. 首版不从 `config.json`、数据库或外部 Key 文件读取公钥；
4. 首版固定单一 KeyId，建议值为 `AMCONTROL_RSA_V1`；
5. 拦截性硬件强校验字段固定为 `clientId`、`machineCode`、`cpuId`；
6. `machineName`、`biosSerialNumber`、`mainboardSerialNumber`、`diskSerialNumber`、`macAddress` 只用于诊断日志，不作为首版拦截条件。

---

## 4. 设备管理链路

## 4.1 设备接入目标

设备管理链路用于让设备或设备软件在平台中具备：

1. 注册身份；
2. 在线状态；
3. 心跳保活；
4. 业务上报；
5. 状态历史与事件历史可查询。

设备管理链路与设备软件授权链路是**并行关系**，不是强耦合关系。

当前 `/api/license/apply` 不要求先调用设备注册接口，但在真实业务联调中，建议先完成设备注册，再保持：

- `clientId`
- `machineCode`
- `machineName`

在设备注册、心跳、上报和授权申请链路中尽量保持一致，便于后续审计和问题排查。

---

## 4.2 设备注册

### 请求路径

- `POST /api/devices/register`

### 请求体示例

```json
{
  "deviceId": "DEV-001",
  "name": "Boom Gate A",
  "deviceType": "amcontrol",
  "ipAddress": "192.168.1.10",
  "extra": {
    "siteCode": "SITE-001",
    "laneCode": "LANE-01"
  }
}
```

### 成功响应示例

```json
{
  "success": true,
  "data": {
    "deviceId": "DEV-001",
    "deviceToken": "<token>",
    "registrationAction": "Registered",
    "registeredAt": "2026-04-17T08:00:00Z"
  },
  "message": null,
  "errorCode": null,
  "traceId": "00-abc123...",
  "errors": null
}
```

### 说明

1. 首次注册成功返回 `Registered`；
2. 已存在设备重连成功可返回 `Reconnected`；
3. 设备端必须保存最新的 `deviceToken`；
4. 后续心跳、上报、刷新令牌都依赖 `X-Device-Token`。

---

## 4.3 刷新令牌

### 请求路径

- `POST /api/devices/{id}/refresh-token`

### 请求头

- `X-Device-Token: <current-token>`

### 成功响应示例

```json
{
  "success": true,
  "data": {
    "deviceId": "DEV-001",
    "deviceToken": "<new-token>",
    "refreshedAt": "2026-04-17T08:30:00Z"
  },
  "message": null,
  "errorCode": null,
  "traceId": "00-abc123...",
  "errors": null
}
```

### 说明

1. 刷新接口不是匿名接口；
2. 必须携带当前有效 `deviceToken`；
3. 收到新 token 后设备端应立即覆盖旧 token。

### Token 头固定规则

设备侧实现前应固定以下规则：

1. `refresh-token`、`heartbeat`、`report` 三类接口统一使用 `X-Device-Token`；
2. 不额外引入第二套自定义 Token 头；
3. 刷新成功后必须立即用新 token 覆盖旧 token；
4. 路径中的 `{id}` 必须与 token 主体一致；
5. 设备端不得把管理员 `Bearer Token` 混用于设备写接口。

---

## 4.4 心跳接口

### 请求路径

- `POST /api/devices/{id}/heartbeat`

### 请求头

- `X-Device-Token: <token>`

### 请求体示例

```json
{
  "statusJson": "{\"cpuUsage\":23.5,\"memoryMb\":512,\"temperature\":45.2,\"cameraStatus\":\"capturing\"}"
}
```

### 成功响应示例

```json
{
  "success": true,
  "data": null,
  "message": null,
  "errorCode": null,
  "traceId": "00-abc123...",
  "errors": null
}
```

### 说明

1. `statusJson` 当前为字符串，不是任意对象；
2. 心跳主要承担在线保活和轻量状态快照；
3. 不建议把大体积业务数据塞进心跳请求。

---

## 4.5 结构化上报接口

### 请求路径

- `POST /api/devices/{id}/report`

### 请求头

- `X-Device-Token: <token>`

### 请求体示例

```json
{
  "eventId": "evt-20260417-0001",
  "reportType": "Status",
  "appCode": "AM",
  "appVersion": "1.0.0",
  "clientId": "client-001",
  "machineCode": "MC-001",
  "machineName": "Boom Gate A",
  "userId": 1001,
  "loginName": "operator",
  "pageKey": "dashboard",
  "isSuccess": true,
  "traceId": "trace-001",
  "occurredAt": "2026-04-17T08:00:00Z",
  "payload": {
    "mode": "auto",
    "state": "ok"
  }
}
```

### 字段口径

1. `reportType`：首版统一必填；
2. `payload`：首版统一必填；
3. `eventId`：建议传，用于去重；
4. `occurredAt`：建议传，用于对齐业务发生时间；
5. `appCode`、`clientId`、`machineCode`、`machineName`：建议与授权申请链路保持一致口径；
6. `Alarm` / `Event` 当前不作为公共设备上报接口支持类型。

### 成功与冲突语义

- 首次成功：正常返回 `success=true`
- `eventId` 重复：返回 `409 DEVICE_REPORT_EVENT_ID_CONFLICT`

---

## 4.6 设备写接口的错误语义

当前设备写接口联调时应关注以下错误码：

1. `DEVICE_TOKEN_MISSING`
2. `DEVICE_TOKEN_INVALID`
3. `DEVICE_TOKEN_EXPIRED`
4. `DEVICE_ID_MISMATCH`
5. `DEVICE_ALREADY_REGISTERED`
6. `DEVICE_REPORT_TYPE_PLUGIN_ONLY`
7. `DEVICE_REPORT_EVENT_ID_CONFLICT`

这些都属于设备侧应明确处理的业务错误，而不是统一当作未知异常。

---

## 5. 设备软件授权与设备上报的推荐联调顺序

如果要做真实联调，建议按以下顺序：

1. 管理员确认平台授权正常；
2. 管理员配置软件应用；
3. 管理员配置应用页面；
4. 管理员配置默认授权模板；
5. 设备先调用 `/api/devices/register`；
6. 设备开始 `heartbeat` 与必要的 `report`；
7. 设备再调用 `/api/license/apply`；
8. 设备收到授权后保存为 `license.lic`；
9. 重启软件，验证 `license.lic` 读取、解密、验签、硬件校验与最小功能回退；
10. 登录后验证页面是否按授权结果收口。

---

## 6. 设备端最小实现建议

当前设备端最小实现建议至少包含以下组件：

1. `DeviceRegisterClient`
2. `DeviceHeartbeatClient`
3. `DeviceReportClient`
4. `DeviceLicenseApplyClient`
5. `LicenseFileService`
6. `LicenseCryptoService`
7. `LicenseValidator`
8. `LicenseRuntimeLoader`
9. `HardwareInfoCollector`
10. `LicenseRuntimeContext`
11. `LicensePagePermissionHelper`

说明：

- 设备管理客户端负责与 `/api/devices/*` 交互；
- 授权客户端负责与 `/api/license/apply` 交互；
- 授权文件与运行时逻辑保持独立，避免和设备上报逻辑混在一个类里。

### 6.1 按项目分层的新增类清单

建议按当前 WinForms 解决方案分层固定新增类：

1. `AM.Model`：授权申请请求/响应模型、`licenseText` 明文模型、硬件信息模型、校验结果模型；
2. `AM.Core.Context`：`LicenseRuntimeContext`；
3. `AM.DBService.Services.System`：`LicenseFileService`、`LicenseCryptoService`、`LicenseValidator`、`LicenseRuntimeLoader`、`HardwareInfoCollector`、`DeviceRegisterClient`、`DeviceHeartbeatClient`、`DeviceReportClient`、`DeviceLicenseApplyClient`、`LicensePagePermissionHelper`；
4. `AMControlWinF`：只负责授权状态提示，不承载授权核心逻辑。

### 6.2 实现阶段顺序

1. Phase A：模型、Context、常量、配置项；
2. Phase B：`license.lic` 文件服务、硬件采集、校验器、运行时装载器；
3. Phase C：设备注册、心跳、上报、授权申请客户端；
4. Phase D：启动期接入 `AppBootstrap.cs`，登录期接入 `AuthService.cs`；
5. Phase E：主界面授权状态提示，`MainWindow.cs` 只做展示。

---

## 7. 当前非目标

以下内容当前不属于首版联调范围：

1. `/api/software-licenses/issue`
2. `/api/software-licenses/renew`
3. `/api/software-licenses/revoke`
4. `/api/software-licenses/activate`
5. `/api/software-licenses/check`
6. 复杂在线校验协议
7. 本地授权数据库缓存
8. 多租户和复杂 License 运营能力

当前设备软件授权真实入口只有：

- `POST /api/license/apply`

---

## 8. 结论

当前设备软件侧开发应明确两条并行链路：

1. **设备管理链路**：注册、刷新令牌、心跳、信息上报；
2. **设备软件授权链路**：申请授权、保存 `license.lic`、启动校验、运行时过滤页面。

两条链路共享部分设备标识信息，但职责不同：

- 设备管理链路解决设备在线与状态观测；
- 授权链路解决软件是否被合法授权以及哪些页面可用。

接下来的设备端开发应按本文档定义的请求字段、返回语义和联调顺序执行，避免再引入与当前真实接口不一致的设计。