# 本地 Native 安全模块设计与落地规划

**文档编号**：FEAT-DEVICE-003  
**版本**：1.0.0  
**状态**：规划中  
**最后更新**：2026-04-21  
**维护人**：Am

---

## 1. 文档目标

本文档用于整理当前设备授权申请、设备注册、设备心跳、设备信息上报相关敏感材料的本地保护改造方案，并将前期讨论形成一套可落地的仓库规划。

本文档重点回答以下问题：

1. 为什么当前 `C# + config.json + 本地 PEM 文件` 方案需要调整；
2. 新的本地 `C++ Native` 安全模块应放在仓库哪个位置；
3. 该模块应加到哪个解决方案中统一构建；
4. 构建产物应输出到哪个运行目录；
5. `C#` 侧应如何以最小改动方式接入；
6. 后续迁移到 `.NET Core / .NET 10` 乃至跨平台 UI 时，当前规划如何复用。

本文档面向当前仓库的第一版落地实现，优先满足以下约束：

1. 设备多数情况下可能离线，不能依赖在线取密钥；
2. 操作员使用时应无额外交互；
3. 现阶段不引入与 Windows 强绑定的安全机制作为前提；
4. 优先降低 `C#` 层直接读取秘密明文的暴露面；
5. 保持对当前 WinForms 主线最小侵入。

---

## 2. 当前问题与改造边界

当前实现中，以下敏感材料仍由 `C#` 直接读取：

1. `DeviceAppSecret`：由 `BackendServiceConfigHelper.GetDeviceAppSecret()` 从运行期配置明文读取；
2. 授权申请签名私钥：由 `DeviceLicenseApplyClient` 读取本地私钥文件文本；
3. license 验签公钥：由 `LicenseCryptoService` 读取本地公钥文件文本。

当前方案的问题不是“无法运行”，而是以下三个方面：

1. 敏感材料直接出现在 `config.json` 或本地 PEM 文件中，提取门槛过低；
2. 敏感材料进入 `C#` 托管字符串后，暴露面较大；
3. 后续若转向 `.NET Core / .NET 10` 和前端壳，现有做法无法形成统一的本地安全边界。

本次规划不追求“客户端绝对不可提取秘密”，因为只要客户端本机必须完成签名或加密运算，就不可能做到绝对隐藏。

本次规划追求的边界是：

1. `C#` 层不再直接获取 `DeviceAppSecret` 明文；
2. `C#` 层不再直接获取授权申请私钥文本；
3. `C#` 层不再直接读取本地验签公钥文件；
4. `C#` 只调用“签名 / 加密 / 验签”操作接口；
5. 敏感材料收口到本地 `Native` 模块内部。

---

## 3. 设计原则

### 3.1 第一版原则

第一版实现遵循以下原则：

1. 先做本地离线可运行版本；
2. 先做最简单、最稳的工程结构，不先追求多平台发布链；
3. `Native` 模块只暴露操作接口，不暴露“取秘密字符串”接口；
4. WinForms 主线先打通，WPF 和未来新 UI 形态后续复用；
5. 不采用 `C++/CLI`，避免把桥接层绑定死在 Windows 托管互操作实现上。

### 3.2 导出接口原则

`Native` 模块对外只暴露以下类型的能力：

1. 构建设备注册密文请求包；
2. 构建设备心跳密文请求包；
3. 构建设备上报密文请求包；
4. 对授权申请载荷做签名；
5. 对本地 license 做验签。

明确禁止导出以下能力：

1. `GetDeviceAppSecret`；
2. `GetPrivateKeyPem`；
3. `GetPublicKeyPem`；
4. 任意导出本地敏感材料明文的接口。

### 3.3 跨平台演进原则

为了兼容未来 `.NET Core / .NET 10` 与前端技术栈，`Native` 安全模块当前应保持以下约束：

1. 使用纯 `Native C/C++` 动态库；
2. 对外使用稳定的 `C ABI` 导出函数；
3. `C#` 侧使用 `P/Invoke`；
4. 不使用 `C++/CLI`；
5. 输入输出优先采用 `JSON 字符串`，降低跨语言结构体对齐成本。

---

## 4. 仓库目录与项目结构

### 4.1 新增项目目录

新的 `C++ Native` 项目建议放在仓库根目录，与现有 `AM.Core`、`AM.Model`、`AM.DBService`、`AM.MotionService` 等项目同级。

推荐目录：

`AM.SecureNative`

原因如下：

