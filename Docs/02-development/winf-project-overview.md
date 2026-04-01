# AMControlWinF 项目总览

**文档编号**：DEV-W001  
**版本**：1.0.0  
**状态**：有效  
**最后更新**：2026-03-28  
**维护人**：Am

---

## 1. 项目定位

`AMControlWinF` 是 `ammm56/amcontrol` 仓库 `winform461` 分支上的 WinForms 工业设备控制软件。与 WPF 主控分支（`AMControlWPF`）共享核心层（`AM.Core`、`AM.Model`、`AM.DBService`、`AM.MotionService`），但 UI 层基于 **AntdUI** WinForms 框架独立实现，面向 .NET Framework 4.6.1 运行环境。

---

## 2. 技术栈

| 层级 | 技术 / 框架 | 说明 |
|------|------------|------|
| UI 框架 | AntdUI (WinForms) | 提供 GridPanel、Panel(Shadow/Radius)、Menu、Popover、Avatar 等现代化控件 |
| 目标框架 | .NET Framework 4.6.1 | C# 7.3 |
| 数据库 ORM | SqlSugar | 支持 SQLite / Access / MySQL |
| 日志 | NLog | 统一日志输出 |
| MVVM 基础 | AM.PageModel（手动 BindableBase） | ViewModel 属性采用手动字段 + 属性方式，不使用源生成器特性 |
| 配置 | ConfigContext（本地 config.json）+ 数据库 | 全局配置唯一来源 |
| 认证 | AM.DBService.Services.Auth | 用户 / 角色 / 页面级权限 |

---

## 3. 解决方案结构

```
amcontrol_winform.sln
├── AMControlWinF/          # WinForms 主程序（UI 壳层、视图、工具类）
├── AM.PageModel/           # WPF/WinForms 共享页面模型（导航、登录、主窗体模型）
├── AM.App/                 # 应用引导层（AppBootstrap 全局初始化）
├── AM.Core/                # 核心上下文（ConfigContext、UserContext、SystemContext、MachineContext）
├── AM.Model/               # 数据模型与接口定义（Entity、Interfaces）
├── AM.DBService/           # 数据库服务（Auth、Motion 配置等 CRUD）
├── AM.MotionService/       # 运动控制服务（固高/雷赛/虚拟卡驱动接入）
└── AM.Tools/               # 通用工具（配置文件读写、辅助方法）
```

---

## 4. AMControlWinF 项目文件结构

```
AMControlWinF/
├── Program.cs                              # 程序入口：while 主循环 + 登录/主窗体生命周期
├── MainWindow.cs / .Designer.cs / .resx    # 主窗体壳层
├── Tools/
│   └── AppThemeHelper.cs                   # 全局主题辅助（明/暗切换）
├── Views/
│   ├── Auth/
│   │   └── LoginForm.cs / .Designer.cs     # 登录窗体
│   └── Main/
│       ├── TextureBackgroundControl.cs      # 自定义纹理背景控件
│       ├── UserAvatarMenuControl.cs         # 用户头像菜单（嵌入主窗体标题栏）
│       └── UserAvatarPopoverCard.cs         # 用户操作弹出卡片（切换用户/修改密码/退出）
└── Properties/
    └── Resources / Settings / AssemblyInfo
```

---

## 5. AM.PageModel 项目文件结构

```
AM.PageModel/
├── Common/
│   └── BindableBase.cs                     # INotifyPropertyChanged 基类
├── Navigation/
│   ├── NavigationCatalog.cs                # 全局页面目录（导航 + 权限唯一来源）
│   ├── NavPrimaryDef.cs                    # 一级导航定义
│   └── NavPageDef.cs                       # 二级页面定义
├── Auth/
│   └── LoginPageModel.cs                   # 登录页面模型
├── Main/
│   ├── MainWindowModel.cs                  # 主窗体模型（导航状态、用户信息）
│   └── MainWindowExitReason.cs             # 退出原因枚举（Exit / SwitchUser / Logout）
└── Am/
    └── UserManagementPageModel.cs          # 用户管理页面模型
```

---

## 6. 核心架构约束

| 约束 | 说明 |
|------|------|
| 全局配置唯一来源 | `ConfigContext.Instance` 读写所有配置，不允许各处维护私有副本 |
| 全局设备唯一入口 | `MachineContext` 管理运动控制卡、轴、IO、执行器对象 |
| 统一返回模型 | 全项目统一使用 `Result/Result<T>` 返回模型 |
| 页面级权限 | 权限从数据库读取，`NavigationCatalog` 是权限目录唯一来源 |
| 角色体系 | `Operator`（操作员）、`Engineer`（工程师）、`Am`（管理员） |
| 默认管理员 | 登录名 `am`，初始密码 `am123` |

---

## 7. 分支说明

| 分支 | 说明 |
|------|------|
| `winform461` | AMControlWinF WinForms 分支，目标 .NET Framework 4.6.1 |
| `main` / 其他 | AMControlWPF WPF 分支，目标 .NET Framework 4.7.2 |

两个分支共享 `AM.Core`、`AM.Model`、`AM.DBService`、`AM.MotionService`、`AM.Tools` 等核心项目。`AM.PageModel` 作为公共页面模型中间层，在两个分支中分别维护。

---

## 相关文档

- [应用生命周期与登录流程](../01-architecture/winf-application-lifecycle.md)
- [主题系统设计](../01-architecture/winf-theme-system.md)
- [导航系统设计](../01-architecture/winf-navigation-system.md)
- [用户认证与登录](../03-features/winf-auth-login.md)
- [主窗体壳层](../03-features/winf-mainwindow-shell.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)
