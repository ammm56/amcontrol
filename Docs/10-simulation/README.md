# 10-simulation · 3D 仿真集成文档

本目录存放 `ammm56/amcontrol` 项目的 **3D 虚拟设备仿真集成**相关文档，覆盖 .NET 侧仿真接入设计、WebSocket 通信协议、Vue3 + Three.js 前端规划以及仿真流程脚本设计。

---

## 背景

项目使用 `VirtualMotionCardService` 在无真实运动控制卡的情况下进行开发调试。该虚拟卡服务已具备以下能力：

- `AxisPositionChanged` / `AxisStatusChanged` / `DOChanged` / `DIChanged` 事件向外推送实时状态；
- `InjectDI(short bit, bool value)` 接收外部 3D 仿真器注入的传感器反馈信号；
- `GetAllAxisStates()` / `GetAllDoValues()` / `GetAllDiValues()` 提供全量状态快照。

在此基础上，本仿真集成方案在 .NET 侧增加一个**仿真集成层**，通过 WebSocket 与独立的 Vue3 + Three.js 3D 仿真前端实时双向联动。

---

## 文件列表

| 文件 / 目录 | 说明 |
|-------------|------|
| [backend/net-integration-plan.md](backend/net-integration-plan.md) | .NET 侧 3D 仿真接入详细设计文档，包含项目结构、接口定义、DTO 设计、WebSocket 消息协议 |

---

## 设计原则

1. **不破坏现有 WPF 主控架构**：运动控制对象统一通过 `MachineContext` 访问，不在仿真服务中维护私有运动副本。
2. **配置来源统一**：仿真配置通过 `ConfigContext`（`config.json`）和数据库管理，不在前端硬编码轴号/位号映射。
3. **统一返回模型**：仿真网关接口使用与项目一致的 `Result/Result<T>` 返回模型。
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

- [架构设计](../01-architecture/README.md)
- [开发指南](../02-development/README.md)
- [测试文档](../04-testing/README.md)
