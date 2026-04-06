# Copilot Instructions

---

## 一、项目总览

本解决方案为工业设备运动控制软件，核心功能：
- 运动控制卡（固高 GOOGO / 雷赛 LEISAI / 虚拟 VIRTUAL）驱动几十个步进/伺服电机
- 运动控制卡 IO 控制设备传感器（DI/DO）
- 第三层执行器对象（气缸/真空/夹爪/灯塔）基于 IO 点位抽象
- 视觉检测与 PLC 通信（待实现）
- 用户权限、报警、日志、配置管理等工业软件基础设施

解决方案包含两个 UI 分支：
- **AMControlWPF**（.NET Framework 4.7.2 / WPF / HandyControl / CommunityToolkit.Mvvm）—— 已暂停
- **AMControlWinF**（.NET Framework 4.6.1 / WinForms / AntdUI）—— **当前活跃分支**

两者共享同一套后端项目（AM.Core / AM.Model / AM.Tools / AM.DBService / AM.MotionService / AM.App / AM.PageModel）。

---

## 二、解决方案分层架构

### 2.1 项目职责

| 项目 | 层级 | 职责 |
|------|------|------|
| **AM.Core** | 基础设施 | 全局上下文（ConfigContext / MachineContext / SystemContext / UserContext / RuntimeContext）、ServiceBase 基类、消息总线接口 IMessageBus、报警管理 AlarmManager、日志接口 IAMLogger、统一报告接口 IAppReporter |
| **AM.Model** | 模型层 | Entity（Auth / Motion 拓扑 / Motion 轴参数 / Motion 执行器 / PLC / Dev 报警）、运行时 Config 对象（MotionCardConfig / AxisConfig / ActuatorConfig / PlcConfig）、接口定义（IMotionCardService / IPlcClient）、运行时状态缓存（MotionIoRuntimeState / MotionAxisRuntimeState / PlcRuntimeState）、公共结构体（AxisStatus / AxisParam）、统一 Result/Result\<T\> 返回模型 |
| **AM.Tools** | 工具层 | NLog 实现 NLogLogger、配置文件读写 Tools.ReadConfig/SaveConfig、MessageBus 实现、AppReporter 实现、错误码目录 JsonErrorCatalog |
| **AM.DBService** | 数据服务 | SqlSugar 数据库操作；Auth 服务（AuthService / AuthSeedService）；Motion 拓扑 CRUD（MotionCardCrudService / MotionAxisCrudService / MotionIoMapCrudService）；Motion 轴参数（MotionAxisConfigService / MotionAxisConfigOverlayService）；执行器 CRUD×4；PLC 配置/驱动/运行时服务；Dev 报警记录（DevAlarmRecordService）；运行时（IoScanWorker / MotionAxisScanWorker / RuntimeTaskManager）；配置重载（MachineConfigReloadService / MachineConfigSeedService） |
| **AM.MotionService** | 运动控制 | MotionCardBase 公共基类；固高 GoogoMotionCardService / 雷赛 LeisaiMotionCardService / 虚拟 VirtualMotionCardService；MotionServiceHub 统一路由调度入口；工厂 MotionCardFactory |
| **AM.PageModel** | 页面模型 | WPF/WinForms 共用的无 UI 依赖中间层；NavigationCatalog（全局页面目录）、NavPageDef / NavPrimaryDef；LoginPageModel、MainWindowModel、MainWindowExitReason、UserManagementPageModel；BindableBase 轻量 INotifyPropertyChanged 基类 |
| **AM.App** | 启动器 | AppBootstrap 组合根，统一编排启动顺序 |
| **AMControlWPF** | WPF UI | 暂停。HandyControl + MVVM |
| **AMControlWinF** | WinForms UI | **当前活跃**。AntdUI 控件库 |

### 2.2 全局上下文体系（5 大单例）

| 上下文 | 位置 | 职责 |
|--------|------|------|
| `ConfigContext` | AM.Core/Context | config.json 基础配置 + 运行时从 DB 装载的 MotionCardsConfig / ActuatorConfig / PlcConfig（JsonIgnore，不写入 json） |
| `SystemContext` | AM.Core/Context | IAMLogger、IMessageBus、AlarmManager、IErrorCatalog、IAppReporter、IRuntimeTaskManager |
| `MachineContext` | AM.Core/Context | 控制卡实例字典 MotionCards\<CardId\>、轴/DI/DO→卡映射（AxisMotionCards / DICards / DOCards）、MotionHub、执行器索引（Cylinders / Vacuums / StackLights / Grippers 按 Name 索引）、PLC 客户端 Plcs |
| `UserContext` | AM.Core/Context | 当前登录用户 CurrentUser、角色 CurrentRoles、页面权限 CurrentPageKeys |
| `RuntimeContext` | AM.Core/Context | MotionIo（DI/DO 运行时缓存 ConcurrentDictionary + SnapshotChanged 事件）、MotionAxis（轴快照缓存 + SnapshotChanged 事件）、Plc（PLC 运行时状态） |

