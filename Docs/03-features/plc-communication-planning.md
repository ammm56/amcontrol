# PLC 通信模块规划归档说明

**文档编号**：FEAT-PLC-001  
**版本**：3.0.0  
**状态**：归档  
**最后更新**：2026-04-14  
**维护人**：Am

---

## 1. 文档说明

本文档原为 PLC 通信模块的规划文档，曾用于描述早期设想的模型、服务职责与页面拆分方案。

随着当前 WinForms 分支和 PLC 后端实现已经落地，原规划中的以下内容已经失效或被替代：

- `PLC.Register` / `PLC.Write` 拆页方案；
- 早期“UI 待开发”的状态判断；
- 部分更细粒度的规划型文件清单；
- 部分未采用的模型拆分与页面组织方式。

因此，本文件不再作为当前实现依据，仅保留为历史规划归档。

---

## 2. 当前实际实现以以下文档为准

### 架构与分层

- [PLC 协议库与 AM 上层分层架构](../01-architecture/plc-protocol-integration-design.md)
- [WinForms 解决方案架构](../01-architecture/winf-solution-architecture.md)

### 功能与调用示例

- [PLC 通信模块实现说明](plc-communication.md)

### WinForms 页面与开发基线

- [WinForms 项目总览](../02-development/winf-project-overview.md)
- [统一 UI 规范与开发模板](../01-architecture/winf-ui-standards.md)
- [页面开发模板与实施基线](../02-development/winf-page-development-template.md)

### 开发进展与手册

- [开发进展记录](../07-release-notes/winf-development-progress.md)
- [WinForms 页面操作手册](../06-user-manual/winf-page-operation-manual.md)

---

## 3. 规划与实现差异摘要

| 规划项 | 当前实现 |
|--------|----------|
| `PLC.Register` | 合并为 `PLC.Debug` 中的配置点位 / 直接地址读取能力 |
| `PLC.Write` | 合并为 `PLC.Debug` 中的写入能力 |
| UI 待开发 | `PLC.Status`、`PLC.Monitor`、`PLC.Debug`、`SysConfig.Plc` 已有首版实现 |
| 多份页面模型规划 | 当前以现有 `PlcStatusPageModel`、`PlcMonitorPageModel`、`PlcDebugPageModel`、`PlcConfigManagementPageModel` 为准 |
| 早期拆页规划 | 当前采用“运行页在 `PLC`，配置页在 `SysConfig.Plc`”的实际导航结构 |

---

## 4. 当前推荐阅读顺序

1. [PLC 协议库与 AM 上层分层架构](../01-architecture/plc-protocol-integration-design.md)
2. [PLC 通信模块实现说明](plc-communication.md)
3. [WinForms 项目总览](../02-development/winf-project-overview.md)
4. [开发进展记录](../07-release-notes/winf-development-progress.md)
5. [WinForms 页面操作手册](../06-user-manual/winf-page-operation-manual.md)

---

## 5. 归档结论

后续开发、联调、补文档时，不再参考本文件中的旧规划细节；统一以当前“已实现”文档体系为准。

