# 09-database-config · 数据库与配置文档

本目录存放 `ammm56/amcontrol` 项目的**数据库表结构说明**和**配置管理文档**，覆盖运行时配置（`config.json`）与数据库持久化配置两个来源。

使用边界说明：

1. 本目录优先记录当前仓库已经落地的表结构事实、表名映射和配置来源；
2. 当文档与代码冲突时，应优先参考 `AM.Model/Entity` 中各实体的 `[SugarTable]` 映射和当前配置模型；
3. 历史设计说明、旧命名或早期规划仅作为对照，不替代当前表结构事实。

---

## 当前表结构事实

### 数据库类型

项目使用 SqlSugar 支持以下数据库：

- **SQLite**（默认，开发调试推荐）
- **Access**（旧版兼容）
- **MySQL**（生产环境推荐）

当前仓库的数据库表名以**下划线命名**为主，字段名保持实体属性的 `PascalCase` 风格。以下列表以当前代码映射为准，优先反映已经存在的持久化事实，而不是早期规划草图。

### 主要业务表

#### Motion 拓扑与点位表

| 表名 | 实体 | 说明 |
|------|------|------|
| `motion_card` | `MotionCardEntity` | 运动控制卡（卡类型、驱动识别、初始化顺序） |
| `motion_axis` | `MotionAxisEntity` | 运动轴（归属卡、逻辑/物理编号、分类） |
| `motion_io_map` | `MotionIoMapEntity` | IO 映射（DI/DO 逻辑位 → 硬件位） |
| `motion_io_point_config` | `MotionIoPointConfigEntity` | IO 点位公共配置（显示名、取反、滤波、输出模式等） |
| `motion_io_wiring` | `MotionIoWiringEntity` | IO 接线/对端信息记录，用于装配接线与现场核对链路 |

#### Motion 参数与执行器表

| 表名 | 实体 | 说明 |
|------|------|------|
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
| `sys_user` | `SysUserEntity` | 用户账户（登录名、密码哈希、姓名、启用状态） |
| `sys_role` | `SysRoleEntity` | 角色定义（Operator / Engineer / Am） |
| `sys_user_role` | `SysUserRoleEntity` | 用户角色关联 |
| `sys_login_log` | `SysLoginLogEntity` | 登录日志 |
| `sys_page_permission` | `SysPagePermissionEntity` | 页面级权限配置 |
| `sys_user_page_permission` | `SysUserPagePermissionEntity` | 用户页面权限关联 |

#### 系统链路与报警表

| 表名 | 实体 | 说明 |
|------|------|------|
| `dev_alarm_record` | `DevAlarmRecordEntity` | 设备报警历史记录（含未清除状态，启动时恢复） |
| `sys_client_identity` | `SysClientIdentityEntity` | 设备客户端身份、注册状态与后台链路基础信息 |
| `sys_client_update_record` | `SysClientUpdateRecordEntity` | 客户端版本/更新记录与后台交互留痕 |
| `sys_usage_event_buffer` | `SysUsageEventBufferEntity` | 使用事件与上报缓冲数据 |

### 历史设计说明与旧口径对照

以下内容在旧文档、旧讨论或早期规划里可能仍会出现，但不代表当前事实：

1. 认证与报警表曾被写成 `SysUser`、`SysRole`、`DevAlarmRecord` 这类实体名口径；当前实际表名以 `sys_*`、`dev_*` 为准；
2. 早期表结构说明中常省略 `motion_io_point_config`、`motion_io_wiring` 和设备后台链路相关系统表；当前仓库这些表已经存在并参与运行链路；
3. 若某份旧文档中的表名、表数量或职责拆分与本页不同，应以当前实体映射和运行代码为准。

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

说明：与数据库表结构类似，`config.json` 只描述当前需要持久化的轻量配置；运行期由数据库装配进 `ConfigContext` 的对象不应被误解为 json 固有字段。

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

- [文档维护规则](../00-governance/document-maintenance-rules.md)
- [架构设计](../01-architecture/README.md)
- [功能模块 · 运动控制](../03-features/README.md)
- [运维与部署](../05-operations/README.md)