### 2.3 启动流程（AppBootstrap.Initialize）

1. 读取 config.json → `ConfigContext.Instance.Initialize(config)`
2. 构造 NLogLogger / MessageBus / AlarmManager / JsonErrorCatalog / AppReporter
3. `SystemContext.Instance.Initialize(...)` 注入所有基础设施
4. AuthSeedService 初始化认证表与默认管理员（am / am123）
5. MachineConfigSeedService 初始化运动配置种子数据
6. **MachineConfigReloadService.ReloadAndRebuild()** 从 DB 加载设备配置 → 重建 MachineContext（创建控制卡实例、轴/IO 映射、执行器注册）
7. 按 InitOrder 顺序 Initialize + Connect 所有控制卡
8. 注册 IoScanWorker（50ms IO 扫描）+ MotionAxisScanWorker（100ms 轴采样）→ RuntimeTaskManager 管理启停
9. Program.cs 主循环：登录 → `Application.Run(MainWindow)` → 按 ExitReason 决策（SwitchUser / Logout / Exit）

---

## 三、运动控制架构

### 3.1 接口分层

```
IMotionCardService（聚合接口）
├── IMotionCardConnection      Initialize / Connect / Disconnect
├── IMotionCardConfiguration   LoadAxisConfig(List<AxisConfig>)
├── IMotionAxisControl         Enable / Stop / StopAll / Home / HomeAsync
├── IMotionAxisMotion          MoveRelative / MoveAbsolute / JogMove（脉冲 + 毫米双套）
├── IMotionDigitalIO           SetDO / GetDI / GetDO
├── IMotionAxisParameter       SetVel / SetAcc / SetDec
├── IMotionAxisState           GetAxisStatus / Get[Command|Encoder]Position[Mm] / IsMoving
└── IMotionAxisMaintenance     ClearStatus / SetZeroPos / ConfigAxisHardware
```

### 3.2 实现类

- `MotionCardBase` —— 公共基类（AM.MotionService/Base）
- `VirtualMotionCardService` / `GoogoMotionCardService` / `LeisaiMotionCardService` —— 三种卡驱动
- `MotionServiceHub` —— 按逻辑轴/逻辑 IO 位路由到对应控制卡实例，注册到 `MachineContext.MotionHub`

### 3.3 控制卡类型枚举

`MotionCardType`：GOOGO(10) / LEISAI(20) / VIRTUAL(90) / Other(99)

---

## 四、数据模型与实体规范

### 4.1 Auth 认证（6 张表，PascalCase 连写表名）

SysUserEntity、SysRoleEntity、SysUserRoleEntity、SysLoginLogEntity、SysPagePermissionEntity、SysUserPagePermissionEntity

### 4.2 Motion 拓扑（3 张表，下划线分隔表名）

| 表名 | 实体 | 说明 |
|------|------|------|
| `motion_card` | MotionCardEntity | 控制卡基础信息、驱动识别、初始化顺序 |
| `motion_axis` | MotionAxisEntity | 轴归属、逻辑/物理编号、分类 |
| `motion_io_map` | MotionIoMapEntity | DI/DO 逻辑位→硬件位映射 |

### 4.3 Motion 轴参数（1 张表，KV 模式）

- `motion_axis_config` / MotionAxisConfigEntity —— (LogicalAxis + ParamName) 唯一；数值统一 REAL + ParamValueType 类型标记
- 不在表中加入 EnumOptions 等偏 UI 字段，枚举选项在代码中定义
- 不引入 AxisParameterCatalog 等把元数据大量固化的复杂设计

### 4.4 Motion IO 点位

- `motion_io_point_config` / MotionIoPointConfigEntity

### 4.5 执行器（第三层对象，4 张表）

