# AMControlWinF 开发进展

**文档编号**：REL-W001  
**版本**：1.1.0  
**状态**：有效  
**最后更新**：2026-05-09  
**维护人**：Am

---

## 1. 项目概况

AMControlWinF 是 `ammm56/amcontrol` 仓库基于 AntdUI + .NET Framework 4.6.1 的 WinForms 工业设备控制软件，与 AMControlWPF 共享后端（AM.Core / AM.Model / AM.Tools / AM.DBService / AM.MotionService / AM.App / AM.PageModel）。

---

## 2. 已完成功能

### 2.1 基础框架

| 功能 | 状态 | 说明 |
|------|------|------|
| 解决方案分层 | ✅ | AMControlWinF / AM.PageModel / AM.App / AM.Core / AM.Model / AM.DBService / AM.MotionService / AM.Tools |
| Program.cs 主入口 | ✅ | while 主循环 + 登录/主窗体生命周期 |
| AppBootstrap 全局初始化 | ✅ | 设备/系统/扫描上下文一次性初始化（含 PLC） |
| ConfigContext 配置体系 | ✅ | 本地 config.json + 数据库混合配置 |

### 2.2 应用生命周期

| 功能 | 状态 | 说明 |
|------|------|------|
| 登录 → 主窗体 → 退出循环 | ✅ | `Application.Run(window)` 阻塞主循环 |
| MainWindowExitReason 枚举 | ✅ | Exit / SwitchUser / Logout 三种退出意图 |
| 切换用户（模态 LoginForm） | ✅ | 在当前窗体上弹出模态登录 → 成功则重建主窗体 |
| 退出登录（SignOut + 重新登录） | ✅ | 清除登录态 → 独立登录窗 → 成功则新主窗体 |
| 安全关闭（using 释放） | ✅ | 旧窗体资源释放后才创建新实例 |

### 2.3 用户认证

| 功能 | 状态 | 说明 |
|------|------|------|
| LoginForm 登录窗体 | ✅ | AntdUI.Window，纹理背景，暗/亮主题自适应 |
| LoginPageModel | ✅ | 数据绑定：LoginName / StatusText / ErrorMessage |
| 角色体系 | ✅ | Operator / Engineer / Am |
| 页面权限同步 | ✅ | NavigationCatalog → SysPagePermission 表 |
| 默认管理员 | ✅ | am / am123 |

### 2.4 主窗体壳层

| 功能 | 状态 | 说明 |
|------|------|------|
| MainWindow 壳层 | ✅ | GridPanel 三列布局 + 状态栏 |
| 一级/二级导航菜单 | ✅ | AntdUI.Menu + NavigationCatalog 驱动 |
| 页面缓存机制 | ✅ | Dictionary 缓存 + 工厂模式创建 |
| 导航图标映射 | ✅ | PageKey → AntdUI SVG Icon |
| 系统消息订阅与通知 | ✅ | MessageBus → 状态栏 + 弹出通知 |

### 2.5 主题系统

| 功能 | 状态 | 说明 |
|------|------|------|
| AppThemeHelper | ✅ | 全局唯一入口，AntdUI Config.IsDark/IsLight + Window 基色 |
| 卡片面板原生渲染 | ✅ | 不设置 Back，Shadow+Radius 由 AntdUI 原生处理 |
| TextureBackgroundControl | ✅ | 自定义纹理背景，SetTheme 切换 |
| 主题持久化 | ✅ | SkinDefault / SkinDark 保存到 config.json |

### 2.6 语言切换

| 功能 | 状态 | 说明 |
|------|------|------|
| 中/英双语 | ✅ | 壳层文本 + 导航菜单 + 用户菜单 |
| 语言持久化 | ✅ | zh-CN / en-US 保存到 config.json |
| 切换后页面重建 | ✅ | 清空缓存 + 完整刷新壳层 |

### 2.7 用户头像菜单

| 功能 | 状态 | 说明 |
|------|------|------|
| UserAvatarMenuControl | ✅ | 嵌入标题栏，显示头像 + 用户名 |
| UserAvatarPopoverCard | ✅ | 弹出操作卡片（切换用户/修改密码/退出登录） |
| 安全关闭模式 | ✅ | Dispose 前获取 ownerForm → BeginInvoke 延迟通知 |

