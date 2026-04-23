# 02-development · 开发指南

本目录存放 `ammm56/amcontrol` 项目的**开发指南**，帮助开发者快速理解项目结构、搭建开发环境、遵循编码规范并参与日常开发流程。

说明：本文档中涉及 `.NET Framework 4.6.1 / 4.7.2`、VS2019 兼容和旧式项目结构的内容，默认仅描述当前实现事实，不构成未来整体迁移到 `.NET 10` 的限制。

---

## 内容范围

- **环境搭建**：开发环境要求（.NET Framework 4.6.1 / 4.7.2、Visual Studio 版本、SDK 兼容边界）、首次克隆后的配置步骤。
- **项目结构说明**：解决方案各项目的职责说明与依赖关系。
- **编码规范**：
  - ViewModel 属性采用手动字段 + 属性方式，不使用 `[ObservableProperty]` 源生成器特性；
  - 统一 `Result/Result<T>` 返回模型；
  - 服务层采用构造注入依赖；
  - 可复用样式下沉到统一资源字典。
- **分支与提交规范**：分支命名、提交信息格式、PR 流程说明。
- **虚拟卡开发调试指引**：如何使用 `VirtualMotionCardService` 在无真实硬件情况下进行开发调试。
- **常见问题**：常见编译错误、配置问题与解决方案。

---

## AMControlWinF 开发文档

| 文件 | 说明 |
|------|------|
| [winf-project-overview.md](winf-project-overview.md) | AMControlWinF WinForms 分支项目总览（技术栈、解决方案结构、文件布局、核心架构约束） |
| [winf-page-development-template.md](winf-page-development-template.md) | WinForms 页面开发模板（UserControl + PageModel + Service 的标准分工、代码骨架、交付清单） |
| [winf-vs2019-compatibility.md](winf-vs2019-compatibility.md) | VS2019 兼容说明（SDK-style 项目加载、global.json、设计器限制与适用边界） |

---

## 相关文档

- [架构设计](../01-architecture/README.md)
- [文档维护规则](../00-governance/document-maintenance-rules.md)
- [第三方依赖说明](../08-third-party/README.md)
- [3D 仿真集成开发](../10-simulation/README.md)
