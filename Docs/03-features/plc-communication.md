# PLC 通信模块实现说明

**文档编号**：FEAT-PLC-002  
**版本**：1.0.0  
**状态**：后端已实现  
**最后更新**：2026-05-09  
**维护人**：Am

---

## 1. 模块定位

PLC 通信模块负责在 AMControlWinF 中连接、采样、查询与手动操作 PLC 设备数据，覆盖：

- 配置管理（站与点位的增删改查及运行时装配）
- 后台周期扫描（运行时状态缓存写入）
- 运行时快照查询（供 UI 页面消费）
- 手动读写操作（工程调试页使用）

---

## 2. 文件目录结构

```
AM.Model/
  Entity/Plc/
    PlcStationConfigEntity.cs    数据库实体（plc_station 表）
    PlcPointConfigEntity.cs      数据库实体（plc_point 表）
  Plc/
    PlcConfig.cs                 配置聚合对象
    PlcStationConfig.cs          站运行时配置
    PlcPointConfig.cs            点位运行时配置
  Interfaces/Plc/
    IPlcClient.cs                AM 侧客户端门面接口
    IPlcClientFactory.cs         客户端工厂接口
    App/
      IPlcConfigAppService.cs    配置应用服务接口
    Config/
      IPlcStationCrudService.cs  站 CRUD 接口
      IPlcPointCrudService.cs    点位 CRUD 接口
    Runtime/
      IPlcRuntimeService.cs      运行时服务接口
      IPlcScanWorker.cs          扫描工作单元接口
  Runtime/
    PlcStationRuntimeSnapshot.cs 站运行时快照
    PlcPointRuntimeSnapshot.cs   点位运行时快照

AM.DBService/Services/Plc/
  Config/
    PlcStationCrudService.cs
    PlcPointCrudService.cs
  Driver/
    ProtocolAssemblyRegistry.cs  协议 DLL 插件扫描与注册
    ProtocolPlcClient.cs         AM 侧门面客户端实现
    NullPlcClient.cs             占位客户端
    PlcClientFactory.cs          工厂实现
  App/
    PlcConfigAppService.cs       DB → Context 装配
    PlcConfigSeedService.cs      默认种子数据
  Runtime/
    PlcScanWorker.cs             后台扫描工作单元
    PlcRuntimeQueryService.cs    快照查询服务
    PlcOperationService.cs       手动读写服务

ProtocolLib/
  Common/CommonLib/
    Interface/IProtocol.cs       协议统一接口
    Model/
      M_ProtocolOptions.cs
      M_PointReadRequest.cs
      M_PointWriteRequest.cs
      M_PointData.cs
      M_Return.cs
  Modbus/ModbusTcp/Protocol.cs   Modbus TCP 实现
  Siemens/S7Tcp/Protocol.cs      Siemens S7 实现

AM.Tests/Protocols/
  ModbusTcpProtocolTests.cs      ModbusTcp 集成测试
```

---

## 3. 数据模型设计原则

当前 PLC 模块采用极简模型，以下设计原则已确定：

| 原则 | 说明 |
|------|------|
| Address 直接表达完整协议地址 | 不拆分 AreaType / BitIndex 等字段 |
| DataType 统一字符串名称 | bool/uint8/int8/uint16/int16/uint32/int32/uint64/int64/float/double/string，不使用枚举 |
| Length 统一长度 | 标量=1，字符串=字符长度，数组=元素个数；不拆分 StringLength / ArrayLength |
| 无块读写 | 当前版本仅按点位读写，无 BlockRead / BlockWrite |
| 无 PlcName 字段在点位请求中 | 点位配置本身已含 PlcName，读写请求通过配置查找路由 |

---

## 4. 关键类说明

### 4.1 ProtocolPlcClient

`AM.DBService.Services.Plc.Driver.ProtocolPlcClient` 是 AM 侧 PLC 客户端门面实现，核心逻辑：

```csharp
// Configure() 时按 protocolType 解析协议实现类
ProtocolAssemblyRegistry.TryResolve(options.protocolType, out Type implType);
_protocol = (IProtocol)Activator.CreateInstance(implType);
_protocol.Configure(options);

// 所有读写直接转发到协议库
public Result<M_PointData> ReadPoint(M_PointReadRequest request)
    => ToResult(_protocol.ReadPoint(request));
```

