# 03-features · 功能模块文档

本目录按**业务功能模块**组织文档，覆盖 `ammm56/amcontrol` 项目各核心功能的设计说明、实现逻辑和使用注意事项。

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
| [software-license-design.md](software-license-design.md) | 设备软件授权设计（license.lic 文件方案、设备侧请求 JSON、授权数据、页面授权清单、运行时接入规则） |
| [license-file-validation-flow.md](license-file-validation-flow.md) | 设备侧 license.lic 读取、解密、RSA 验签、硬件绑定校验与最小功能回退流程 |
| [license-runtime-integration.md](license-runtime-integration.md) | 设备侧运行时授权上下文、pageKeys 与当前用户页面权限交集、导航过滤接入说明 |
| [device-license-and-reporting.md](device-license-and-reporting.md) | 设备软件授权申请与设备信息上报说明（apply、register、heartbeat、report、返回语义、联调顺序） |
| [plc-communication.md](plc-communication.md) | PLC 通信模块实现说明（配置模型、服务接口、协议库、扫描链路、UI 页面映射、调用示例） |
| [plc-communication-planning.md](plc-communication-planning.md) | PLC 通信模块功能文档（配置模型、数据库实体、服务职责边界、页面规划与实现状态） |

---

## 相关文档

- [架构设计](../01-architecture/README.md)
- [数据库与配置](../09-database-config/README.md)
