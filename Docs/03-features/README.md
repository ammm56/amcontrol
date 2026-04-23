# 03-features · 功能模块文档

本目录按**业务功能模块**组织文档，覆盖 `ammm56/amcontrol` 项目各核心功能的设计说明、实现逻辑和使用注意事项。

当同一主题同时存在“实现文档”和“规划文档”时，默认按以下方式使用：

1. 优先阅读标记为“实现说明”“运行时流程”“当前实现”的文档；
2. 规划文档仅用于背景对照、历史设计取舍和未落地部分补充；
3. 若实现与规划描述冲突，以当前代码和实现说明文档为准。

---

## 内容范围

| 功能模块 | 说明 |
|----------|------|
| 运动控制 | 固高/雷赛运动控制卡接入、轴配置、回零流程、速度规划、急停/平停联动 |
| IO 管理 | DI/DO 扫描缓存机制、IO 映射配置、IO 监视页面 |
| 气缸与执行器 | `Cylinder`/`Actuator` 层设计、状态机、DI/DO 绑定 |
| 视觉功能 | 工业相机接入、视觉触发与结果回写 |
| PLC 通信 | 支持的 PLC 类型、通信协议配置、信号映射 |
| 权限管理 | 用户角色体系（Operator/Engineer/Am）、页面级权限、用户管理 |
| 报警系统 | 报警持久化（`IAlarmRecord`/`AlarmRecordService`）、报警抽屉面板、消息链路 |
| 3D 仿真 | 详见 [10-simulation](../10-simulation/README.md) |

---

## 子目录建议

```
03-features/
  motion-control/
  io-management/
  cylinder-actuator/
  vision/
  plc-communication/
  permission-management/
  alarm-system/
```

---

## AMControlWinF 功能文档

| 文件 | 说明 |
|------|------|
| [winf-auth-login.md](winf-auth-login.md) | WinForms 用户认证与登录（角色体系、LoginForm、切换用户/退出登录流程、PopoverCard 安全关闭） |
| [winf-mainwindow-shell.md](winf-mainwindow-shell.md) | WinForms 主窗体壳层（布局结构、主题/语言切换、系统消息、用户菜单交互） |
| [software-license-design.md](software-license-design.md) | 设备软件授权设计说明，偏授权模型与许可文件结构约束 |
| [license-file-validation-flow.md](license-file-validation-flow.md) | 设备侧 license.lic 读取、解密、RSA 验签、硬件绑定校验与最小功能回退流程（当前实现） |
| [license-runtime-integration.md](license-runtime-integration.md) | 设备侧运行时授权上下文、pageKeys 与当前用户页面权限交集、导航过滤接入说明（当前实现） |
| [device-license-and-reporting.md](device-license-and-reporting.md) | 设备软件授权申请与设备信息上报说明，偏接口联调口径与返回语义 |
| [device-management-runtime-flow.md](device-management-runtime-flow.md) | 设备注册、token 刷新、设备心跳、使用事件上报与结构化 report 的后台实现说明（当前实现） |
| [native-secure-runtime-design.md](native-secure-runtime-design.md) | 本地 Native 安全模块历史规划归档说明（当前仓库未落地，现阶段仅供对照） |
| [plc-communication.md](plc-communication.md) | PLC 通信模块实现说明（当前实现，优先于规划文档） |
| [plc-communication-planning.md](plc-communication-planning.md) | PLC 通信模块历史规划归档说明，仅用于对照早期方案，当前实现以 [plc-communication.md](plc-communication.md) 为准 |

---

## 相关文档

- [架构设计](../01-architecture/README.md)
- [数据库与配置](../09-database-config/README.md)
