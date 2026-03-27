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

## 相关文档

- [架构设计](../01-architecture/README.md)
- [数据库与配置](../09-database-config/README.md)
