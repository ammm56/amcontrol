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

| 表名 | 说明 |
|------|------|
| `motion_card` | 运动控制卡配置（卡类型、卡号、启用状态等） |
| `motion_axis` | 运动轴配置（轴参数、软件限位、速度、回零参数等） |
| `motion_io_map` | IO 映射配置（DI/DO 位号、名称、关联设备） |
| `auth_user` | 用户账户（登录名、密码哈希、角色） |
| `auth_role` | 角色定义（Operator / Engineer / Am） |
| `auth_permission` | 页面级权限配置 |
| `alarm_record` | 报警历史记录 |

### ConfigContext 配置项

`config.json`（位于 `Configuration/` 目录）管理以下轻量配置：

- 程序语言/主题选项
- 数据库连接类型与路径
- 是否启用 3D 仿真（仿真服务端口等）

### 配置管理服务

配置管理服务按职责拆分：

- `IMotionCardCrudService`：运动控制卡配置的增删改查
- `IMotionAxisCrudService`：运动轴配置的增删改查
- `IMotionIoMapCrudService`：IO 映射配置的增删改查

---

## 相关文档

- [架构设计](../01-architecture/README.md)
- [功能模块 · 运动控制](../03-features/README.md)
- [运维与部署](../05-operations/README.md)