### 2.8 用户管理

| 功能 | 状态 | 说明 |
|------|------|------|
| UserManagementPage | ✅ | 搜索+统计卡片+AntdUI Table，三行布局 |
| UserEditDialog | ✅ | 新增/编辑共用弹窗，LoginForm 风格 |
| ResetUserPasswordDialog | ✅ | 密码重置弹窗，LoginForm 风格 |
| 完整 CRUD | ✅ | 新增/编辑/删除/启用禁用/密码重置 |

### 2.9 后端基础设施（全部完成）

| 模块 | 状态 |
|------|------|
| 统一 Result/Result\<T\> 返回模型 | ✅ |
| ServiceBase（日志/消息/报警封装 + ReportChannels） | ✅ |
| IAppReporter / AppReporter | ✅ |
| AlarmManager + 持久化 + 启动恢复 | ✅ |
| IMessageBus / MessageBus | ✅ |
| ConfigContext / SystemContext / MachineContext / UserContext / RuntimeContext | ✅ |
| Auth 完整流程（登录/用户管理/角色/页面权限/种子数据） | ✅ |
| Motion 拓扑 CRUD（Card / Axis / IoMap）× 3 | ✅ |
| Motion 轴参数 CRUD + Overlay 覆盖 | ✅ |
| 执行器 CRUD × 4 + Runtime Service × 4 | ✅ |
| MachineConfigReloadService（DB → 内存重建） | ✅ |
| MachineConfigSeedService | ✅ |
| IoScanWorker + MotionAxisScanWorker + RuntimeTaskManager | ✅ |
| MotionServiceHub 统一路由调度 | ✅ |
| VirtualMotionCardService / GoogoMotionCardService / LeisaiMotionCardService | ✅ 框架完成 |
| RuntimeContext 运行时状态缓存（IO / 轴 / PLC） | ✅ |

### 2.10 PLC 后端（全部完成）

| 模块 | 状态 | 说明 |
|------|------|------|
| 数据模型（PlcStationConfig / PlcPointConfig / PlcConfig） | ✅ | 极简模型：Address 直接表达协议地址，DataType 字符串，Length 统一长度 |
| 数据库实体（plc_station / plc_point） | ✅ | SqlSugar CodeFirst |
| PlcStationCrudService / PlcPointCrudService | ✅ | 站 + 点位增删查改 |
| PlcConfigAppService | ✅ | DB → ConfigContext + MachineContext.Plcs 装配 |
| PlcConfigSeedService | ✅ | 默认 ModbusTcp 测试种子数据 |
| PlcClientFactory + ProtocolPlcClient | ✅ | AM 侧门面客户端，反射创建 IProtocol |
| NullPlcClient | ✅ | 占位客户端，保证启动链路完整 |
| ProtocolAssemblyRegistry | ✅ | Protocols/ 目录扫描 DLL，插件式注册协议实现 |
| PlcScanWorker | ✅ | 100ms 后台扫描，自动重连，RuntimeContext 更新 |
| PlcRuntimeQueryService | ✅ | 站/点位/全量快照查询（配置 + 运行时合并） |
| PlcOperationService | ✅ | 手动读写（按配置名称或直接地址） |
| ProtocolLib.ModbusTcp.Protocol | ✅ | 完整实现，含集成测试 |
| ProtocolLib.S7Tcp.Protocol | ✅ | 完整实现 |
| AppBootstrap PLC 集成 | ✅ | 种子→注册→装配→扫描，完整启动链路 |

---

## 3. 重要设计决策

| 时间 | 决策 | 原因 |
|------|------|------|
| 2026-03 | 移除 MainApplicationContext 状态机 | 事件链过深、竞态条件、已释放对象引用等 6+ 问题 |
| 2026-03 | 改用 Program.Main() while 主循环 | Application.Run 天然阻塞，using 天然释放，逻辑最简 |
| 2026-03 | 切换用户使用模态 LoginForm | 同步调用、无需异步状态机、取消时主窗体不变 |
| 2026-03 | 移除 IPageTheme 接口 | 后续页面实现负担大、AntdUI 原生已覆盖大部分场景 |
| 2026-04 | PLC 极简模型：移除 AreaType / 枚举 / BlockRead | 避免过度设计；Address 字符串直接表达完整协议地址更简洁 |
| 2026-04 | 协议插件化（ProtocolAssemblyRegistry） | 协议库与 AM 层解耦，新增协议无需修改 AM 层代码 |
| 2026-04 | NullPlcClient 占位方案 | 协议 DLL 不可用时保证启动不中断，运行时按需降级 |