### 4.2 PlcScanWorker

关键设计点：

- **配置缓存引用级比较**：`object.ReferenceEquals(_cachedPlcConfig, plcConfig)` 避免每轮重建分组
- **按站控制扫描频率**：每个站独立维护 `LastScanTime`，按各站 `ScanIntervalMs` 决定是否扫描
- **自动重连**：每站独立维护 `NextReconnectTime`，避免高频重连冲击
- **站快照构建**：包含成功/失败读取计数、平均读取耗时、连接时间等统计信息

### 4.3 PlcRuntimeQueryService

快照合并策略（`MergeStationSnapshot` / `MergePointSnapshot`）：

- 以运行时快照为主体（IsConnected / 时间戳 / 统计信息）
- 配置侧补全 DisplayName / IsEnabled / CurrentProtocol 等元数据字段
- 若运行时快照不存在，返回基于配置构建的默认快照（IsConnected=false，Quality="Unknown"/"Stale"）

### 4.4 PlcConfigAppService.ReloadFromDatabase()

执行流程：

```
1. EnsureTables()                      // CodeFirst 建表
2. QueryAll()                          // 读取全部配置
3. ConfigContext.Config.PlcConfig = plcConfig  // 注入 ConfigContext
4. 遍历启用站：
   PlcClientFactory.Create(station)    // 创建 ProtocolPlcClient 或 NullPlcClient
   MachineContext.Plcs[name] = client  // 注册到 MachineContext
5. 遍历启用站：连接
   client.Connect()                    // 建立实际协议连接
```

---

## 5. 地址格式参考

| 协议 | 类型 | 地址示例 |
|------|------|----------|
| Modbus TCP | 线圈（bool） | `00001` |
| Modbus TCP | 保持寄存器（uint16） | `40001` |
| Modbus TCP | 保持寄存器（float） | `40030` |
| Modbus TCP | 字符串（20字节） | `40040` + length=20，或 `40040[20]` |
| Siemens S7 | DB 数据块 bool | `DB1.0` |
| Siemens S7 | DB 数据块 int16 | `DB1.2` |
| Siemens S7 | DB 数据块字符串 | `DB1.20` + length=20 |
| Siemens S7 | M 区 float | `MD10` |

---

## 6. 集成测试

`AM.Tests/Protocols/ModbusTcpProtocolTests.cs` 覆盖：

- 协议实例化（通过 `IProtocol` 接口操作，不直接依赖具体类）
- Configure + Connect + Disconnect
- 各基础数据类型读写（bool / uint16 / int16 / uint32 / int32 / float / double / string）
- 字符串固定长度方案（`[20]` 后缀）
- 重连测试

---

## 7. 下一步（UI 页面开发参考）

### 7.1 PLC.Monitor 站状态总览

- 数据来源：`PlcRuntimeQueryService.QueryAllStations()`
- 刷新方式：订阅 `RuntimeContext.Instance.Plc.SnapshotChanged`，~500ms 低频采样
- 显示内容：站名/协议/连接状态/最近扫描时间/错误信息/读取统计

### 7.2 PLC.Register 点位实时监视

- 数据来源：`PlcRuntimeQueryService.QueryAllPoints()`
- 刷新方式：同上 SnapshotChanged 事件
- 按 PlcName + GroupName 分组显示
- 列：点位名/地址/类型/当前值/质量/更新时间

### 7.3 PLC.Write 手动写入

- 服务：`PlcOperationService.WriteRaw(plcName, address, dataType, value, length)`
- 权限：Engineer / Am 角色
- 交互：选择站 → 输入地址/类型/值 → 确认写入 → 显示写入结果

### 7.4 SysConfig.Plc 配置管理

- 站列表：`IPlcStationCrudService.QueryAll()`，增删改保存后调用 `PlcConfigAppService.ReloadFromDatabase()`
- 点位列表：`IPlcPointCrudService.QueryByPlcName(plcName)`，关联所选站
- 重载配置：配置变更后调用 `ReloadFromDatabase()` 重建 MachineContext.Plcs

---

## 相关文档

- [PLC 协议库架构设计](../01-architecture/plc-protocol-integration-design.md)
- [数据库表结构](../09-database-config/README.md)
- [开发进展记录](../07-release-notes/winf-development-progress.md)
