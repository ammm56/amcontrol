# 10-simulation · 3D 仿真集成文档

本目录存放 `ammm56/amcontrol` 项目的 **3D 虚拟设备仿真集成**相关文档。

当前说明：

1. 本目录内容以历史规划和中长期设计资料为主；
2. 当前活跃 WinForms 分支尚未形成完整的 3D 仿真实现链路；
3. 本目录不作为当前仓库事实的优先来源；
4. 若与当前实现冲突，应优先参考代码、[02-development/winf-project-overview.md](../02-development/winf-project-overview.md) 和 [07-release-notes/winf-development-progress.md](../07-release-notes/winf-development-progress.md)。

---

## 背景

项目使用 `VirtualMotionCardService` 在无真实运动控制卡的情况下进行开发调试。该虚拟卡服务已具备以下能力：

- `AxisPositionChanged` / `AxisStatusChanged` / `DOChanged` / `DIChanged` 事件向外推送实时状态；
- `InjectDI(short bit, bool value)` 接收外部 3D 仿真器注入的传感器反馈信号；
- `GetAllAxisStates()` / `GetAllDoValues()` / `GetAllDiValues()` 提供全量状态快照。

在此基础上，本目录中的方案文档主要用于保留早期和中长期的 3D 仿真接入设想，供后续恢复该方向时对照使用。

---

## 文件列表

| 文件 / 目录 | 说明 |
|-------------|------|
| [backend/net-integration-plan.md](backend/net-integration-plan.md) | .NET 侧 3D 仿真接入历史规划文档，保留早期 WPF/仿真集成方向设计，仅供对照 |

---

## 历史方案中的设计原则

以下原则仅用于保留早期仿真方案的设计取向，不构成当前仓库或未来新方案的强制约束：

1. **不破坏当时 WPF 主控架构**：运动控制对象统一通过 `MachineContext` 访问，不在仿真服务中维护私有运动副本。
2. **配置来源统一**：仿真配置通过 `ConfigContext`（`config.json`）和数据库管理，不在前端硬编码轴号/位号映射。
3. **统一返回模型**：仿真网关接口使用当时项目中的 `Result/Result<T>` 返回模型。
4. **独立子系统**：3D 仿真前端作为独立应用，通过 WebSocket 与 .NET 侧实时同步，不内嵌到 WPF 窗口中。

---

## 子目录说明

```
10-simulation/
  backend/        # .NET 侧仿真集成设计文档
  frontend/       # Vue3 + Three.js 前端规划文档（待补充）
  protocols/      # WebSocket 消息协议定义（待补充）
  flow-scripts/   # 仿真流程脚本模板（待补充）
```

---

## 相关文档

- [文档维护规则](../00-governance/document-maintenance-rules.md)
- [架构设计](../01-architecture/README.md)
- [开发指南](../02-development/README.md)
- [测试文档](../04-testing/README.md)
