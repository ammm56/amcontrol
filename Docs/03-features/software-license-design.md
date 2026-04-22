# 设备软件授权设计说明

**文档编号**：FEAT-LICENSE-001  
**版本**：1.0.0  
**状态**：实现前最终版  
**最后更新**：2026-04-17  
**维护人**：Am

---

## 1. 文档目标

本文档用于统一设备侧与后端授权服务侧的软件授权设计，为后续以下开发提供基线：

1. 设备软件提交授权申请；
2. 后端授权服务匹配默认授权配置并生成授权数据；
3. 设备软件将成功授权响应中的 `data.licenseText` 保存到根目录 `license.lic`；
4. 程序启动时读取 `license.lic`，解密得到 JSON 明文；
5. 程序校验 RSA 签名和硬件绑定信息；
6. 校验成功后将授权信息载入运行时上下文；
7. 将 `pageKeys` 收口到当前 WinForms 导航与页面权限体系中。

当前阶段先完成**明文授权数据交互**。后端正式返回体可以是加密文本，设备侧先解密，再得到 JSON 明文并完成后续校验。

---

## 2. 总体设计原则

### 2.1 职责边界

#### 设备侧负责

1. 收集软件信息；
2. 收集硬件绑定信息；
3. 提交授权申请 JSON；
4. 接收授权数据并保存为根目录 `license.lic`；
5. 程序启动时读取 `license.lic`；
6. 解密得到 JSON 明文；
7. 验签并校验硬件绑定信息；
8. 解析有效期与授权页面；
9. 将授权信息载入运行时上下文；
10. 将许可页面与当前用户页面权限做交集；
11. 驱动导航显示与功能可用性。

#### 后端授权侧负责

1. 预配置程序信息；
2. 预配置程序对应的全部页面目录；
3. 预配置默认授权模板；
4. 处理授权申请；
5. 按程序分类 / 程序编码 / 程序名称 / 当前运行版本是否命中模板 `minAppVersion` / `maxAppVersion` 范围匹配授权模板；
6. 生成授权数据；
7. 返回授权数据给设备侧。

### 2.2 设计约束

1. 设备侧提交的授权申请 **不上传页面列表**；
2. 页面目录统一由后端按程序信息从数据库读取；
3. 授权信息当前阶段**不使用数据库缓存**；
4. 授权成功返回的数据统一保存到应用根目录 `license.lic`；
5. 设备侧最终权限 = 当前用户页面权限 ∩ License 授权页面；
6. 基础状态与审计页面建议在授权失效时保留只读访问；
7. 授权逻辑不得破坏当前稳定的用户权限体系与导航体系。

---

## 3. 设备侧授权申请 JSON 设计

### 3.1 请求示例

