# Copilot Instructions

## 项目指南
- 该项目为 .NET Framework 4.7.2 的 WPF 工业设备控制软件，采用 MVVM（CommunityToolkit.Mvvm）、HandyControl、SqlSugar，日志使用 NLog；配置来源为本地 Configuration/config.json（ConfigContext）与数据库并存；核心领域包含运动控制卡（固高/雷赛/虚拟）、IO 传感器、视觉与 PLC 通信。后续建议与改动需围绕此架构。
- 当前处于开发起步阶段，允许对现有代码进行大幅重构；优先保证运动控制架构思路和接口分层设计正确，再推进实现细节。
- 状态读取接口需要与脉冲/毫米位置语义一一对应：除脉冲位置方法外，还应提供规划毫米位置与编码器毫米位置方法，而不是仅单一 GetPositionMm。