1. 该模块不属于 WinForms UI 专属逻辑；
2. 该模块不属于 `AM.DBService` 的内部细节实现；
3. 与当前仓库的分层和平级项目结构一致；
4. 后续若 WPF 或未来新宿主复用，无需重新搬迁目录。

### 4.2 推荐的第一版目录草图

```text
AM.SecureNative/
  AM.SecureNative.vcxproj
  AM.SecureNative.vcxproj.filters
  exports/
    am_secure_runtime_exports.h
    am_secure_runtime_exports.cpp
  facade/
    secure_runtime_facade.h
    secure_runtime_facade.cpp
  crypto/
    crypto_engine.h
    crypto_engine.cpp
  secrets/
    secret_provider.h
    secret_provider.cpp
  json/
    json_serializer.h
    json_serializer.cpp
  utils/
    base64url_utils.h
    base64url_utils.cpp
  resources/
    secure_materials.dat
    secure_materials.h
    secure_materials.rc
  README.md
```

### 4.3 各目录职责

1. `exports/`：仅放 DLL 导出函数与导出层薄封装；
2. `facade/`：汇总内部流程，避免导出层直接耦合加密细节；
3. `crypto/`：放 `HKDF`、`AES-GCM`、`RSA` 签名与验签实现；
4. `secrets/`：放本地敏感材料读取、解包和生命周期管理；
5. `json/`：放输入输出 JSON 序列化；
6. `utils/`：放 Base64Url、字符串、敏感内存清理等工具；
7. `resources/`：放第一版本地嵌入或附带的加密材料文件。

---

## 5. 应加入的解决方案

### 5.1 第一优先方案

第一版建议先将 `AM.SecureNative` 加入以下解决方案：

1. `AMControlWinF.sln`

原因如下：

1. 当前活跃主线是 WinForms；
2. 当前设备联调、运行和功能验证主要集中在 WinForms 启动链；
3. 先保证当前活跃分支一键构建可用，比一开始同步所有解决方案更稳。

### 5.2 第二阶段可选方案

后续如果需要整仓统一入口，可再补充加入：

1. `AMControl.sln`

当前不建议第一步就同步加入：

1. `AMControlWPF.sln`

原因是 WPF 当前不是活跃开发线，先不让它增加 native 工程配置复杂度。

### 5.3 构建关系建议

在 `AMControlWinF.sln` 中，建议建立以下构建关系：

1. `AM.SecureNative` 先构建；
2. `AMControlWinF` 作为最终启动项目后构建；
3. 构建后将 native DLL 复制到 WinForms 运行目录。

---

## 6. 运行目录与部署路径

### 6.1 Native DLL 输出位置

第一版最稳妥的做法是将 native DLL 放在 WinForms 可执行文件同目录。

推荐运行目录示意：

```text
AMControlWinF/
  bin/
    x64/
      Debug/
        AMControlWinF.exe
        am_secure_runtime.dll
        Configuration/
          config.json
          Security/
            secure_materials.dat
```

### 6.2 为什么不建议第一版把 DLL 放进 Configuration 子目录

原因如下：

1. `P/Invoke` 默认优先从应用程序目录加载 DLL；
2. 将 DLL 放在 exe 同目录时，加载路径最简单；
3. 可以避免第一版就引入复杂的自定义搜索路径和手工 `LoadLibrary`；
4. 更适合后续迁移到新的 .NET 宿主。

### 6.3 本地敏感材料文件位置

如果第一版选择“DLL + 本地加密 blob”方案，则建议将附带的安全材料文件放在：

`Configuration/Security/secure_materials.dat`

如果第一版选择“全部嵌入 DLL 资源”方案，则可以暂时没有该文件。

---

## 7. Native 模块对外接口边界

### 7.1 第一版最小导出接口

第一版建议只保留以下 6 个导出函数：

1. `am_secure_build_register_envelope`
2. `am_secure_build_heartbeat_envelope`
3. `am_secure_build_report_envelope`
4. `am_secure_sign_license_apply_payload`
5. `am_secure_verify_license_signature`
6. `am_secure_free_buffer`

### 7.2 输入输出形式

第一版建议统一采用以下模式：

1. `C#` 向 native 传入 JSON 字符串；
2. native 返回统一 JSON 字符串；
3. 返回结构统一带 `success`、`code`、`message`、`data`；
4. native 分配的返回缓冲区由 `am_secure_free_buffer` 释放。

### 7.3 为什么第一版不引入复杂上下文句柄

第一版不建议先做 `CreateContext/DestroyContext` 之类的上下文句柄模型，原因如下：