- `motion_cylinder_config` / CylinderConfigEntity（单/双线圈气缸，伸出/缩回 IO + 反馈 + 超时 + 报警码）
- `motion_gripper_config` / GripperConfigEntity
- `motion_vacuum_config` / VacuumConfigEntity
- `motion_stacklight_config` / StackLightConfigEntity

### 4.6 PLC（3 张表）

PlcStationConfigEntity、PlcReadBlockConfigEntity、PlcPointConfigEntity

### 4.7 Dev 报警

DevAlarmRecordEntity

### 4.8 命名规范

- Motion 相关表名使用下划线分隔：`motion_card`、`motion_axis`、`motion_io_map` 等
- 表字段保持实体属性 PascalCase，不额外添加下划线字段映射
- 数据库实体统一使用 SqlSugar `[SugarTable]` / `[SugarColumn]` 标注风格
- 实体目录按类别组织：Entity/Auth、Entity/Motion/Topology、Entity/Motion/Actuator、Entity/Motion/Point、Entity/Plc、Entity/Dev
- 旧版运动配置实体与实现直接废弃删除，不做过渡兼容

---

## 五、导航体系

NavigationCatalog（AM.PageModel/Navigation）定义 10+ 个一级模块、40+ 二级页面：

| 一级模块 | 二级页面 |
|----------|----------|
| Home 首页 | Overview 总览看板、SysStatus 系统状态 |
| Motion 设备 | DI 监视、DO 监视、Monitor 多轴总览、Axis 轴控制、Actuator 执行器控制 |
| Production 生产 | Order、Recipe、Data、Report、Trace、MesStatus、UploadLog |
| Vision 视觉 | Monitor、Result、Calibrate |
| PLC | Monitor、Register、Status、Write |
| Peripheral 外设 | Scanner、ScanTest、Sensor、SensorTrend |
| MotionConfig 运控配置 | Card、Axis、IoMap、AxisParam、Actuator |
| SysConfig 系统配置 | Camera、Plc、Sensor、Scanner、Mes、Runtime |
| Engineer 工程 | Diagnostic、RawAxis、RawPlc、RawCamera |
| AlarmLog 报警与日志 | Current、History、RunLog |
| System 系统 | User 用户管理、Permission 权限分配、LoginLog 登录日志 |

每个页面定义了：ModuleKey、PageKey、DisplayName、Description、DefaultRoleCodes（Operator/Engineer/Am）、RiskLevel、SortOrder。

---

## 六、公共架构规范（WPF / WinForms 共享）

### 6.1 统一结果模型

- 全项目统一使用 `Result` / `Result<T>`（AM.Model.Common），不按领域拆分
- Result 包含：Success / Code / Message / Source / NotifyMode
- ResultNotifyMode：All / LogOnly / MessageOnly / Silent
- 失败结果始终全量通知；成功结果的通知渠道由 ServiceBase 方法控制

### 6.2 ServiceBase

- 所有服务继承 `ServiceBase`（AM.Core/Base），封装日志/消息/报警输出
- 提供 Ok / OkSilent / OkLogOnly / OkMessageOnly / Fail / Error / Alarm 等方法
- 高频后台扫描场景使用 OkSilent 避免日志/消息风暴
- 子类可重写 MessageSourceName / DefaultResultSource / MessageCardId

### 6.3 IAppReporter / AppReporter

- 统一封装日志（IAMLogger）+ 消息通知（IMessageBus）+ 报警（AlarmManager）
- 支持 ReportChannels 控制输出渠道（Log / Message / Alarm / All / None）
- ServiceBase、ViewModel、Tools 等均通过 Reporter 输出，不直接调用 IMessageBus / IAMLogger

### 6.4 报警系统

- AlarmManager（AM.Core/Alarm）管理内存中活动报警
- 持久化通过 IAlarmRecord / DevAlarmRecordService 写到 DB
- 启动时从 DB 恢复未清除报警
- AlarmCode 枚举定义报警码（PLCDisconnect / AxisServoAlarm / IoScanFailed 等）

### 6.5 消息总线

- IMessageBus / MessageBus（发布/订阅 SystemMessage）
- SystemMessage 包含 Type（Status/Info/Warning/Error/Alarm）、Source、Message、Code、Time
- MainWindow 订阅消息总线，更新状态栏 + 弹出通知

### 6.6 全局配置规范

- 语言/主题等界面配置定义在 Config.Setting 中，通过 ConfigContext 读写，不在各处维护私有副本
- 对启动阶段已初始化的全局对象（ConfigContext 等）不重复做空判断
- 运动控制对象通过 MachineContext 唯一入口访问，禁止各窗口持有私有副本
- 运行时状态统一通过 RuntimeContext 缓存，覆盖 Motion IO / 轴 / PLC，不允许各子系统零散维护

