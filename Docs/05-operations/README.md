# 05-operations · 运维与部署

本目录存放 `ammm56/amcontrol` 项目的**运维与部署文档**，面向负责现场安装、升级和维护的工程师与运维人员。

使用边界说明：

1. 本目录面向部署、升级、排障和现场维护操作，不承担“当前实现事实总览”的角色；
2. 若涉及当前页面状态、当前服务职责或当前运行链路，应优先参考代码、[02-development/README.md](../02-development/README.md)、[03-features/README.md](../03-features/README.md) 和 [07-release-notes/README.md](../07-release-notes/README.md)；
3. 本目录中的操作步骤和部署约束可直接用于现场执行，但不应用来判断某项功能是否已经在当前分支落地。
4. 本文档中关于 `.NET Framework` 依赖项处理、目标机器环境要求和当前部署链路的描述，默认仅代表当前部署事实，不构成未来整体迁移到 `.NET 10` 时的部署模型限制。

---

## 内容范围

- **安装包制作**：如何基于当前解决方案生成可部署安装包，包含 .NET Framework 依赖项处理。
- **部署说明**：目标机器的环境要求（操作系统、.NET Framework 版本、运动控制卡驱动安装步骤）。
- **数据库初始化与迁移**：
  - SQLite / Access / MySQL 三种数据库的初始化方式；
  - 版本升级时的表结构迁移说明（SqlSugar CodeFirst 流程）。
- **配置管理**：
  - `config.json` 各配置项说明；
  - 运动控制卡配置（数据库表 `motion_card`、`motion_axis`、`motion_io_map`）的初始导入方式。
- **日志排查指引**：NLog 日志位置、日志级别说明、常见错误日志分析。
- **备份与恢复**：数据库备份策略、配置文件备份方式。

---

## 相关文档

- [文档维护规则](../00-governance/document-maintenance-rules.md)
- [数据库与配置](../09-database-config/README.md)
- [版本发布记录](../07-release-notes/README.md)
