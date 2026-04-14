# 01-architecture · 架构设计

本目录存放 `ammm56/amcontrol` 项目的**架构设计文档**，包括系统整体分层架构、核心模块说明、接口契约定义、运行时链路和关键技术决策记录（ADR）。

---

## 内容范围

- **系统架构图**：WinForms 主控程序与各子系统的整体分层关系图。
- **分层设计说明**：`AM.Core`、`AM.Model`、`AM.MotionService`、`AM.DBService`、`AM.PageModel`、`AM.App`、`AMControlWinF` 各层的职责边界。
- **核心约束说明**：
  - `MachineContext` 作为运动控制全局唯一入口；
  - `ConfigContext` 作为全局配置唯一来源；
  - 统一 `Result/Result<T>` 返回模型；
  - 页面级权限从数据库读取。
- **接口契约**：关键接口的设计规范与稳定性承诺。
- **架构决策记录（ADR）**：记录重要技术选型和架构决策的背景、决策内容与影响。
- **AMControlWinF 架构文档**：WinForms 分支的分层架构、应用生命周期、主题系统、导航系统和统一 UI 规范。

---

## AMControlWinF 架构文档

| 文件 | 说明 |
|------|------|
| [winf-solution-architecture.md](winf-solution-architecture.md) | WinForms 解决方案总架构（分层、上下文、模块、服务、模型、接口、后台任务、页面落地情况） |
| [winf-application-lifecycle.md](winf-application-lifecycle.md) | WinForms 应用生命周期与登录流程（Program.cs while 主循环、ExitReason、三条退出路径） |
| [winf-theme-system.md](winf-theme-system.md) | WinForms 主题系统（AppThemeHelper、AntdUI 原生渲染、卡片面板规范） |
| [winf-navigation-system.md](winf-navigation-system.md) | WinForms 导航系统与页面缓存（NavigationCatalog、两级导航、页面工厂与缓存策略） |
| [winf-ui-standards.md](winf-ui-standards.md) | WinForms 统一 UI 规范与页面开发模板（布局模式、刷新规则、权限约束、DoD 基线） |
| [plc-protocol-integration-design.md](plc-protocol-integration-design.md) | PLC 协议库与 AM 上层分层架构（分层边界、接口定义、数据流、协议插件机制） |

---

## 子目录建议

```
01-architecture/
  adr/          # 架构决策记录（ADR）
  diagrams/     # 架构图（引用 assets/architecture/ 中的图片）
```

---

## 相关文档

- [文档命名规范（含 ADR 命名）](../00-governance/document-naming-conventions.md)
- [数据库与配置说明](../09-database-config/README.md)