### 6.7 运行时状态与后台服务

- IoScanWorker：50ms 周期扫描 DI/DO → 写入 RuntimeContext.MotionIo → 触发 SnapshotChanged
- MotionAxisScanWorker：100ms 周期采样轴状态 → 写入 RuntimeContext.MotionAxis
- UI 层应以 ~500ms 低频采样刷新，不将每次缓存变化直接事件驱动到整页界面
- IRuntimeTaskManager 统一管理后台工作单元注册/启停

### 6.8 配置管理服务

- 按职责拆分：IMotionCardCrudService / IMotionAxisCrudService / IMotionIoMapCrudService
- 更高层聚合：MachineConfigAppService / MachineConfigReloadService（DB→内存重建）
- 轴参数覆盖：MotionAxisConfigOverlayService 将 DB 参数覆盖到 MotionCardConfig.AxisConfigs
- 执行器 CRUD×4（Cylinder / Gripper / Vacuum / StackLight）+ Runtime Service×4

### 6.9 认证与权限

- 默认管理员：登录名 `am`，初始密码 `am123`
- 角色编码：`Operator`（操作员）/ `Engineer`（工程师）/ `Am`（管理员）
- 现阶段只做页面级权限，权限从 DB 读取和保存
- 操作员可查看监视信息，限制写入/动作/配置/调试权限
- 权限分配页支持选择用户，按页面布局设计，不依赖外部弹窗传入用户

### 6.10 PLC 接口设计

- PLC 领域接口放到 `AM.Model.Interfaces.Plc` 下，不放 `Interfaces.DB.Plc`
- DB 目录只放数据库相关内容
- IPlcClient / IPlcClientFactory 定义在 AM.Model.Interfaces.Plc

### 6.11 文件组织与编码规范

- 新增代码按项目层级结构与职责统一放置，避免功能位置分散
- 文档统一放入 `Docs` 目录，遵循现有文件夹分层与 markdown 约定
- 实现设备控制代码从实际设备场景和全局架构出发，不写局部脱离现场的代码
- 大数据列表默认采用分页或性能友好方案
- 第三层对象注册到 MachineContext 按名称索引，不增加多个反查字典
- AxisConfig 采用强类型、统一业务命名，属性命名作为数据库轴参数名称标准

### 6.12 公共页面交互规范

- 切换用户：模态 LoginForm，登录成功后关闭当前 MainWindow 并创建新 MainWindow；取消则保持当前主界面不变
- 退出登录：关闭主界面并返回登录页
- 修改密码：轻量弹窗操作，不占用右侧主工作区页面
- 只有管理员的用户管理进入一级/二级导航对应页面
- 用户编辑对话框：新增/编辑共用，新增模式窗口尺寸更大，编辑模式更紧凑
- 数据处理成功/失败由系统消息总线统一展示，页面不额外弹成功/失败对话框
- 页面只保留必要的交互确认类对话框

---

## 七、AMControlWPF 专属规范（WPF 分支，已暂停）

> WPF 分支当前已暂停开发，以下规范保留以备后续恢复。

### 7.1 技术栈

- .NET Framework 4.7.2
- WPF + HandyControl（主题/控件库）
- CommunityToolkit.Mvvm（MVVM 框架，但 **不使用** 源生成器 `[ObservableProperty]`，采用手动字段+属性）
- AM.ViewModel 项目承载 WPF ViewModel

### 7.2 WPF 主题与样式

- 复用 HandyControl 原生 Theme/Skin，通过项目自定义资源字典追加样式，不复制整套主题
- LangThemeHelper 保持最小实现，直接基于 ConfigContext 读写
- 主题切换由 AppThemeHelper 处理

### 7.3 WPF 布局规范

- 主界面左侧两级固定可见导航（一级+二级同时显示，不折叠展开）
- 右侧显示当前二级页面完整工作区，不堆叠多个二级页面
- 可复用样式下沉到 `Resources/Themes/Styles/Style.xaml`，减少 MainWindow.xaml 内联定义
- 复杂布局中关键 Grid 行使用 x:Name 便于调试，减少多余 RowDefinition

### 7.4 WPF 数据刷新

- 按 MVVM 数据绑定思路刷新，避免 DispatcherTimer 轮询式刷新
- 优先数据变化驱动刷新

