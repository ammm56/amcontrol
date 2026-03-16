# Copilot Instructions

## 项目指南
- 该项目为 .NET Framework 4.7.2 的 WPF 工业设备控制软件，采用 MVVM（CommunityToolkit.Mvvm）、HandyControl、SqlSugar，日志使用 NLog；配置来源为本地 Configuration/config.json（ConfigContext）与数据库并存；核心领域包含运动控制卡（固高/雷赛/虚拟）、IO 传感器、视觉与 PLC 通信。后续建议与改动需围绕此架构。
- 当前处于开发起步阶段，允许对现有代码进行大幅重构；优先保证运动控制架构思路和接口分层设计正确，再推进实现细节。
- 状态读取接口需要与脉冲/毫米位置语义一一对应：除脉冲位置方法外，还应提供规划毫米位置与编码器毫米位置方法，而不是仅单一 GetPositionMm。
- 确保全局配置（如 ConfigContext）在运行时同步可见，不接受各类中长期缓存私有副本导致与全局配置脱节。
- 运动控制对象应通过 MachineContext 作为全局唯一入口直接访问，避免各窗口持有私有副本导致急停/唯一控制失效。
- 运动控制接口从现在开始统一使用结果容器替代裸 short 返回值；同时结果需承载日志、报警与 UI 状态展示所需信息。
- 全项目统一使用单一 `Result/Result<T>` 返回模型，而不是按领域拆分 `MotionResult`、`PlcResult`、`VisualResult` 等多个结果类型。
- 报警持久化命名采用 IAlarmRecord、AlarmRecordService、AlarmRecord，并保持当前解决方案的全局基础设施注入架构；数据库实体统一使用 SqlSugar 标注风格；UI 消息与报警应优先走统一链路而不是各窗口各自定义监听实现。
- AxisConfig 采用强类型、统一且更清晰的业务命名，属性命名作为数据库轴参数名称标准，补齐软件限位、默认速度、回零流程兼容、多卡使能时序、急停/平停联动等参数。
- 用户选择 ConfigAxisArg 长期方案 A：数值统一 REAL + 类型标记，并希望通过服务实现将数据库参数覆盖到 MotionCardConfig.AxisConfigs。
- 保留 `ConfigAxisArgViewModel` 这一命名，因为它直接对应数据库表并表达直接操作数据库；同时对运行时配置类采用统一的、更通用的命名体系，而不是过窄地使用 `AxisRuntimeConfigViewModel`。

## 服务层设计
- 服务层统一采用“构造注入依赖 + 封装消息发布/日志通知”的风格，避免在各方法中重复直接调用 IMessageBus 和 IAMLogger。
- 用户希望统一抽取跨服务的日志、消息通知和警报处理到公共 ServiceBase，而不是在各 Service 中重复实现，以明确代码层级、简化框架并提升模块化。
- 在 AM.Core.Reporter 中引入 IAppReporter/AppReporter，并继续统一修改 ServiceBase、ViewModel、Tools 等非服务类的日志、消息通知与报警处理，同时完善错误码和错误描述映射设计，减少强制类型转换。