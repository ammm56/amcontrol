# 09-database-config · 数据库与配置文档

本目录存放 `ammm56/amcontrol` 项目的**数据库表结构说明**和**配置管理文档**，覆盖运行时配置（`config.json`）与数据库持久化配置两个来源。

---

## 内容范围

### 数据库配置

项目使用 SqlSugar 支持以下数据库：

- **SQLite**（默认，开发调试推荐）
- **Access**（旧版兼容）
- **MySQL**（生产环境推荐）

主要配置表（命名采用下划线风格）：

#### 运动控制表

| 表名 | 实体 | 说明 |
|------|------|------|
| `motion_card` | `MotionCardEntity` | 运动控制卡（卡类型、驱动识别、初始化顺序） |
| `motion_axis` | `MotionAxisEntity` | 运动轴（归属卡、逻辑/物理编号、分类） |
| `motion_io_map` | `MotionIoMapEntity` | IO 映射（DI/DO 逻辑位 → 硬件位） |
| `motion_axis_config` | `MotionAxisConfigEntity` | 轴参数（KV 模式：LogicalAxis + ParamName 唯一，数值统一 REAL） |
| `motion_cylinder_config` | `CylinderConfigEntity` | 气缸执行器（单/双线圈、伸出/缩回 IO + 反馈 + 超时 + 报警码） |
| `motion_gripper_config` | `GripperConfigEntity` | 夹爪执行器 |
| `motion_vacuum_config` | `VacuumConfigEntity` | 真空执行器 |
| `motion_stacklight_config` | `StackLightConfigEntity` | 灯塔执行器 |

#### PLC 表

| 表名 | 实体 | 说明 |
|------|------|------|
| `plc_station` | `PlcStationConfigEntity` | PLC 站配置（厂商/型号/协议/连接/通讯参数） |
| `plc_point` | `PlcPointConfigEntity` | PLC 点位（Address + DataType + Length 极简模型） |

#### 认证与权限表

| 表名 | 实体 | 说明 |
|------|------|------|
| `SysUser` | `SysUserEntity` | 用户账户（登录名、密码哈希、姓名、启用状态） |
| `SysRole` | `SysRoleEntity` | 角色定义（Operator / Engineer / Am） |
| `SysUserRole` | `SysUserRoleEntity` | 用户角色关联 |
| `SysLoginLog` | `SysLoginLogEntity` | 登录日志 |
| `SysPagePermission` | `SysPagePermissionEntity` | 页面级权限配置 |
| `SysUserPagePermission` | `SysUserPagePermissionEntity` | 用户页面权限关联 |

#### 报警表

| 表名 | 实体 | 说明 |
|------|------|------|
| `DevAlarmRecord` | `DevAlarmRecordEntity` | 设备报警历史记录（含未清除状态，启动时恢复） |

### ConfigContext 配置项

`config.json`（位于程序运行目录）管理以下轻量配置：

| 字段 | 说明 |
|------|------|
| Language | 界面语言（zh-CN / en-US） |
| SkinType | 主题（SkinDefault / SkinDark） |
| DBType | 数据库类型（Sqlite / Access / MySQL） |
| DBPath | SQLite/Access 数据库文件路径 |
| ConnectionString | MySQL 连接字符串 |
| IoScanConfig | IO 扫描间隔与 AutoStart |
| MotionCardsConfig | （JsonIgnore）从 DB 装配后注入，不写入 json |
| PlcConfig | （JsonIgnore）从 DB 装配后注入，不写入 json |

### 配置管理服务

配置管理服务按职责拆分：

#### 运动控制配置

| 服务 | 职责 |
|------|------|
| `IMotionCardCrudService` | 控制卡 CRUD |
| `IMotionAxisCrudService` | 轴拓扑 CRUD |
| `IMotionIoMapCrudService` | IO 映射 CRUD |
| `IMotionAxisConfigService` | 轴参数 KV CRUD |
| `MotionAxisConfigOverlayService` | DB 参数 → MotionCardConfig.AxisConfigs 覆盖 |
| `MachineConfigAppService` | 配置应用聚合（DB → ConfigContext） |
| `MachineConfigReloadService` | 从 DB 重载并重建 MachineContext（卡实例/轴映射/执行器） |
| `MachineConfigSeedService` | 运动控制种子数据初始化 |
| 执行器 CRUD×4 | CylinderCrudService / GripperCrudService / VacuumCrudService / StackLightCrudService |

#### PLC 配置

| 服务 | 职责 |
|------|------|
| `IPlcStationCrudService` | PLC 站 CRUD（plc_station 表） |
| `IPlcPointCrudService` | PLC 点位 CRUD（plc_point 表） |
| `PlcConfigAppService` | DB → PlcConfig → ConfigContext + MachineContext.Plcs |
| `PlcConfigSeedService` | PLC 配置种子数据初始化 |

---

## 相关文档

- [架构设计](../01-architecture/README.md)
- [功能模块 · 运动控制](../03-features/README.md)
- [运维与部署](../05-operations/README.md)