```json
{
  "requestId": "0b91d4d7d5a34d3c9e65d7b2417e6c11",
  "requestTime": "2026-04-17T14:30:25+08:00",
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
    "clientId": "A1B2C3D4E5F60718293A4B5C",
    "machineCode": "MC-001",
    "machineName": "ASSEMBLY-LINE-01",
    "machineModel": "AM-STD-01",
    "osVersion": "Microsoft Windows 10 Enterprise 22H2",
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

### 3.2 字段说明

#### 顶层字段

| 字段 | 必填 | 说明 |
|------|------|------|
| `requestId` | 是 | 本次授权请求唯一标识 |
| `requestTime` | 是 | 请求时间 |
| `licenseProtocolVersion` | 是 | 授权协议版本 |

#### software

| 字段 | 必填 | 说明 |
|------|------|------|
| `appCategory` | 是 | 程序分类，如 `MotionControl` |
| `appCode` | 是 | 程序编码，如 `AMControlWinF` |
| `appName` | 是 | 程序名称 |
| `appEdition` | 否 | 版型，如 `Standard` / `Professional` / `Enterprise` |
| `appVersion` | 是 | 当前设备运行版本，供后端判断是否命中模板版本范围 |
| `targetFramework` | 否 | 目标框架 |
| `uiPlatform` | 否 | UI 类型，如 `WinForms` |
| `vendor` | 否 | 软件厂商 |

#### device

| 字段 | 必填 | 说明 |
|------|------|------|
| `clientId` | 是 | 软件实例唯一标识 |
| `machineCode` | 否 | 设备编码 |
| `machineName` | 否 | 设备名称 |
| `machineModel` | 否 | 设备型号 |
| `osVersion` | 否 | 操作系统版本 |
| `cpuId` | 否 | CPU 标识 |
| `biosSerialNumber` | 否 | BIOS 序列号 |
| `mainboardSerialNumber` | 否 | 主板序列号 |
| `diskSerialNumber` | 否 | 磁盘序列号 |
| `macAddress` | 否 | 网卡 MAC |

#### environment

| 字段 | 必填 | 说明 |
|------|------|------|
| `siteCode` | 否 | 工厂或站点编码 |
| `customerCode` | 否 | 客户编码 |
| `networkMode` | 否 | `Online` / `Offline` |

#### signature

| 字段 | 必填 | 说明 |
|------|------|------|
| `algorithm` | 否 | 摘要算法 |
| `contentSha256` | 否 | 请求内容摘要 |

---

## 4. 后端返回授权数据设计

### 4.1 解密后的许可明文 JSON 示例

```json
{
  "licenseId": "LIC-20260417-000001",
  "licenseProtocolVersion": "1.0",
  "issueTime": "2026-04-17T14:35:02+08:00",
  "issuer": "AM License Server",
  "software": {
    "appCategory": "MotionControl",
    "appCode": "AMControlWinF",
    "appName": "AM Motion Control",
    "appEdition": "Professional",
    "minAppVersion": "1.2.0",
    "maxAppVersion": "1.2.9"
  },
  "deviceBinding": {
    "clientId": "A1B2C3D4E5F60718293A4B5C",
    "machineCode": "MC-001",
    "machineName": "ASSEMBLY-LINE-01",
    "cpuId": "BFEBFBFF000906EA",
    "biosSerialNumber": "ABC123456789",
    "mainboardSerialNumber": "MB-987654321",
    "diskSerialNumber": "SSD-1234567890",
    "macAddress": "00-1A-2B-3C-4D-5E"
  },
  "validity": {
    "licenseType": "TimeLimited",
    "notBefore": "2026-04-17T00:00:00+08:00",
    "expiresAt": "2027-04-16T23:59:59+08:00",
    "graceDays": 7
  },
  "authorization": {
    "moduleKeys": [
      "Home",
      "Motion",
      "PLC",
      "AlarmLog",
      "MotionConfig",
      "SysConfig"
    ],
    "pageKeys": [
      "Home.Overview",
      "Home.SysStatus",
      "Motion.DI",
      "Motion.DO",
      "Motion.Monitor",
      "Motion.Axis",
      "Motion.Actuator",
      "PLC.Status",
      "PLC.Monitor",
      "PLC.Debug",
      "AlarmLog.Current",
      "AlarmLog.History",
      "AlarmLog.RunLog",
      "MotionConfig.Card",
      "MotionConfig.Axis",
      "MotionConfig.IoMap",
      "MotionConfig.AxisParam",
      "MotionConfig.Actuator",
      "SysConfig.Plc",
      "SysConfig.Runtime"
    ]
  },
  "signature": {
    "algorithm": "RSA-SHA256",
    "contentSha256": "d41d8cd98f00b204e9800998ecf8427e",
    "signText": "BASE64_SIGNATURE_TEXT"
  }
}
```

### 4.2 设备侧真正使用的核心字段

设备侧许可解析后重点使用：

1. `validity.notBefore`
2. `validity.expiresAt`
3. `validity.graceDays`
4. `authorization.moduleKeys`
5. `authorization.pageKeys`
6. `deviceBinding.*`

授权版本补充规则：

1. 所有授权版型统一要求 license 下发 `software.minAppVersion` / `software.maxAppVersion`，设备侧按当前程序版本是否落在该闭区间内判断授权是否有效；
2. 设备侧不再回退为 `software.appVersion` 精确匹配；任一范围字段缺失都视为授权数据不完整；
3. 当前版本比较使用标准 `Version` 语义，适配 `0.0.2` 这类 `major.minor.build` 格式。

### 4.3 默认授权模板版本范围示例

后端默认授权模板数据应直接保存版本范围，不再保存用于授权判定的单值 `appVersion`。建议模板至少表达如下口径：

```json
{
  "appCode": "AMControlWinF",
  "appEdition": "Professional",
  "customerCode": "CUS-10001",
  "siteCode": "SZ01",
  "machineModel": "AM-STD-01",
  "minAppVersion": "1.2.0",
  "maxAppVersion": "1.2.9",
  "licenseType": "TimeLimited",
  "graceDays": 7,
  "moduleKeys": ["Home", "Motion", "PLC"],
  "pageKeys": ["Home.Overview", "Motion.DI", "Motion.Monitor", "PLC.Monitor"]
}
```

模板版本匹配规则：

1. 后端使用设备申请中的 `software.appVersion` 判断当前请求是否落在模板 `[minAppVersion, maxAppVersion]` 闭区间内；
2. 模板命中后，签发到 `licenseText.software` 的版本字段也应为 `minAppVersion` / `maxAppVersion`；
3. 模板与签发结果都不再使用单值 `appVersion` 作为授权有效性的判断依据。

---

## 5. 后端授权服务处理流程

### 5.1 接口建议

- `POST /api/license/apply`

### 5.2 处理步骤

1. 解析请求 JSON；
2. 提取软件信息：`appCategory`、`appCode`、`appName`、`appVersion`；
3. 根据 `appCode` 查询程序主数据；
4. 根据程序主数据查询该程序全部页面目录；
5. 根据程序分类、程序名称、客户、站点等信息筛选模板，再用当前 `appVersion` 判断是否命中模板 `minAppVersion` / `maxAppVersion` 范围；
6. 若查不到默认授权模板，则返回“待授权”；
7. 若查到默认授权模板，则生成许可明文 JSON；
8. 将模板中的 `minAppVersion` / `maxAppVersion`、有效期、设备绑定信息、授权模块、授权页面写入许可明文 JSON；
9. 对许可明文 JSON 执行 RSA 签名；
10. 对签名后的授权数据执行加密；
11. 返回加密授权数据给设备侧。

### 5.3 无默认授权配置时的返回建议

```json
{
  "success": false,
  "errorCode": "LicensePending",
  "message": "未找到默认授权配置，请联系管理员添加授权模板。"
}
```

### 5.4 当前真实成功响应包装

当前设备侧实现应以真实后端成功响应为准，而不是自行推断返回结构。

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

1. 顶层 `success`、`message`、`errorCode`、`traceId`、`errors` 是统一 API 包装；
2. 真正的授权业务载荷位于 `data`；
3. `data.licenseText` 是一个 JSON 字符串，不是嵌套对象；
4. 设备侧落地到 `license.lic` 的内容固定为 `data.licenseText`，而不是整个响应包装；
5. `data.issuedAt`、`data.expiresAt` 仅作为当前申请结果展示或日志辅助字段，不替代 `licenseText.validity` 中的正式校验字段。

### 5.5 requestId 固定规则

设备侧首版必须把 `requestId` 视为幂等键，而不是普通流水号。

1. 同一次授权申请重试时，`requestId` 必须保持不变；
2. 新的一次授权申请才生成新的 `requestId`；
3. 推荐格式：`apply-{yyyyMMddHHmmss}-{随机短码}`；
4. 设备侧不得把 `requestId` 与 `traceId` 混用；
5. 当后端返回 `LicensePending` 时，设备侧应保留本次 `requestId` 以便后续排查。

---

## 6. 设备侧本地 License 数据模型草案

### 6.1 本地许可明文模型

建议设备侧定义以下模型：

- `DeviceLicense`
- `DeviceLicenseSoftware`
- `DeviceLicenseBinding`
- `DeviceLicenseValidity`
- `DeviceLicenseAuthorization`
- `DeviceLicenseSignature`

设备侧本地模型应覆盖：

1. 许可 ID；
2. 软件标识；
3. 设备绑定信息；
4. 生效时间 / 到期时间 / 宽限天数；
5. 模块键集合；
6. 页面键集合；
7. 签名信息。

### 6.2 本地缓存方式建议

当前阶段不使用数据库缓存授权信息。

建议使用：

- 根目录授权文件：`license.lic`

说明：

1. 授权成功后，将 `data.licenseText` 原文保存到应用根目录；
2. 文件名固定为 `license.lic`；
3. 启动时优先读取 `license.lic`；
4. 若 `licenseText` 为密文，则设备侧先解密，再得到 JSON 明文；若当前联调阶段返回的已是明文 JSON 字符串，则直接解析；
5. 不单独落本地授权数据库表。

### 6.3 设备侧首版模型清单

为避免实现时临时拼装匿名对象，建议设备侧首版固定以下模型：

- `LicenseApplyRequest`
- `LicenseApplyResponse`
- `DeviceLicense`
- `DeviceLicenseSoftware`
- `DeviceLicenseBinding`
- `DeviceLicenseValidity`
- `DeviceLicenseAuthorization`
- `DeviceLicenseSignature`
- `DeviceHardwareInfo`
- `LicenseValidationResult`

### 6.4 运行时状态模型建议

建议增加运行时状态模型：

- `DeviceLicenseState`

建议包含：

1. `HasLicense`
2. `IsValid`
3. `IsExpired`
4. `IsInGracePeriod`
5. `ExpiresAt`
6. `ModuleKeys`
7. `PageKeys`
8. `Message`

---

## 7. 设备侧 license.lic 启动处理规则

### 7.1 启动时处理流程

程序启动时建议按以下顺序处理：

1. 检查应用根目录是否存在 `license.lic`；
2. 若不存在，则进入最小功能模式；
3. 若存在，则读取文件内容；
4. 解密得到 JSON 明文；
5. 解析出 License 数据模型；
6. 重新读取当前设备硬件信息；
7. 校验 License 中的 RSA 签名；
8. 校验 License 中的硬件绑定信息是否与当前设备一致；
9. 校验有效期；
10. 若全部通过，则将授权信息载入运行时上下文；
11. 若任一校验失败，则退回默认最小功能模式。

### 7.2 校验失败时的处理原则

以下任一情况成立时，均视为授权无效：

1. `license.lic` 不存在；
2. 文件读取失败；
3. 解密失败；
4. JSON 解析失败；
5. RSA 签名校验失败；
6. 硬件信息不匹配；
7. 已过期且不在宽限期内。

处理策略：

- 不报致命异常；
- 不阻断软件启动；
- 自动退回默认最小功能模式。

### 7.3 首版验签与硬件绑定定稿

首版授权校验固定采用以下口径：

1. 启动时正式执行 `RSA-SHA256` 验签；
2. 验签公钥固定为随程序编译发布的内置 PEM 文本资源，不从配置文件、数据库或外部 Key 文件读取；
3. 首版固定单一 KeyId，建议值为 `AMCONTROL_RSA_V1`；若授权中携带 `signature.keyId`，则必须与该值一致；
4. 硬件强校验字段固定为 `ClientId`、`MachineCode`、`CpuId`；
5. `MachineName`、`BiosSerialNumber`、`MainboardSerialNumber`、`DiskSerialNumber`、`MacAddress` 只做诊断采集，不作为首版拦截条件；
6. 强校验字段任一缺失、采集失败或比较不一致，均判定授权无效并退回最小功能模式。

---

## 8. 设备侧如何将 pageKeys 应用到现有导航与页面权限体系

### 8.1 当前现有权限链路

当前 WinForms 页面导航过滤逻辑建立在以下对象之上：

1. `UserContext.Instance.CurrentPageKeys`
2. `UserContext.Instance.HasPagePermission(pageKey)`
3. `MainWindowModel.CanAccessPage(...)`
4. `NavigationCatalog`

当前 `MainWindowModel` 的页面过滤逻辑为：

- 页面可见 = `UserContext.Instance.HasPagePermission(page.PageKey)`

因此，建议 **不修改当前导航过滤逻辑**，而是在登录成功后将 License 授权页面与当前用户页面权限做交集，再写回 `UserContext.CurrentPageKeys`。

### 8.2 最终权限规则

最终页面权限建议定义为：

`最终可见页面 = 当前用户页面权限 ∩ License 授权页面`

即：

1. 用户本来没有权限的页面，即使 License 授权，也不能显示；
2. 用户本来有权限的页面，若 License 未授权，也不能显示；
3. 只有同时满足“用户权限允许”和“License 授权允许”的页面才可见。

### 8.3 推荐接入点

建议在 **登录成功后、主界面加载前** 完成授权页面收口。

推荐顺序：

1. `AuthService.Login(...)` 得到当前用户页面权限；
2. 本地读取 License；
3. 校验 License 是否有效；
4. 取出 `pageKeys`；
5. 将用户权限页与 License 页做交集；
6. 调用 `UserContext.Instance.RefreshPagePermissions(...)`；
7. `MainWindowModel.LoadNavigation()` 按最终权限自动显示导航。

### 8.4 建议的辅助规则

#### 正常授权模式

- 使用 `用户页面权限 ∩ License 页面权限`

#### 无许可 / 许可失效模式

建议保留基础监视与审计页面，避免软件完全不可用。

---

## 9. 页面授权清单建议

### 9.1 默认开放页面

这些页面建议在授权失效时仍保留：

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

### 9.2 建议纳入授权限制的页面

```json
{
  "licensedPages": [
    "Assembly.Wiring",
    "Motion.Axis",
    "Motion.Actuator",
    "Production.Recipe",
    "Production.Data",
    "Production.Report",
    "Vision.Monitor",
    "Vision.Result",
    "Vision.Calibrate",
    "PLC.Debug",
    "Peripheral.ScanTest",
    "Peripheral.SensorTrend",
    "MotionConfig.Card",
    "MotionConfig.Axis",
    "MotionConfig.IoMap",
    "MotionConfig.AxisParam",
    "MotionConfig.Actuator",
    "SysConfig.Camera",
    "SysConfig.Plc",
    "SysConfig.Sensor",
    "SysConfig.Scanner",
    "SysConfig.Mes",
    "SysConfig.Runtime",
    "System.User",
    "System.Permission"
  ]
}
```

### 9.3 当前 NavigationCatalog 对应页面目录

当前 WinForms 分支页面目录来自 `AM.PageModel.Navigation.NavigationCatalog`，后端应预先将该程序的页面目录维护到授权库中。当前页面包括：

- `Home.Overview`
- `Home.SysStatus`
- `Assembly.Wiring`
- `Motion.DI`
- `Motion.DO`
- `Motion.Monitor`
- `Motion.Axis`
- `Motion.Actuator`
- `Production.Recipe`
- `Production.Data`
- `Production.Report`
- `Vision.Monitor`
- `Vision.Result`
- `Vision.Calibrate`
- `PLC.Status`
- `PLC.Monitor`
- `PLC.Debug`
- `Peripheral.Scanner`
- `Peripheral.ScanTest`
- `Peripheral.Sensor`
- `Peripheral.SensorTrend`
- `MotionConfig.Card`
- `MotionConfig.Axis`
- `MotionConfig.IoMap`
- `MotionConfig.AxisParam`
- `MotionConfig.Actuator`
- `SysConfig.Camera`
- `SysConfig.Plc`
- `SysConfig.Sensor`
- `SysConfig.Scanner`
- `SysConfig.Mes`
- `SysConfig.Runtime`
- `AlarmLog.Current`
- `AlarmLog.History`
- `AlarmLog.RunLog`
- `System.User`
- `System.Permission`
- `System.LoginLog`

---

## 10. 后续开发建议

后续建议按以下顺序推进：

1. Phase A：新增模型、运行时上下文、常量与配置项；
2. Phase B：实现 `LicenseFileService`、`LicenseCryptoService`、`LicenseValidator`、`HardwareInfoCollector`、`LicenseRuntimeLoader`；
3. Phase C：实现 `DeviceRegisterClient`、`DeviceHeartbeatClient`、`DeviceReportClient`、`DeviceLicenseApplyClient`；
4. Phase D：在 `AppBootstrap.cs` 接入启动期授权运行时装载，并在 `AuthService.cs` 登录成功后做页面权限收口；
5. Phase E：在 `MainWindow.cs` 增加授权状态提示与到期提示，但不承载授权核心逻辑。

### 10.1 按项目分层的新增类清单

建议按当前 WinForms 解决方案分层固定新增类的放置位置：

1. `AM.Model`：`LicenseApplyRequest`、`LicenseApplyResponse`、`DeviceLicense*`、`DeviceHardwareInfo`、`LicenseValidationResult`；
2. `AM.Core.Context`：`LicenseRuntimeContext`；
3. `AM.DBService.Services.System`：`LicenseFileService`、`LicenseCryptoService`、`LicenseValidator`、`LicenseRuntimeLoader`、`HardwareInfoCollector`、`DeviceRegisterClient`、`DeviceHeartbeatClient`、`DeviceReportClient`、`DeviceLicenseApplyClient`、`LicensePagePermissionHelper`；
4. `AMControlWinF`：后续只承载授权状态展示，不承载授权核心逻辑。

---

## 11. 结论

本方案中：

1. 设备侧只提交软件与硬件信息；
2. 后端负责根据程序信息与版本查询页面目录和默认授权配置；
3. 后端返回的是加密授权数据，设备侧解密后得到 JSON 明文；
4. 授权信息当前阶段不入数据库，而是保存到根目录 `license.lic`；
5. 程序启动时读取并校验 `license.lic`，失败则退回默认最小功能；
6. 校验通过后将授权信息载入运行时上下文；
7. 设备侧再将 `pageKeys` 收口到现有 `UserContext.CurrentPageKeys`；
8. 当前导航与页面权限体系保持稳定，不做大改。