### 7.5 WPF 页面生命周期

- 被 MainWindow 缓存复用的页面不在 Unloaded 中释放 ViewModel/运行态绑定
- 首次加载用布尔标记控制，不用 Loaded 后解绑事件
- 代码中补充注释说明原因

### 7.6 WPF 页面布局细节

- DI/DO 监视页：顶部按控制卡筛选支持多卡，无"全部/DI/DO"切换按钮，无左侧分组导航，直接展示磁贴卡片
- DI/DO 卡片：不显示控制卡信息和扫描时间，DI/DO 与 ON/OFF 紧连显示，需分页
- Motion.Monitor：两列布局（左侧卡片+右侧详情），顶部选控制卡后显示全轴
- 报警抽屉面板：主界面右侧展开，展开时主页面轻微缩小产生 3D 后退效果
- 权限分配页：方块整体背景做细微颜色变化，小圆角，与 HandyControl 风格一致

### 7.7 WPF 按钮样式

- 优先复用 HandyControl 和现有页面风格，不使用额外自定义按钮样式
- 允许保留与执行器类型对应的颜色区分

---

## 八、AMControlWinF 专属规范（WinForms 分支，当前活跃）

### 8.1 技术栈

- .NET Framework 4.6.1
- WinForms + AntdUI 控件库（Demo/AntdUI-Demo 为控件使用示例）
- AM.PageModel 项目作为 WPF/WinForms 共用的页面模型中间层（不依赖任何 UI 框架）
- 数据库同时支持 SQLite / Access / MySQL（SqlSugar）

### 8.2 WinForms 程序生命周期

```
Program.Main()
├── AppBootstrap.Initialize()     // 全局唯一初始化
├── SyncPagePermissions()         // 同步页面权限目录到 DB
├── ShowLogin() → LoginForm       // 首次登录
└── while(true) 主循环
    ├── Application.Run(MainWindow)
    └── 根据 ExitReason 决策：
        ├── SwitchUser → 直接下一轮
        ├── Logout → SignOut + 重新登录
        └── Exit → 退出
```

### 8.3 WinForms 主题切换

- `AppThemeHelper.Apply(window, isDarkMode)` → 设置 AntdUI 全局亮暗 + Window 色值
- `TextureBackgroundControl.SetTheme(isDarkMode)` → 自定义纹理背景同步
- AntdUI.Panel 带 Shadow 时原生主题渲染已包含正确的卡片背景、边界和阴影，完全交由 AntdUI 原生处理
- 明暗主题优先使用 AntdUI 默认样式，避免自定义导致样式异常

### 8.4 WinForms 主界面布局

- 左侧两级固定可见导航（一级 Menu + 二级 Menu），右侧工作区
- 页面缓存机制：`_pageCache` 字典 + `_pageFactories` 工厂，已访问页面缓存复用
- 底部状态栏显示系统消息（语义色区分：错误/报警红色，警告橙色）
- 用户头像菜单控件：切换用户 / 修改密码 / 退出登录
- 语言切换（中/英）+ 主题切换（亮/暗）
- 消息通知：订阅 SystemContext.MessageBus，状态栏更新 + AntdUI.Message 弹出

### 8.5 WinForms 页面实现规范

- 被 MainWindow 页面缓存复用的页面：不在离开时释放 ViewModel；首次加载用布尔标记 `_isFirstLoad`
- 页面内校验/信息提示不优先用 AntdUI.Message，倾向用 AntdUI.Modal 对话框
- 数据处理成功/失败由系统消息总线统一展示，页面不额外弹成功/失败对话框
- 页面只保留必要的交互确认类对话框
- 主界面重构时页面显示内容完整不变，只允许替换布局控件
- 优先使用 AntdUI 的 FlowPanel、Panel、GridPanel 等布局控件
- 主界面简化时删除多余抽象（ShellRefreshScope / Model_PropertyChanged / ConfigureShellLayout），保留最少的刷新与页面切换逻辑

### 8.6 WinForms 窗体/对话框风格

- LoginForm / UserEditDialog / ResetUserPasswordDialog 统一风格：纹理背景（TextureBackgroundControl）+ 大卡片
- 卡片分三行：顶部固定标题说明，中间可滚动表单区，底部固定右对齐按钮栏
- LoginForm 更贴合 AMControlWinF 现有样式与颜色，背景使用 TextureBackgroundControl 并支持主题切换
- 新增用户模式窗口尺寸更大，编辑用户模式更紧凑

