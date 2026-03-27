# 05-operations · 运维与部署

本目录存放 `ammm56/amcontrol` 项目的**运维与部署文档**，面向负责现场安装、升级和维护的工程师与运维人员。

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

- [数据库与配置](../09-database-config/README.md)
- [版本发布记录](../07-release-notes/README.md)
