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

### 本地视觉 SDK / amvision 调用封装

| 目录 | 说明 |
|------|------|
| `Libsrc/amvision/src/Amvision.Workflows` | amvision .NET SDK 源码，包含 Workflow 管理 HTTP client 与 ZeroMQ TriggerSource 调用能力 |
| `Libsrc/amvision/apps/Amvision.Workflows.Console` | 面向 `net461;net472;net10.0` 的现场调用封装和 console 参考实现 |
| `Libsrc/amvision/apps/Amvision.Workflows.Console/Config` | 实际视觉调用配置来源，包含 backend、runtime、TriggerSource、ZeroMQ endpoint 等调用参数 |

amcontrol 后续视觉调用应优先通过 `WorkflowOperationRunner` 使用上述封装。`amcontrol` 的 `config.json` 不重复保存 `DefaultAccessToken`、`DefaultZeroMqEndpoint`、runtime id 或 TriggerSource id。

---

## 使用说明

- 运动控制卡 SDK 的 DLL 文件直接放置在 `AM.MotionService/` 项目目录下，项目构建时自动复制到输出目录。
- 所有第三方 NuGet 包通过各 `.csproj` 的 `packages.config` 管理（.NET Framework 风格）。
- 如需查看具体版本号，请参阅各项目的 `packages.config` 文件。
- amvision SDK 与 Console 封装当前以源码形式放在 `Libsrc/amvision`，后续接入主解决方案时需确认 SDK-style 项目引用、依赖复制和 `Config/config_*.json` 输出目录策略。

---

## 相关文档

- [开发指南](../02-development/README.md)
- [运维与部署](../05-operations/README.md)
- [视觉、相机与 amvision SDK 集成规划](../03-features/vision-camera-sdk-integration-planning.md)
