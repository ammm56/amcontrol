# AMControl 项目文档中心

本目录是 `ammm56/amcontrol` 仓库的**仓库级文档中心**，集中管理所有项目相关文档。文档中心作为仓库级资产，独立于具体的 .NET 解决方案和项目，便于长期维护与 Copilot/本地开发使用。

---

## 目录索引

| 目录 | 说明 |
|------|------|
| [00-governance](00-governance/README.md) | 文档治理与命名规范，定义全项目文档的目录命名、文件命名、元数据规范和资产管理规则 |
| [01-architecture](01-architecture/README.md) | 架构设计文档，包含系统整体架构图、分层设计、核心模块说明、接口契约和关键技术决策记录（ADR） |
| [02-development](02-development/README.md) | 开发指南，包含环境搭建、项目结构说明、编码规范、分支策略、代码审查流程和常见开发问题 |
| [03-features](03-features/README.md) | 功能模块文档，按业务功能组织，覆盖运动控制、IO 管理、视觉、PLC 通信、权限管理等功能模块的设计与说明 |
| [04-testing](04-testing/README.md) | 测试文档，包含测试策略、测试用例模板、虚拟卡调试指引、集成测试说明 |
| [05-operations](05-operations/README.md) | 运维与部署文档，包含安装包制作、部署说明、数据库迁移、配置管理、日志排查指引 |
| [06-user-manual](06-user-manual/README.md) | 用户手册，面向操作员和工程师，包含界面操作指引、设备参数配置、报警处理流程 |
| [07-release-notes](07-release-notes/README.md) | 版本发布记录，按版本号归档变更日志、升级说明和已知问题 |
| [08-third-party](08-third-party/README.md) | 第三方依赖说明，记录引入的第三方库、运动控制卡 SDK、协议和许可证信息 |
| [09-database-config](09-database-config/README.md) | 数据库与配置文档，包含表结构说明、字段定义、ConfigContext 配置项说明、数据库切换指引 |
| [10-simulation](10-simulation/README.md) | 3D 仿真集成文档，包含虚拟设备集成方案、WebSocket 协议、前端仿真规划和网络接入设计 |
| [assets](assets/README.md) | 共享资产目录，存放文档中引用的图片、截图、附件和导出文件 |

---

## 快速导航

- **新成员入门** → [02-development](02-development/README.md)
- **架构理解** → [01-architecture](01-architecture/README.md)
- **功能模块说明** → [03-features](03-features/README.md)
- **3D 仿真集成概览** → [10-simulation](10-simulation/README.md)
- **3D 仿真接入设计文档** → [net-integration-plan.md](10-simulation/backend/net-integration-plan.md)
- **数据库表结构** → [09-database-config](09-database-config/README.md)
- **文档命名规范** → [00-governance/document-naming-conventions.md](00-governance/document-naming-conventions.md)
- **版本记录** → [07-release-notes](07-release-notes/README.md)

### AMControlWinF 分支

- **WinForms 项目总览** → [02-development/winf-project-overview.md](02-development/winf-project-overview.md)
- **WinForms 解决方案架构** → [01-architecture/winf-solution-architecture.md](01-architecture/winf-solution-architecture.md)
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
- **本地 Native 安全模块规划** → [03-features/native-secure-runtime-design.md](03-features/native-secure-runtime-design.md)
- **开发进展记录** → [07-release-notes/winf-development-progress.md](07-release-notes/winf-development-progress.md)
- **WinForms 页面操作手册** → [06-user-manual/winf-page-operation-manual.md](06-user-manual/winf-page-operation-manual.md)

### PLC 通信模块

- **PLC 协议库架构设计** → [01-architecture/plc-protocol-integration-design.md](01-architecture/plc-protocol-integration-design.md)
- **PLC 功能文档（实现）** → [03-features/plc-communication.md](03-features/plc-communication.md)
- **PLC 功能文档（规划→已落地）** → [03-features/plc-communication-planning.md](03-features/plc-communication-planning.md)

---

## 文档维护说明

- 所有文档均使用 **中文** 编写，技术术语保留英文原名。
- 文档命名与目录规范请遵循 [00-governance/document-naming-conventions.md](00-governance/document-naming-conventions.md)。
- 图片和附件统一放置在 [assets/](assets/) 目录下对应子目录中。
- 本文档中心为**仓库级资产**，不隶属于任何单一 .NET 解决方案或项目目录。