1. 当前目标是先打通最小闭环；
2. 当前核心操作是无状态的签名、加密和验签；
3. 无状态接口更容易从 WinForms 迁移到未来新的宿主；
4. 可以降低第一版联调复杂度。

---

## 8. C# 侧接入方式

### 8.1 总体策略

`C#` 侧不直接调用 native 细节，而是新增一层托管适配层，统一管理 `P/Invoke`、路径检查、异常包装和结果映射。

### 8.2 建议新增的 C# 文件

第一版建议在 `AM.DBService/Services/System` 下新增以下文件：

```text
AM.DBService/
  Services/
    System/
      NativeSecureRuntimeClient.cs
      NativeSecureRuntimeLoader.cs
      NativeSecureRuntimeModels.cs
      NativeSecureRuntimeInterop.cs
      NativeSecureRuntimeConstants.cs
      NativeSecureRuntimeExceptionHelper.cs
```

### 8.3 各文件职责

1. `NativeSecureRuntimeInterop.cs`：集中定义 `DllImport`；
2. `NativeSecureRuntimeLoader.cs`：定位 DLL、检查存在性、包装加载错误；
3. `NativeSecureRuntimeModels.cs`：定义与 native JSON 对应的托管 DTO；
4. `NativeSecureRuntimeConstants.cs`：统一库名、默认路径、错误码常量；
5. `NativeSecureRuntimeExceptionHelper.cs`：将 native 加载异常映射为统一错误消息；
6. `NativeSecureRuntimeClient.cs`：对上提供托管友好的业务方法。

### 8.4 为什么第一版推荐静态 P/Invoke

第一版最稳妥的加载方式是：

1. native DLL 放 exe 同目录；
2. `C#` 使用静态 `DllImport("am_secure_runtime.dll")`；
3. 外围增加文件存在性和异常包装；
4. 暂不自己手工维护复杂 `LoadLibrary` 逻辑。

这样做的原因：

1. 当前运行环境固定为 Windows；
2. 当前目标是最小改动先跑通；
3. `P/Invoke + exe 同目录` 是第一版最稳的路径；
4. 后续迁移到新的 .NET 版本时，再演进为更灵活的加载器。

---

## 9. 当前仓库中的改造点

### 9.1 新增的托管桥入口

第一版建议新增统一托管入口类：

1. `NativeSecureRuntimeClient`

建议暴露的方法：

1. `BuildRegisterPackage(...)`
2. `BuildHeartbeatPackage(...)`
3. `BuildReportPackage(...)`
4. `SignLicenseApplyPayload(...)`
5. `VerifyLicenseSignature(...)`

### 9.2 DeviceRequestCryptoService

文件：

1. `AM.DBService/Services/System/DeviceRequestCryptoService.cs`

改造建议：

1. 保留现有服务类和对外方法名，减少调用面波动；
2. 将 `BuildRegisterPackage`、`BuildHeartbeatPackage`、`BuildReportPackage` 的内部实现改为委托给 `NativeSecureRuntimeClient`；
3. 将 `HKDF`、`AES-GCM`、随机数与 Base64Url 等底层实现移出 `C#`；
4. `ResolveRegisterAppCode` 等纯业务字段解析逻辑可继续保留在 `C#`。

### 9.3 DeviceLicenseApplyClient

文件：

1. `AM.DBService/Services/System/DeviceLicenseApplyClient.cs`

改造建议：

1. 保留当前申请请求组织逻辑；
2. 将签名计算改为调用 `NativeSecureRuntimeClient.SignLicenseApplyPayload(...)`；
3. 废弃 `LoadRequestSigningPrivateKeyText()` 这类私钥文本读取入口；
4. 保持现有后端调用、日志和返回处理逻辑不变。

### 9.4 LicenseCryptoService

文件：

1. `AM.DBService/Services/System/LicenseCryptoService.cs`

改造建议：

1. 保留当前本地授权文本解码和前置校验逻辑；
2. 将真正的 RSA 验签改为调用 `NativeSecureRuntimeClient.VerifyLicenseSignature(...)`；
3. 废弃公钥 PEM 读取与解析逻辑；
4. 将本地验签关键边界统一收口到 native 模块。

### 9.5 BackendServiceConfigHelper

文件：

1. `AM.DBService/Services/System/BackendServiceConfigHelper.cs`

改造建议：

