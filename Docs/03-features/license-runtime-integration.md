# 设备侧运行时上下文与页面过滤接入说明

**文档编号**：FEAT-LICENSE-003  
**版本**：1.0.0  
**状态**：实现前最终版  
**最后更新**：2026-04-17  
**维护人**：Am

---

## 1. 文档目标

本文档用于说明设备侧授权信息在程序启动后如何载入运行时上下文，以及如何将 `pageKeys` 接入当前 WinForms 导航与页面权限体系。

---

## 2. 当前现有权限与导航基础

当前解决方案中，页面导航过滤已具备稳定链路：

1. `UserContext.Instance.CurrentPageKeys`
2. `UserContext.Instance.HasPagePermission(pageKey)`
3. `MainWindowModel.CanAccessPage(...)`
4. `NavigationCatalog`

当前 `MainWindowModel` 的页面过滤规则是：

- 页面可见 = `UserContext.Instance.HasPagePermission(page.PageKey)`

因此，设备授权功能不应重写导航过滤逻辑，而应复用现有 `UserContext.CurrentPageKeys` 作为最终页面权限入口。

---

## 3. 运行时上下文设计建议

建议新增独立的授权运行时上下文，例如：

- `LicenseRuntimeContext`

### 建议职责

1. 保存当前是否存在有效授权；
2. 保存授权是否通过验签；
3. 保存是否处于宽限期；
4. 保存授权模块集合；
5. 保存授权页面集合；
6. 提供授权状态查询能力；
7. 为登录后的页面权限收口提供数据来源。

### 建议保存内容

- `HasLicense`
- `IsSignatureValid`
- `IsValid`
- `IsExpired`
- `IsInGracePeriod`
- `ExpiresAt`
- `ModuleKeys`
- `PageKeys`
- `Message`

---

## 4. 设备侧运行流程建议

### 4.1 启动阶段

建议在程序启动时完成：

1. 读取 `license.lic`；
2. 解密；
3. 验签；
4. 硬件绑定校验；
5. 有效期校验；
6. 将结果写入 `LicenseRuntimeContext`。

此时只负责“获得授权状态”，不直接操作当前登录用户权限。

### 4.2 登录阶段

登录成功后，认证服务会得到当前用户页面权限。

此时应：

1. 从 `UserContext` 或登录结果拿到用户页面权限集合；
2. 从 `LicenseRuntimeContext` 拿到授权页面集合；
3. 对两者做交集；
4. 将交集结果写回 `UserContext.CurrentPageKeys`；
5. 由 `MainWindowModel.LoadNavigation()` 自动过滤可见页面。

推荐固定到当前 WinForms 代码链路中的具体接入点：

1. 启动期装载入口：`AppBootstrap.cs`；
2. 登录后页面权限收口入口：`AuthService.cs` 的登录成功分支；
3. 最终页面权限落点：`UserContext.RefreshPagePermissions(...)` 或 `UserContext.SignIn(...)` 写入的页面集合；
4. `MainWindowModel.CanAccessPage(...)` 保持不变。

---

## 5. 最终页面权限规则

建议最终规则如下：

`最终可见页面 = 当前用户页面权限 ∩ License 授权页面`

说明：

1. 用户无权访问的页面，即使 License 授权，也不能显示；
2. 用户有权限访问的页面，如果 License 不授权，也不能显示；
3. 只有同时满足用户权限和授权许可的页面才可见。

---

## 6. 最小功能模式下的页面处理

当 `LicenseRuntimeContext` 显示当前授权无效时，建议系统退回最小功能模式。

### 最小功能模式建议页面

```json
{
  "defaultOpenPages": [
    "Home.Overview",
    "Home.SysStatus",
    "Motion.DI",
    "Motion.DO",
    "Motion.Monitor",
    "PLC.Status",
    "PLC.Monitor",
    "AlarmLog.Current",
    "AlarmLog.History",
    "AlarmLog.RunLog",
    "System.LoginLog"
  ]
}
```

### 使用方式建议

最小功能模式下，可采用：

- `当前用户页面权限 ∩ 默认开放页面`

这样既保持了最小可用性，又不绕过用户原有角色权限。

---

## 7. 推荐的接入位置

### 7.1 启动接入点

建议在程序启动早期、主窗体显示前完成 License 文件校验，并将结果写入 `LicenseRuntimeContext`。

### 7.2 登录成功后的接入点

建议在以下时机接入页面权限收口：