---

## 4. 待开发功能

### 4.1 近期优先（Motion 监控）

| 功能 | 说明 |
|------|------|
| Motion.DI 监视页 | 磁贴卡片 + 分页 + SnapshotChanged 事件驱动刷新；左侧列表禁止刷新时滚动回顶部 |
| Motion.DO 监视页 | 复用 DI 实现 |
| Motion.Monitor 多轴总览 | 两列布局：控制卡筛选 + 轴卡片区 + 右侧详情 |
| System.Permission 权限分配页 | 按用户选择，按页面布局设计 |
| System.LoginLog 登录日志 | AntdUI Table + 分页 |

### 4.2 设备控制与配置

| 功能 | 说明 |
|------|------|
| Motion.Axis 轴控制 | 左侧动作卡片单击即执行；右侧始终显示实时监视；参数输入在对应动作卡片；消除底部空白 |
| Motion.Actuator 执行器控制 | 手动操作气缸、真空、夹爪、灯塔 |
| MotionConfig.Card / Axis / IoMap / AxisParam / Actuator | 运控配置 5 页 |
| 报警抽屉面板 | 主界面右侧展开，主页面轻微缩小产生后退效果 |

### 4.3 PLC UI（后端已全部完成）

| 功能 | 说明 |
|------|------|
| PLC.Monitor 站状态总览 | 卡片形式展示各站连接状态、扫描频率、统计信息 |
| PLC.Register 点位实时监视 | 按站/分组展示点位列表，SnapshotChanged 驱动刷新 |
| PLC.Status 连接状态详情 | 单站详情弹窗或右侧详情区 |
| PLC.Write 手动写入调试 | 直接地址写入，工程师权限限制 |
| SysConfig.Plc 站 & 点位配置管理 | 增删改查 + 重载配置 |

### 4.4 报警日志与系统管理

| 功能 | 说明 |
|------|------|
| AlarmLog.Current 当前报警 | 实时报警列表 + 清除操作 |
| AlarmLog.History 报警历史 | 分页查询 + 时间过滤 |
| AlarmLog.RunLog 运行日志 | NLog 日志查看 |
| Home.Overview 总览看板 | 设备状态汇总 |
| Home.SysStatus 系统状态 | 各子系统健康状态 |

### 4.5 远期规划

| 功能 | 说明 |
|------|------|
| Production 系列 | 工单、配方、数据、报表、追溯、MES |
| Vision 系列 | 相机监视、检测结果、标定管理 |
| Peripheral 系列 | 扫码器、传感器监视与趋势 |
| SysConfig 其余 | 相机/传感器/扫码器/MES/运行配置 |
| Engineer 系列 | 设备诊断、原始参数调试 |

---

## 5. 已知问题

| 问题 | 状态 | 说明 |
|------|------|------|
| 多数业务页面为占位页 | 已知 | 当前使用 `CreatePlaceholderPage()` 显示文本占位 |
| 修改密码功能 | 已知 | 当前为 MessageBox 占位，需实现 ResetUserPasswordDialog 挂接 |
| 页面工厂注册不完整 | 已知 | 仅注册了 UserManagement 等少数页面，其余显示"未实现页面" |

---

## 相关文档

- [AMControlWinF 项目总览](../02-development/winf-project-overview.md)
- [应用生命周期](../01-architecture/winf-application-lifecycle.md)
- [主题系统设计](../01-architecture/winf-theme-system.md)
- [导航系统设计](../01-architecture/winf-navigation-system.md)
- [用户认证与登录](../03-features/winf-auth-login.md)
- [主窗体壳层](../03-features/winf-mainwindow-shell.md)
- [PLC 通信功能文档](../03-features/plc-communication-planning.md)
- [PLC 协议库架构设计](../01-architecture/plc-protocol-integration-design.md)