1. 保留后端地址、`AppCode`、授权范围、`DeviceKeyVersion` 等普通业务配置读取；
2. 逐步废弃 `GetDeviceAppSecret()`；
3. 逐步废弃 `GetLicenseValidationPublicKeyFilePath()`；
4. 逐步废弃 `GetLicenseRequestSigningPrivateKeyFilePath()`；
5. 第一版可先保留这些方法，但不再让核心链路依赖它们。

### 9.6 受益于最小改造的现有调用点

以下类在第一版中理论上只需保持原有调用方式，不必大幅重构：

1. `AM.DBService/Services/System/DeviceRegisterClient.cs`
2. `AM.DBService/Services/System/DeviceHeartbeatClient.cs`
3. `AM.DBService/Services/System/DeviceReportClient.cs`
4. `AM.DBService/Services/System/UsageUploadWorker.cs`

原因是：

1. 当前这些类主要依赖 `DeviceRequestCryptoService` 或更高层服务；
2. 只要加密和签名边界在内部切换到 native，这些客户端与 worker 的主流程、日志链和错误传播链可以基本保持不变。

---

## 10. 第一版最小文件清单

### 10.1 Native 项目最小文件集

```text
AM.SecureNative/
  AM.SecureNative.vcxproj
  exports/
    am_secure_runtime_exports.h
    am_secure_runtime_exports.cpp
  facade/
    secure_runtime_facade.h
    secure_runtime_facade.cpp
  crypto/
    crypto_engine.h
    crypto_engine.cpp
  secrets/
    secret_provider.h
    secret_provider.cpp
  json/
    json_serializer.h
    json_serializer.cpp
  utils/
    base64url_utils.h
    base64url_utils.cpp
  resources/
    secure_materials.dat
    secure_materials.h
    secure_materials.rc
  README.md
```

### 10.2 C# 托管桥最小文件集

```text
AM.DBService/
  Services/
    System/
      NativeSecureRuntimeClient.cs
      NativeSecureRuntimeInterop.cs
      NativeSecureRuntimeModels.cs
      NativeSecureRuntimeLoader.cs
```

### 10.3 文档文件

建议同时补充以下文档：

1. 本文档：`Docs/03-features/native-secure-runtime-design.md`
2. 后续实现完成后，可在 `device-management-runtime-flow.md` 与 `license-file-validation-flow.md` 中补一节“native 安全模块接入说明”。

---

## 11. 分阶段实施建议

### 11.1 第一阶段

目标：先把 native 工程、托管桥和运行目录打通。

实施内容：

1. 新建 `AM.SecureNative` 项目；
2. 将项目加入 `AMControlWinF.sln`；
3. 输出 `am_secure_runtime.dll` 到 WinForms 输出目录；
4. 在 `AM.DBService` 下建立 `NativeSecureRuntime*` 托管桥文件。

### 11.2 第二阶段

目标：切换设备 AES-GCM 加密链路。

实施内容：

1. 改造 `DeviceRequestCryptoService`；
2. 保持 `DeviceRegisterClient`、`DeviceHeartbeatClient`、`DeviceReportClient` 上层调用不变；
3. 完成设备注册、心跳、上报的本地联调。

### 11.3 第三阶段

目标：切换授权申请签名与本地 license 验签。

实施内容：

1. 改造 `DeviceLicenseApplyClient`；
2. 改造 `LicenseCryptoService`；
3. 清理 `C#` 侧对私钥、公钥和 `DeviceAppSecret` 的直接读取依赖。

---

## 12. 非目标与风险说明

### 12.1 当前非目标

第一版明确不做以下内容：

1. 不做在线密钥下发；
2. 不做硬件安全模块接入；
3. 不做 TPM、DPAPI、Windows 证书存储强依赖方案；
4. 不做跨平台多产物一次性交付链；
5. 不追求客户端绝对不可逆向。

### 12.2 当前风险

当前方案虽然比明文配置和明文 PEM 更好，但仍有以下事实边界：

1. 只要客户端本机必须完成签名或加密，秘密就不可能做到绝对不可提取；
2. 第一版 `Native` 本地离线方案本质上是在提高提取成本，而不是建立绝对安全边界；
3. 后续如果需要更强的秘密隔离，仍应考虑远程下发、硬件安全模块或平台安全存储。

---

## 13. 相关文档

1. [device-license-and-reporting.md](device-license-and-reporting.md)
2. [device-management-runtime-flow.md](device-management-runtime-flow.md)
3. [license-file-validation-flow.md](license-file-validation-flow.md)
4. [software-license-design.md](software-license-design.md)
5. [document-naming-conventions.md](../00-governance/document-naming-conventions.md)