1. `AuthService.Login(...)` 成功后；
2. `UserContext.SignIn(...)` 之前或之后立即处理；
3. 在 `MainWindowModel.LoadNavigation()` 调用之前完成。

建议当前首版固定为：

1. `AuthService.Login(...)` 查询到角色和页面权限后；
2. 使用 `LicensePagePermissionHelper` 计算最终页面集合；
3. 将最终页面集合写入 `UserContext.SignIn(...)`；
4. 如需刷新已登录用户权限，统一走 `UserContext.RefreshPagePermissions(...)`。

---

## 8. 建议的辅助类

建议后续增加以下辅助类：

### 8.1 LicensePagePermissionHelper

职责：

1. 合并用户权限页与授权页；
2. 处理最小功能模式白名单；
3. 输出最终页面权限集合。

### 8.2 LicenseRuntimeLoader

职责：

1. 从 `license.lic` 读取并生成授权状态；
2. 将授权状态装载到 `LicenseRuntimeContext`；
3. 提供统一初始化入口。

### 8.3 DeviceLicenseApplyClient

职责：

1. 按固定请求体调用 `POST /api/license/apply`；
2. 解析统一 API 包装；
3. 成功时返回 `LicenseApplyResponse`；
4. `LicensePending` 时返回明确业务结果，而不是抛未知异常。

### 8.4 DeviceRegisterClient / DeviceHeartbeatClient / DeviceReportClient

职责：

1. 负责设备注册、令牌刷新、心跳与结构化上报；
2. 固定使用 `X-Device-Token` 请求头；
3. 对 `DEVICE_TOKEN_MISSING`、`DEVICE_TOKEN_INVALID`、`DEVICE_TOKEN_EXPIRED`、`DEVICE_ID_MISMATCH`、`DEVICE_REPORT_EVENT_ID_CONFLICT` 等错误码做明确映射。

---

## 9. 按项目分层的实现清单

建议实现前先固定以下项目分层：

1. `AM.Model`：`LicenseApplyRequest`、`LicenseApplyResponse`、`DeviceLicense`、`DeviceLicenseSoftware`、`DeviceLicenseBinding`、`DeviceLicenseValidity`、`DeviceLicenseAuthorization`、`DeviceLicenseSignature`、`DeviceHardwareInfo`、`LicenseValidationResult`；
2. `AM.Core.Context`：`LicenseRuntimeContext`；
3. `AM.DBService.Services.System`：`LicenseFileService`、`LicenseCryptoService`、`LicenseValidator`、`LicenseRuntimeLoader`、`HardwareInfoCollector`、`DeviceRegisterClient`、`DeviceHeartbeatClient`、`DeviceReportClient`、`DeviceLicenseApplyClient`、`LicensePagePermissionHelper`；
4. `AMControlWinF`：只负责授权状态提示和导航刷新后的展示。

---

## 10. 对现有代码的影响原则

本授权接入方案应遵守以下原则：

1. 不修改 `MainWindowModel.CanAccessPage(...)` 的核心过滤逻辑；
2. 不替换现有 `UserContext` 页面权限体系；
3. 不把授权逻辑侵入 PLC、Motion 等稳定后台任务；
4. 只在“登录后页面权限收口”这一点接入；
5. 运行时上下文独立保存授权状态，避免和用户上下文混淆。

---

## 11. 后续开发建议顺序

建议后续按以下顺序实现：

1. Phase A：`LicenseApplyRequest/Response`、`DeviceLicense*` 模型、`DeviceHardwareInfo`、`LicenseValidationResult`、`LicenseRuntimeContext`、授权常量与配置项；
2. Phase B：`LicenseFileService`、`LicenseCryptoService`、`LicenseValidator`、`HardwareInfoCollector`、`LicenseRuntimeLoader`；
3. Phase C：`DeviceRegisterClient`、`DeviceHeartbeatClient`、`DeviceReportClient`、`DeviceLicenseApplyClient`；
4. Phase D：启动期接入 `AppBootstrap.cs`，登录期接入 `AuthService.cs`；
5. Phase E：主界面授权状态提示与到期提示，`MainWindow.cs` 只做展示。

---

## 12. 结论

设备侧授权运行时接入的核心原则是：

1. 启动时读取 `license.lic` 并完成校验；
2. 校验成功后将授权状态载入运行时上下文；
3. 登录后将授权页面与用户页面权限做交集；
4. 继续复用现有 `UserContext` 与 `MainWindowModel` 导航过滤链路；
5. 在授权无效时退回默认最小功能模式，而不是破坏主程序整体启动与运行稳定性。
