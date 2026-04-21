# 设备侧 license.lic 读取与校验流程

**文档编号**：FEAT-LICENSE-002  
**版本**：1.0.0  
**状态**：实现前最终版  
**最后更新**：2026-04-17  
**维护人**：Am

---

## 1. 文档目标

本文档用于说明设备侧如何读取、解密、验签并校验根目录 `license.lic` 文件，为接下来的设备侧 License 实现提供流程基线。

---

## 2. 文件位置与约定

### 2.1 文件位置

- 文件名：`license.lic`
- 存放位置：应用程序根目录

### 2.2 文件内容约定

当前阶段：

1. 后端授权成功后返回统一 API 包装；
2. 设备侧只将 `response.data.licenseText` 保存到 `license.lic`；
3. `license.lic` 不保存顶层 `success/message/errorCode/traceId` 包装；
4. `license.lic` 的内容可以是密文，也可以是当前联调阶段的明文 JSON 字符串；
5. 设备侧读取后先判断是否需要解密，再得到 JSON 明文。

当前真实成功响应包装示例：

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

设备侧固定写文件规则：

```text
license.lic = response.data.licenseText
```

---

## 3. 启动读取流程

建议在程序启动早期完成授权校验，但不阻断主程序基础启动流程。

推荐步骤：

1. 检查根目录是否存在 `license.lic`；
2. 若不存在，则进入最小功能模式；
3. 若存在，则读取文件内容；
4. 调用解密服务，获得 JSON 明文；
5. 解析 JSON 明文为设备侧 License 模型；
6. 重新采集当前设备硬件信息；
7. 验证 RSA 签名；
8. 校验硬件绑定信息；
9. 校验许可有效期；
10. 若全部通过，则载入运行时授权上下文；
11. 若任一失败，则退回默认最小功能模式。

---

## 4. 建议的服务拆分

### 4.1 LicenseFileService

职责：

1. 检查 `license.lic` 是否存在；
2. 读取 `license.lic` 内容；
3. 写入新授权数据到 `license.lic`；
4. 提供统一的文件路径与异常处理；
5. 必要时保留旧文件备份或临时写入文件，避免直接覆盖导致文件损坏。

### 4.2 LicenseCryptoService

职责：

1. 对 `license.lic` 内容解密；
2. 对解密后的 JSON 明文执行 RSA 验签；
3. 返回验签结果与错误信息。

### 4.3 LicenseValidator

职责：

1. 校验 JSON 是否完整；
2. 校验硬件绑定信息是否与当前设备一致；
3. 校验当前时间是否在有效期内；
4. 判断是否进入宽限期；
5. 输出最终许可状态。

### 4.4 HardwareInfoCollector

职责：

1. 统一采集 `clientId`、`machineCode`、`machineName`、`cpuId`、`biosSerialNumber`、`mainboardSerialNumber`、`diskSerialNumber`、`macAddress`；
2. 对大小写、空格、分隔符做统一归一化；
3. 为 `LicenseValidator` 提供标准化的当前设备硬件快照；
4. 与 `ClientIdentityService` 分工明确，不把授权校验逻辑写进身份服务。

---

## 5. RSA 验签规则

设备侧不应仅依赖文件存在与否判断授权有效性。

必须至少校验：

1. 授权 JSON 的签名是否由合法授权服务器生成；
2. 签名原文是否未被篡改；
3. 验签公钥是否为设备软件内置可信公钥。

### 建议规则

- 验签成功：继续校验硬件信息与有效期；
- 验签失败：直接视为无效授权，退回最小功能模式。

首版定稿规则如下：

1. 首版即启用正式 `RSA-SHA256` 验签，不采用“仅明文解析不验签”的过渡方案；
2. 验签公钥固定为 `AM.Model/Common/Config.Secrets.cs` 中 `Setting` 的代码内置 PEM 文本，由 `LicenseCryptoService` 读取使用；
3. 首版不从 `config.json`、数据库或外部 Key 文件加载公钥，避免现场配置漂移导致授权失效；
4. 首版不实现多公钥轮换路由；
5. 若 `licenseText.signature.keyId` 存在，则其值必须等于固定内置 KeyId；不一致直接判定验签失败；
6. 固定内置 KeyId 建议使用 `AMCONTROL_RSA_V1`，后续如需轮换，再以新版本扩展多 KeyId 支持。

---

## 6. 硬件绑定校验规则

设备侧启动时应重新采集当前设备硬件信息，并与 License 中记录的绑定信息比较。

首版统一采集字段：

1. `ClientId`
2. `MachineCode`
3. `CpuId`
4. `BiosSerialNumber`
5. `MainboardSerialNumber`
6. `DiskSerialNumber`
7. `MacAddress`

### 建议处理原则

- 关键硬件标识不一致：授权无效；
- 非关键字段为空：允许忽略；
- 字段比较建议统一去空格、大小写不敏感处理。

建议首版强校验字段固定为：

1. `ClientId`
2. `MachineCode`
3. `CpuId`

首版按以下规则执行：

1. `ClientId`、`MachineCode`、`CpuId` 属于拦截性强校验字段，任一缺失、采集失败或比较不一致，均判定授权无效；
2. `MachineName` 不纳入强校验，避免现场改机名导致授权误失效；
3. `BiosSerialNumber`、`MainboardSerialNumber`、`DiskSerialNumber`、`MacAddress` 首版只采集并写入诊断日志，不作为拦截条件；
4. 所有比较统一执行 `Trim()`、去分隔符、转大写后再比较；
5. 后端签发时也应保证首版授权一定写入 `ClientId`、`MachineCode`、`CpuId` 三个绑定字段。

---

## 7. 有效期校验规则

建议校验以下字段：

1. `NotBefore`
2. `ExpiresAt`
3. `GraceDays`

### 判定逻辑

- 当前时间 < `NotBefore`：未生效，视为无效；
- 当前时间 <= `ExpiresAt`：有效；
- 当前时间 > `ExpiresAt` 且在宽限期内：宽限有效；
- 超过宽限期：无效。

---

## 8. 校验失败时的处理策略

以下任一情况成立时，设备侧进入最小功能模式：

1. `license.lic` 不存在；
2. 文件读取失败；
3. 解密失败；
4. JSON 解析失败；
5. RSA 验签失败；
6. 硬件绑定信息不匹配；
7. 已过期且不在宽限期内。

### 处理原则

1. 不中断软件基础启动；
2. 记录日志；
3. 授权状态标记为无效；
4. 使用默认开放页面集合作为最终允许页面范围。

---

## 9. 最小功能模式建议

建议在最小功能模式下保留基础监视与审计页面：

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

---

## 10. 推荐接入位置

建议设备侧在以下阶段调用授权文件处理流程：

1. 程序启动时完成 `license.lic` 读取与校验；
2. 登录成功后将授权页面与用户权限页面做交集；
3. MainWindow 构建导航时直接使用已经收口后的最终页面权限。

---

## 11. 后续代码实现建议

建议后续实现以下类：

- `LicenseFileService`
- `LicenseCryptoService`
- `LicenseValidator`
- `HardwareInfoCollector`
- `LicenseRuntimeLoader`
- `LicensePagePermissionHelper`
- `DeviceLicenseState`
- `LicenseRuntimeContext`

这些类建议保持职责单一，避免把文件读取、解密、校验、导航过滤混在一个类中。