### 8.7 WinForms 用户管理页

- 三行布局：第一行（搜索框+操作按钮）→ 第二行（统计卡片）→ 第三行（AntdUI Table 占满整行）
- 取消右侧详情栏，操作按钮放搜索行下方

### 8.8 WinForms 监视页规范（DI/DO/Motion.Monitor）

- DI/DO 监视页：顶部按控制卡筛选多卡，无分组导航，直接展示磁贴卡片
- DI/DO 卡片：不显示控制卡信息和扫描时间，DI/DO 与 ON/OFF 紧连显示颜色明显区分，需分页
- Motion.Monitor 多轴总览：两列布局，顶部选控制卡后显示全部轴卡片，右侧详情区
- 报警抽屉面板：主界面右侧展开，展开时主页面整体轻微缩小产生 3D 后退效果

---

## 九、当前开发进度

### 9.1 后端基础设施（全部完成）

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
| 执行器 CRUD×4 + Runtime Service×4 | ✅ |
| MachineConfigReloadService（DB→内存重建） | ✅ |
| MachineConfigSeedService | ✅ |
| IoScanWorker + MotionAxisScanWorker + RuntimeTaskManager | ✅ |
| MotionServiceHub 统一路由调度 | ✅ |
| VirtualMotionCardService / GoogoMotionCardService / LeisaiMotionCardService | ✅ 框架完成 |
| RuntimeContext 运行时状态缓存（IO / 轴 / PLC） | ✅ |
| PLC 接口分层（IPlcClient / Config / Runtime） | ✅ 接口定义完成 |
| PLC 驱动实现 | ❌ 待实现 |
| 视觉模块 | ❌ 待实现 |

### 9.2 WinForms UI 进度（AMControlWinF）

| 页面/组件 | 状态 |
|-----------|------|
| Program.cs（主循环 + 登录流程） | ✅ |
| AppBootstrap（启动编排） | ✅ |
| LoginForm（纹理背景 + AntdUI 主题 + 数据绑定） | ✅ |
| MainWindow（左侧两级导航 + 页面缓存 + 消息通知 + 语言/主题 + 状态栏） | ✅ |
| TextureBackgroundControl（明暗纹理背景） | ✅ |
| UserAvatarMenuControl + UserAvatarPopoverCard | ✅ |
| AppThemeHelper / PageDialogHelper | ✅ |
| UserManagementPage（搜索+统计+Table+CRUD） | ✅ |
| UserEditDialog（新增/编辑共用 + LoginForm 风格） | ✅ |
| ResetUserPasswordDialog（LoginForm 风格） | ✅ |
| Motion.DI / Motion.DO 监视页 | ❌ Placeholder |
| Motion.Monitor 多轴总览 | ❌ Placeholder |
| Motion.Axis 轴控制 | ❌ Placeholder |
| Motion.Actuator 执行器控制 | ❌ Placeholder |
| System.Permission 权限分配页 | ❌ Placeholder |
| System.LoginLog 登录日志 | ❌ Placeholder |
| MotionConfig.* 运控配置（5页） | ❌ Placeholder |
| AlarmLog.* 报警与日志（3页） | ❌ Placeholder |
| Home.* 首页（2页） | ❌ Placeholder |
| Production / Vision / PLC / Peripheral / SysConfig / Engineer | ❌ Placeholder |

### 9.3 推荐开发顺序

**阶段 1：核心设备监视**
1. Motion.DI → DI 监视页（磁贴卡片 + 分页 + 运行时事件驱动刷新）
2. Motion.DO → DO 监视页（复用 DI 实现）
3. Motion.Monitor → 多轴总览（两列布局 + 卡片 + 详情）
4. System.Permission → 权限分配页

**阶段 2：设备控制与运控配置**
5. Motion.Axis → 单轴控制
6. Motion.Actuator → 执行器手动操作
7. MotionConfig.* → 运控配置 5 页
8. 报警抽屉面板

**阶段 3：报警日志与系统管理**
9. AlarmLog.* → 报警与日志 3 页
10. System.LoginLog → 登录日志
11. Home.* → 总览与状态看板

**阶段 4：生产/视觉/PLC/外设**
12. 按实际硬件对接进度推进

### 9.4 控制卡详情展示

- 控制卡详情优先使用 AntdUI Popover 气泡卡片方式展示，更轻量，并参考 AntdUI Demo 的 Popover 用法保持现有项目风格一致。
