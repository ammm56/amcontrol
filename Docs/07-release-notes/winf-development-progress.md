# AMControlWinF 开发进展

**文档编号**：REL-W001  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 项目概况

AMControlWinF 是 `ammm56/amcontrol` 仓库 `winform461` 分支上基于 AntdUI + .NET Framework 4.6.1 的 WinForms 工业设备控制软件。

---

## 2. 已完成功能

### 2.1 基础框架（已完成）

| 功能 | 状态 | 说明 |
|------|------|------|
| 解决方案分层 | ✅ | AMControlWinF / AM.PageModel / AM.App / AM.Core / AM.Model / AM.DBService / AM.MotionService / AM.Tools |
| Program.cs 主入口 | ✅ | while 主循环 + 登录/主窗体生命周期 |
| AppBootstrap 全局初始化 | ✅ | 设备/系统/扫描上下文一次性初始化 |
| ConfigContext 配置体系 | ✅ | 本地 config.json + 数据库混合配置 |

### 2.2 应用生命周期（已完成）

| 功能 | 状态 | 说明 |
|------|------|------|
| 登录 → 主窗体 → 退出循环 | ✅ | `Application.Run(window)` 阻塞主循环 |
| MainWindowExitReason 枚举 | ✅ | Exit / SwitchUser / Logout 三种退出意图 |
| 切换用户（模态 LoginForm） | ✅ | 在当前窗体上弹出模态登录 → 成功则重建主窗体 |
| 退出登录（SignOut + 重新登录） | ✅ | 清除登录态 → 独立登录窗 → 成功则新主窗体 |
| 安全关闭（using 释放） | ✅ | 旧窗体资源释放后才创建新实例 |

### 2.3 用户认证（已完成）

| 功能 | 状态 | 说明 |
|------|------|------|
| LoginForm 登录窗体 | ✅ | AntdUI.Window，纹理背景，暗/亮主题自适应 |
| LoginPageModel | ✅ | 数据绑定：LoginName / StatusText / ErrorMessage |
| 角色体系 | ✅ | Operator / Engineer / Am |
| 页面权限同步 | ✅ | NavigationCatalog → SysPagePermission 表 |
| 默认管理员 | ✅ | am / am123 |

### 2.4 主窗体壳层（已完成）

| 功能 | 状态 | 说明 |
|------|------|------|
| MainWindow 壳层 | ✅ | GridPanel 三列布局 + 状态栏 |
| 一级/二级导航菜单 | ✅ | AntdUI.Menu + NavigationCatalog 驱动 |
| 页面缓存机制 | ✅ | Dictionary 缓存 + 工厂模式创建 |
| 导航图标映射 | ✅ | PageKey → AntdUI SVG Icon |
| 系统消息订阅与通知 | ✅ | MessageBus → 状态栏 + 弹出通知 |

### 2.5 主题系统（已完成）

| 功能 | 状态 | 说明 |
|------|------|------|
| AppThemeHelper | ✅ | 全局唯一入口，AntdUI Config.IsDark/IsLight + Window 基色 |
| 卡片面板原生渲染 | ✅ | 不设置 Back，Shadow+Radius 由 AntdUI 原生处理 |
| TextureBackgroundControl | ✅ | 自定义纹理背景，SetTheme 切换 |
| 主题持久化 | ✅ | SkinDefault / SkinDark 保存到 config.json |

### 2.6 语言切换（已完成）

| 功能 | 状态 | 说明 |
|------|------|------|
| 中/英双语 | ✅ | 壳层文本 + 导航菜单 + 用户菜单 |
| 语言持久化 | ✅ | zh-CN / en-US 保存到 config.json |
| 切换后页面重建 | ✅ | 清空缓存 + 完整刷新壳层 |

### 2.7 用户头像菜单（已完成）

| 功能 | 状态 | 说明 |
|------|------|------|
| UserAvatarMenuControl | ✅ | 嵌入标题栏，显示头像 + 用户名 |
| UserAvatarPopoverCard | ✅ | 弹出操作卡片（切换用户/修改密码/退出登录） |
| 安全关闭模式 | ✅ | Dispose 前获取 ownerForm → BeginInvoke 延迟通知 → sender 传 null |

---

## 3. 重要设计决策

| 时间 | 决策 | 原因 |
|------|------|------|
| 2026-03 | 移除 MainApplicationContext 状态机 | 事件链过深、竞态条件、已释放对象引用等 6+ 问题 |
| 2026-03 | 改用 Program.Main() while 主循环 | Application.Run 天然阻塞，using 天然释放，逻辑最简 |
| 2026-03 | 切换用户使用模态 LoginForm | 同步调用、无需异步状态机、取消时主窗体不变 |
| 2026-03 | 移除 IPageTheme 接口 | 后续页面实现负担大、AntdUI 原生已覆盖大部分场景 |
| 2026-03 | 移除 AppThemeHelper.ApplyCardPanel | 显式设置 Back 绕过 AntdUI 原生主题渲染 |
| 2026-03 | AM.PageModel 作为公共中间层 | WPF/WinForms 共享导航、登录、主窗体模型 |

---

## 4. 待开发功能

### 4.1 近期优先

| 功能 | 优先级 | 说明 |
|------|--------|------|
| 修改密码弹窗 | 高 | 替换当前占位 MessageBox |
| 用户管理页 | 高 | System.User 页面实现 |
| 权限分配页 | 高 | System.Permission 页面实现 |
| DI/DO 监视页 | 高 | Motion.DI / Motion.DO 页面实现 |

### 4.2 中期计划

| 功能 | 说明 |
|------|------|
| Motion.Monitor 多轴总览 | 两列布局：控制卡筛选 + 轴卡片区 + 右侧详情 |
| Motion.Axis 轴控制 | 单轴点动、回零、使能操作 |
| Motion.Actuator 执行器控制 | 手动操作气缸、真空、夹爪 |
| MotionConfig 系列 | 控制卡/轴/IO/参数/执行器配置页 |
| AlarmLog 系列 | 当前报警、报警历史、运行日志 |

### 4.3 远期规划

| 功能 | 说明 |
|------|------|
| Production 系列 | 工单、配方、数据、报表、追溯、MES |
| Vision 系列 | 相机监视、检测结果、标定管理 |
| PLC 系列 | 点位监视、寄存器、通讯状态、写入调试 |
| Peripheral 系列 | 扫码器、传感器监视与趋势 |
| SysConfig 系列 | 相机/PLC/传感器/扫码器/MES/运行配置 |
| Engineer 系列 | 设备诊断、原始参数调试 |

---

## 5. 已知问题

| 问题 | 状态 | 说明 |
|------|------|------|
| 所有业务页面为占位页 | 已知 | 当前使用 `CreatePlaceholderPage()` 显示文本占位 |
| 修改密码未实现 | 已知 | 当前为 MessageBox 占位 |
| 页面工厂注册不完整 | 已知 | 仅注册了部分 PageKey，其余显示"未实现页面" |

---

## 相关文档

- [AMControlWinF 项目总览](../02-development/winf-project-overview.md)
- [应用生命周期](../01-architecture/winf-application-lifecycle.md)
- [主题系统设计](../01-architecture/winf-theme-system.md)
- [导航系统设计](../01-architecture/winf-navigation-system.md)
- [用户认证与登录](../03-features/winf-auth-login.md)
- [主窗体壳层](../03-features/winf-mainwindow-shell.md)
