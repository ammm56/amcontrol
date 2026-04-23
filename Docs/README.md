# AMControl 项目文档中心

本目录是 `ammm56/amcontrol` 仓库的**仓库级文档中心**，集中管理所有项目相关文档。文档中心作为仓库级资产，独立于具体的 .NET 解决方案和项目，便于长期维护与 Copilot/本地开发使用。

---

## 目录索引

| 目录 | 说明 |
|------|------|
| [00-governance](00-governance/README.md) | 文档治理目录，定义命名规范、长期维护规则、事实来源优先级和索引使用方式 |
| [01-architecture](01-architecture/README.md) | 架构设计文档，偏长期稳定的分层边界、核心约束、接口契约和 ADR |
| [02-development](02-development/README.md) | 开发指南，偏当前活跃分支的开发基线、工程环境与协作方式 |
| [03-features](03-features/README.md) | 功能模块文档，按业务功能组织，区分当前实现说明与历史规划/接口说明 |
| [04-testing](04-testing/README.md) | 测试文档，包含测试策略、测试用例模板、虚拟卡调试指引、集成测试说明 |
| [05-operations](05-operations/README.md) | 运维与部署文档，面向安装、升级、排障与现场维护操作，不作为当前实现事实总览 |
| [06-user-manual](06-user-manual/README.md) | 用户手册，面向操作使用与培训，不作为当前实现事实或开发状态的优先来源 |
| [07-release-notes](07-release-notes/README.md) | 版本发布与开发进展目录，区分正式发布记录与当前活跃分支进展 |
| [08-third-party](08-third-party/README.md) | 第三方依赖说明，记录引入的第三方库、运动控制卡 SDK、协议和许可证信息 |
| [09-database-config](09-database-config/README.md) | 数据库与配置文档，优先记录当前表结构事实、表名映射和配置来源，历史设计说明仅作对照 |
| [10-simulation](10-simulation/README.md) | 3D 仿真集成文档，当前以历史规划和长期设计资料为主 |
| [assets](assets/README.md) | 共享资产目录，存放文档中引用的图片、截图、附件和导出文件 |

---

## 快速导航

- **新成员入门** → [02-development](02-development/README.md)
- **架构理解** → [01-architecture](01-architecture/README.md)
- **多前端/跨平台演进说明** → [01-architecture/multi-frontend-cross-platform-evolution.md](01-architecture/multi-frontend-cross-platform-evolution.md)
- **功能模块说明** → [03-features](03-features/README.md)
- **VS2019 兼容说明** → [02-development/winf-vs2019-compatibility.md](02-development/winf-vs2019-compatibility.md)
- **3D 仿真集成概览** → [10-simulation](10-simulation/README.md)
- **3D 仿真接入历史规划** → [net-integration-plan.md](10-simulation/backend/net-integration-plan.md)
- **数据库表结构** → [09-database-config](09-database-config/README.md)
- **文档命名规范** → [00-governance/document-naming-conventions.md](00-governance/document-naming-conventions.md)
- **文档维护规则** → [00-governance/document-maintenance-rules.md](00-governance/document-maintenance-rules.md)
- **发布与进展** → [07-release-notes](07-release-notes/README.md)

### AMControlWinF 分支

- **WinForms 项目总览** → [02-development/winf-project-overview.md](02-development/winf-project-overview.md)
- **WinForms VS2019 兼容说明** → [02-development/winf-vs2019-compatibility.md](02-development/winf-vs2019-compatibility.md)
- **WinForms 解决方案架构** → [01-architecture/winf-solution-architecture.md](01-architecture/winf-solution-architecture.md)
- **多前端与跨平台演进说明** → [01-architecture/multi-frontend-cross-platform-evolution.md](01-architecture/multi-frontend-cross-platform-evolution.md)
- **应用生命周期与登录流程** → [01-architecture/winf-application-lifecycle.md](01-architecture/winf-application-lifecycle.md)
- **主题系统设计** → [01-architecture/winf-theme-system.md](01-architecture/winf-theme-system.md)
- **导航系统与页面缓存** → [01-architecture/winf-navigation-system.md](01-architecture/winf-navigation-system.md)
- **统一 UI 规范与开发模板** → [01-architecture/winf-ui-standards.md](01-architecture/winf-ui-standards.md)
- **页面开发模板与实施基线** → [02-development/winf-page-development-template.md](02-development/winf-page-development-template.md)
- **用户认证与登录** → [03-features/winf-auth-login.md](03-features/winf-auth-login.md)
- **主窗体壳层** → [03-features/winf-mainwindow-shell.md](03-features/winf-mainwindow-shell.md)
- **设备软件授权设计** → [03-features/software-license-design.md](03-features/software-license-design.md)
- **license.lic 读取与校验流程** → [03-features/license-file-validation-flow.md](03-features/license-file-validation-flow.md)
- **运行时上下文与页面过滤接入** → [03-features/license-runtime-integration.md](03-features/license-runtime-integration.md)
- **设备软件授权申请与设备上报** → [03-features/device-license-and-reporting.md](03-features/device-license-and-reporting.md)
- **设备后台链路实现** → [03-features/device-management-runtime-flow.md](03-features/device-management-runtime-flow.md)
- **本地 Native 安全模块历史规划** → [03-features/native-secure-runtime-design.md](03-features/native-secure-runtime-design.md)
- **开发进展记录** → [07-release-notes/winf-development-progress.md](07-release-notes/winf-development-progress.md)
- **WinForms 页面操作手册** → [06-user-manual/winf-page-operation-manual.md](06-user-manual/winf-page-operation-manual.md)

### PLC 通信模块

- **PLC 协议库架构设计** → [01-architecture/plc-protocol-integration-design.md](01-architecture/plc-protocol-integration-design.md)
- **PLC 功能文档（实现）** → [03-features/plc-communication.md](03-features/plc-communication.md)
- **PLC 功能文档（历史规划归档）** → [03-features/plc-communication-planning.md](03-features/plc-communication-planning.md)

---

## 文档维护说明

- 所有文档均使用 **中文** 编写，技术术语保留英文原名。
- 文档命名与目录规范请遵循 [00-governance/document-naming-conventions.md](00-governance/document-naming-conventions.md)。
- 文档的长期维护规则、事实来源优先级和实现/规划文档的并存方式，请遵循 [00-governance/document-maintenance-rules.md](00-governance/document-maintenance-rules.md)。
- 当实现文档与规划文档并存时，默认优先读取“当前实现说明”类文档；规划文档仅用于背景和历史对照。
- 当前事实优先参考代码、[02-development](02-development/README.md)、[07-release-notes](07-release-notes/README.md) 和对应专题的“实现说明”文档。
- 长期设计、架构意图和历史规划优先参考 [01-architecture](01-architecture/README.md) 及已明确标记为“历史规划/归档”的文档。
- 若未来新增 UI 主线、后端形态、宿主方案或整体迁移到 `.NET 10`，请优先参考 [01-architecture/multi-frontend-cross-platform-evolution.md](01-architecture/multi-frontend-cross-platform-evolution.md) 判断文档边界与新增文档方式。
- 图片和附件统一放置在 [assets/](assets/) 目录下对应子目录中。
- 本文档中心为**仓库级资产**，不隶属于任何单一 .NET 解决方案或项目目录。