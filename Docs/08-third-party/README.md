# 08-third-party · 第三方依赖说明

本目录记录 `ammm56/amcontrol` 项目引入的**第三方库、运动控制卡 SDK、通信协议和相关许可证信息**。

说明：本文档中出现的 `.NET Framework` 风格依赖管理、`packages.config` 和当前 SDK/运行时配套关系，默认仅描述当前实现事实，不构成未来整体迁移到 `.NET 10` 的限制。

---

## 主要依赖

### .NET / WPF 层

| 库 | 用途 | 许可证 |
|----|------|--------|
| [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) | MVVM 基础库，提供 `ObservableObject`、`RelayCommand` 等 | MIT |
| [HandyControl](https://github.com/HandyOrg/HandyControl) | WPF UI 组件库，提供主题、样式和控件 | MIT |
| [SqlSugar](https://github.com/DotNetNext/SqlSugar) | ORM 数据库操作，支持 SQLite/Access/MySQL | Apache-2.0 |
| [NLog](https://nlog-project.org/) | 日志库 | BSD-3-Clause |

### 运动控制卡 SDK

| SDK | 厂商 | 说明 |
|-----|------|------|
| 固高（GT）SDK | 固高科技 | `gts.dll`、`gt_rn.dll`，用于固高运动控制卡接入 |
| 雷赛（LA/PI）SDK | 雷赛智能 | `LAFunc.dll`、`PIFunc.dll`、`VFunc.dll`、`MachineBuilding.dll` |

---

## 使用说明

- 运动控制卡 SDK 的 DLL 文件直接放置在 `AM.MotionService/` 项目目录下，项目构建时自动复制到输出目录。
- 所有第三方 NuGet 包通过各 `.csproj` 的 `packages.config` 管理（.NET Framework 风格）。
- 如需查看具体版本号，请参阅各项目的 `packages.config` 文件。

---

## 相关文档

- [开发指南](../02-development/README.md)
- [运维与部署](../05-operations/README.md)
