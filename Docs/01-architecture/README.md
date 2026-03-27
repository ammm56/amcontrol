# 01-architecture · 架构设计

本目录存放 `ammm56/amcontrol` 项目的**架构设计文档**，包括系统整体分层架构、核心模块说明、接口契约定义和关键技术决策记录（ADR）。

---

## 内容范围

- **系统架构图**：WPF 主控程序与各子系统的整体分层关系图。
- **分层设计说明**：`AM.Core`、`AM.Model`、`AM.MotionService`、`AM.DBService`、`AM.ViewModel`、`AMControlWPF` 各层的职责边界。
- **核心约束说明**：
  - `MachineContext` 作为运动控制全局唯一入口；
  - `ConfigContext` 作为全局配置唯一来源；
  - 统一 `Result/Result<T>` 返回模型；
  - 页面级权限从数据库读取。
- **接口契约**：关键接口的设计规范与稳定性承诺。
- **架构决策记录（ADR）**：记录重要技术选型和架构决策的背景、决策内容与影响。

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
