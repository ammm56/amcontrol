# PLC 协议库与 AM 上层重新分层设计草案

**文档编号**：ARCH-PLC-001  
**版本**：1.0.0  
**状态**：待落地  
**最后更新**：2026-04-10  
**维护人**：Am

---

## 1. 文档目的

本文档用于明确 PLC 协议库与 `AMControlWinF` 上层之间的最终分层边界，并给出下一阶段编码前的接口草案、模型草案、数据流与类关系说明。

本文档的核心目标是解决以下问题：

- 避免 `AM.DBService` 直接操作 `ModbusTCP`、`SiemensS7` 等协议底层客户端；
- 让协议地址解析、字符串长度、位索引、块读写等协议行为全部收敛到 `ProtocolLib` 内部；
- 保留 `AM.Model.Interfaces.Plc.IPlcClient` 作为系统统一入口，但只做薄门面，不承载协议实现；
- 支持“默认按点位读写、明确连续块时才走块读写”的运行策略；
- 为后续 `PlcScanWorker`、`PlcOperationService`、`PLC.Status`、`PLC.Monitor`、`PLC.Write` 页面落地提供统一接口基础。

---

## 2. 当前状态与设计决议

### 2.1 已有基础

当前解决方案已经具备以下基础：

- `AM.Model` 中已存在：
  - `IPlcClient`
  - `IPlcClientFactory`
  - `PlcConfig`
  - `PlcStationConfig`
  - `PlcPointConfig`
  - `PlcReadBlockConfig`
  - `PlcRawDataBlock`
  - `PlcRuntimeState`
- `AM.DBService` 中已存在：
  - `PlcConfigAppService`
  - `PlcRuntimeQueryService`
  - `PlcOperationService`
  - `PlcScanWorker`
  - `PlcClientFactory`
- 协议库已导入并通过基本测试：
  - `ProtocolLib.ModbusTcp`
  - `ProtocolLib.S7Tcp`

### 2.2 设计决议

本轮设计明确以下决议：

1. **协议读写行为必须在协议库中实现**  
   `AM.DBService` 不直接操作 `ModbusTCP`、`SiemensS7` 等底层客户端。

2. **AM 上层只通过协议库公开接口使用 PLC 能力**  
   上层不自行拼接 `40040[20]`、`40001.2`、`DB1.20[20]` 等协议表达式。

3. **保留 `IPlcClient`，但它只是 AM 的薄门面**  
   它负责生命周期管理、反射实例化协议实现、结果对象转换，不负责协议行为。

4. **默认按点位读写**  
   只有在明确连续地址、区域一致、类型一致且存在显式块配置时，才走块读写。

5. **结构化请求优先**  
   AM 上层传递 `AreaType / Address / DataType / BitIndex / StringLength / ArrayLength` 等结构化字段，由协议库内部解释为具体协议表达式。

---

## 3. 总体分层结构

### 3.1 分层边界

#### `ProtocolLib`

职责：

- 协议连接管理；
- 单点读写；
- 连续块读写；
- 地址解释；
- 类型解释；
- 字符串长度解释；
- 位索引解释；
- 协议结果模型返回。

#### `AM.DBService / AM.Core / AM.Model`

职责：

- 配置装配；
- 运行时调度；
- 点位与读块策略选择；
- 结果缓存到 `RuntimeContext.Instance.Plc`；
- 页面与业务服务调用。

### 3.2 类关系

```text
AM.DBService
  PlcConfigAppService
  PlcScanWorker
  PlcOperationService
  PlcClientFactory
  ProtocolPlcClient
        |
        v
ProtocolLib.CommonLib.Interface
  IProtocol
        |
        +---- ProtocolLib.ModbusTcp.Protocol
        |
        +---- ProtocolLib.S7Tcp.Protocol
```

### 3.3 数据流

#### 点位读取

```text
PlcOperationService
  -> IPlcClient.ReadPoint(...)
  -> ProtocolPlcClient
  -> IProtocol.ReadPoint(...)
  -> ModbusTcp.Protocol / S7Tcp.Protocol
  -> 返回统一结果
  -> RuntimeContext.Instance.Plc
```

#### 点位写入

```text
PlcOperationService
  -> IPlcClient.WritePoint(...)
  -> ProtocolPlcClient
  -> IProtocol.WritePoint(...)
  -> 协议库内部完成地址与类型处理
  -> 返回结果
```

#### 块读取

```text
PlcScanWorker
  -> IPlcClient.ReadBlock(...)
  -> IProtocol.ReadBlock(...)
  -> 协议库返回 M_BlockData
  -> AM 解析块中点位
  -> 更新 RuntimeContext.Instance.Plc
```

---

## 4. 协议库层最终接口草案

### 4.1 `IProtocol` 最终职责

`IProtocol` 从当前的“初始化 + 单点 Get/Set”升级为：

- 生命周期统一接口；
- 单点读写统一接口；
- 块读写统一接口；
- 结构化请求模型；
- 结构化返回模型。

### 4.2 `IProtocol` 最终方法签名设计表

| 分类 | 方法 | 说明 |
|---|---|---|
| 生命周期 | `M_Return<bool> Configure(M_ProtocolOptions options)` | 设置协议连接参数 |
| 生命周期 | `M_Return<bool> Connect()` | 建立连接 |
| 生命周期 | `M_Return<bool> Disconnect()` | 断开连接 |
| 生命周期 | `M_Return<bool> Reconnect()` | 重连 |
| 生命周期 | `M_Return<bool> IsConnected()` | 查询连接状态 |
| 单点读取 | `M_Return<M_PointData> ReadPoint(M_PointReadRequest request)` | 单点读取 |
| 单点写入 | `M_Return<M_PointData> WritePoint(M_PointWriteRequest request)` | 单点写入 |
| 块读取 | `M_Return<M_BlockData> ReadBlock(M_BlockReadRequest request)` | 连续块读取 |
| 块写入 | `M_Return<M_BlockData> WriteBlock(M_BlockWriteRequest request)` | 连续块写入 |

### 4.3 `IProtocol` 最终代码草案

```csharp
using ProtocolLib.CommonLib.Model;

namespace ProtocolLib.CommonLib.Interface
{
    /// <summary>
    /// 协议库统一接口。
    /// 协议差异应在协议库内部完成屏蔽，AM 上层只依赖该接口。
    /// </summary>
    public interface IProtocol
    {
        /// <summary>
        /// 配置协议连接参数。
        /// </summary>
        M_Return<bool> Configure(M_ProtocolOptions options);

        /// <summary>
        /// 建立连接。
        /// </summary>
        M_Return<bool> Connect();

        /// <summary>
        /// 断开连接。
        /// </summary>
        M_Return<bool> Disconnect();

        /// <summary>
        /// 重连。
        /// </summary>
        M_Return<bool> Reconnect();

        /// <summary>
        /// 查询当前连接状态。
        /// </summary>
        M_Return<bool> IsConnected();

        /// <summary>
        /// 按点位读取。
        /// </summary>
        M_Return<M_PointData> ReadPoint(M_PointReadRequest request);

        /// <summary>
        /// 按点位写入。
        /// </summary>
        M_Return<M_PointData> WritePoint(M_PointWriteRequest request);

        /// <summary>
        /// 按连续地址块读取。
        /// </summary>
        M_Return<M_BlockData> ReadBlock(M_BlockReadRequest request);

        /// <summary>
        /// 按连续地址块写入。
        /// </summary>
        M_Return<M_BlockData> WriteBlock(M_BlockWriteRequest request);
    }
}
```

### 4.4 旧接口处理原则

当前已有：

- `Init()`
- `Get(...)`
- `Set(...)`
- `SetCFG(...)`
- `Reconnect()`
- `CloseConnected()`

这些接口建议视为**过渡接口**：

- 短期内可在协议库内部保留兼容；
- 中期统一迁移到 `Configure / Connect / Disconnect / ReadPoint / WritePoint / ReadBlock / WriteBlock`；
- 新增实现与新测试均以新接口为准。

---

## 5. 协议库模型草案

### 5.1 `M_ProtocolOptions`

用途：协议连接配置模型。

```csharp
namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 协议连接配置参数。
    /// </summary>
    public class M_ProtocolOptions
    {
        public string protocolType { get; set; } = string.Empty;
        public string connectionType { get; set; } = string.Empty;
        public string ip { get; set; } = string.Empty;
        public int port { get; set; }
        public short? stationNo { get; set; }
        public short? rack { get; set; }
        public short? slot { get; set; }
        public int timeoutMs { get; set; }
        public string byteOrder { get; set; } = string.Empty;
        public string wordOrder { get; set; } = string.Empty;
        public string stringEncoding { get; set; } = string.Empty;
    }
}
```

### 5.2 `M_PointReadRequest`

```csharp
namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位读取请求。
    /// </summary>
    public class M_PointReadRequest
    {
        public string areaType { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string dataType { get; set; } = string.Empty;
        public short? bitIndex { get; set; }
        public int stringLength { get; set; }
        public int arrayLength { get; set; }
    }
}
```

### 5.3 `M_PointWriteRequest`

```csharp
namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位写入请求。
    /// </summary>
    public class M_PointWriteRequest
    {
        public string areaType { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string dataType { get; set; } = string.Empty;
        public object value { get; set; }
        public short? bitIndex { get; set; }
        public int stringLength { get; set; }
        public int arrayLength { get; set; }
    }
}
```

### 5.4 `M_BlockReadRequest`

```csharp
namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 连续地址块读取请求。
    /// </summary>
    public class M_BlockReadRequest
    {
        public string areaType { get; set; } = string.Empty;
        public string startAddress { get; set; } = string.Empty;
        public int length { get; set; }
        public string dataType { get; set; } = string.Empty;
        public int stringLength { get; set; }
        public int arrayLength { get; set; }
    }
}
```

### 5.5 `M_BlockWriteRequest`

```csharp
namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 连续地址块写入请求。
    /// </summary>
    public class M_BlockWriteRequest
    {
        public string areaType { get; set; } = string.Empty;
        public string startAddress { get; set; } = string.Empty;
        public string dataType { get; set; } = string.Empty;
        public byte[] buffer { get; set; } = new byte[0];
        public int stringLength { get; set; }
        public int arrayLength { get; set; }
    }
}
```

### 5.6 `M_PointData`

```csharp
namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 点位读写结果。
    /// </summary>
    public class M_PointData
    {
        public string areaType { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string dataType { get; set; } = string.Empty;
        public string value { get; set; } = string.Empty;
        public byte[] rawBuffer { get; set; } = new byte[0];
        public string quality { get; set; } = string.Empty;
    }
}
```

### 5.7 `M_BlockData`

```csharp
namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 连续地址块读写结果。
    /// </summary>
    public class M_BlockData
    {
        public string areaType { get; set; } = string.Empty;
        public string startAddress { get; set; } = string.Empty;
        public int length { get; set; }
        public string dataType { get; set; } = string.Empty;
        public byte[] buffer { get; set; } = new byte[0];
        public string valueText { get; set; } = string.Empty;
    }
}
```

---

## 6. AM 上层 `IPlcClient` 最终草案

### 6.1 设计原则

`IPlcClient` 保留，但重新定位为：

- `AM` 上层统一门面；
- 反射创建协议库实现；
- `Result/Result<T>` 与 `M_Return<T>` 转换；
- 生命周期管理；
- 不承载协议读写实现。

### 6.2 `IPlcClient` 最终方法签名设计表

| 分类 | 方法 | 说明 |
|---|---|---|
| 生命周期 | `Result Configure(PlcProtocolClientOptions options)` | 配置客户端 |
| 生命周期 | `Result Connect()` | 建立连接 |
| 生命周期 | `Result Disconnect()` | 断开连接 |
| 生命周期 | `Result Reconnect()` | 重连 |
| 生命周期 | `Result<bool> IsConnected()` | 查询连接状态 |
| 单点读取 | `Result<PlcPointReadResult> ReadPoint(PlcPointReadRequest request)` | 单点读取 |
| 单点写入 | `Result<PlcPointReadResult> WritePoint(PlcPointWriteRequest request)` | 单点写入 |
| 块读取 | `Result<PlcRawDataBlock> ReadBlock(PlcBlockReadRequest request)` | 块读取 |
| 块写入 | `Result<PlcRawDataBlock> WriteBlock(PlcBlockWriteRequest request)` | 块写入 |

### 6.3 `IPlcClient` 最终代码草案

```csharp
using AM.Model.Common;
using AM.Model.Plc;

namespace AM.Model.Interfaces.Plc
{
    /// <summary>
    /// AM 侧统一 PLC 客户端门面。
    /// 负责统一上层调用入口，不负责协议行为实现。
    /// </summary>
    public interface IPlcClient
    {
        string PlcName { get; }

        string ProtocolType { get; }

        string ConnectionType { get; }

        Result Configure(PlcProtocolClientOptions options);

        Result Connect();

        Result Disconnect();

        Result Reconnect();

        Result<bool> IsConnected();

        Result<PlcPointReadResult> ReadPoint(PlcPointReadRequest request);

        Result<PlcPointReadResult> WritePoint(PlcPointWriteRequest request);

        Result<PlcRawDataBlock> ReadBlock(PlcBlockReadRequest request);

        Result<PlcRawDataBlock> WriteBlock(PlcBlockWriteRequest request);
    }
}
```

---

## 7. AM 侧模型草案

### 7.1 `PlcProtocolClientOptions`

```csharp
namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧 PLC 客户端配置参数。
    /// 用于将运行时站配置转换成客户端可消费的结构化参数。
    /// </summary>
    public class PlcProtocolClientOptions
    {
        public string PlcName { get; set; } = string.Empty;
        public string ProtocolType { get; set; } = string.Empty;
        public string ConnectionType { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public int Port { get; set; }
        public short? StationNo { get; set; }
        public short? Rack { get; set; }
        public short? Slot { get; set; }
        public int TimeoutMs { get; set; }
        public string ByteOrder { get; set; } = string.Empty;
        public string WordOrder { get; set; } = string.Empty;
        public string StringEncoding { get; set; } = string.Empty;
    }
}
```

### 7.2 `PlcPointReadRequest`

```csharp
namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧点位读取请求。
    /// </summary>
    public class PlcPointReadRequest
    {
        public string PlcName { get; set; } = string.Empty;
        public string AreaType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public short? BitIndex { get; set; }
        public int StringLength { get; set; }
        public int ArrayLength { get; set; }
    }
}
```

### 7.3 `PlcPointWriteRequest`

```csharp
namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧点位写入请求。
    /// </summary>
    public class PlcPointWriteRequest
    {
        public string PlcName { get; set; } = string.Empty;
        public string AreaType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public object Value { get; set; }
        public short? BitIndex { get; set; }
        public int StringLength { get; set; }
        public int ArrayLength { get; set; }
    }
}
```

### 7.4 `PlcBlockReadRequest`

```csharp
namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧块读取请求。
    /// </summary>
    public class PlcBlockReadRequest
    {
        public string PlcName { get; set; } = string.Empty;
        public string AreaType { get; set; } = string.Empty;
        public string StartAddress { get; set; } = string.Empty;
        public int Length { get; set; }
        public string DataType { get; set; } = string.Empty;
        public int StringLength { get; set; }
        public int ArrayLength { get; set; }
    }
}
```

### 7.5 `PlcBlockWriteRequest`

```csharp
namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧块写入请求。
    /// </summary>
    public class PlcBlockWriteRequest
    {
        public string PlcName { get; set; } = string.Empty;
        public string AreaType { get; set; } = string.Empty;
        public string StartAddress { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public byte[] Buffer { get; set; } = new byte[0];
        public int StringLength { get; set; }
        public int ArrayLength { get; set; }
    }
}
```

### 7.6 `PlcPointReadResult`

```csharp
namespace AM.Model.Plc
{
    /// <summary>
    /// AM 侧点位读写结果。
    /// </summary>
    public class PlcPointReadResult
    {
        public string PlcName { get; set; } = string.Empty;
        public string AreaType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string ValueText { get; set; } = string.Empty;
        public byte[] RawBuffer { get; set; } = new byte[0];
    }
}
```

---

## 8. 结构化字段与协议表达式的职责边界

### 8.1 AM 上层负责提供的字段

- `AreaType`
- `Address`
- `DataType`
- `BitIndex`
- `StringLength`
- `ArrayLength`

### 8.2 协议库内部负责解释的内容

#### Modbus

- `HoldingRegister / Coil / InputRegister / DiscreteInput`
- `40040[20]`
- `40001.2`
- 字符串长度到寄存器长度换算
- 位索引写入

#### S7

- `DB / M / I / Q / V / T / C`
- `DB1.20[20]`
- `DB1.0.2`
- 固定长度字符串地址解释
- 后续标准 `STRING` / `WSTRING` 扩展

### 8.3 决策

`40040[20]` 这类表达式是**协议内部表达式**，不是 AM 上层配置表达式。

上层配置中应保存：

- `Address = 40040`
- `StringLength = 20`

由协议库在内部组合成实际协议地址。

---

## 9. 运行策略

### 9.1 默认点位读写

默认业务路径：

- `PlcOperationService.WritePoint(...)`
- `PlcOperationService.TestReadPoint(...)`
- `PlcScanWorker` 对未落块的点位读取

都优先使用：

- `ReadPoint`
- `WritePoint`

### 9.2 块读写仅在以下场景使用

- 存在显式 `PlcReadBlockConfig`
- 明确连续地址
- 区域一致
- 类型一致或块解释方式一致
- 扫描性能优化需要

### 9.3 原则

- 块读取不能替代点位读取；
- 块读取是扫描优化手段，不是上层默认编程模型；
- 点位写入始终优先使用单点写入。

---

## 10. 推荐实施顺序

### 第一阶段：只落接口与模型

1. 新增协议库请求/结果模型；
2. 扩展 `ProtocolLib.CommonLib.Interface.IProtocol`；
3. 保留旧接口过渡，不立即删除。

### 第二阶段：补协议库实现

1. `ProtocolLib.ModbusTcp.Protocol`
   - `Configure`
   - `Connect`
   - `Disconnect`
   - `IsConnected`
   - `ReadPoint`
   - `WritePoint`
   - `ReadBlock`
   - `WriteBlock`
2. `ProtocolLib.S7Tcp.Protocol`
   - 同步实现同名接口

### 第三阶段：改 AM 上层

1. `IPlcClient` 调整为门面接口；
2. `PlcClientFactory` 反射创建协议实例；
3. `ProtocolPlcClient` 负责 `Result <-> M_Return` 转换；
4. `PlcScanWorker` 与 `PlcOperationService` 切换到新接口。

### 第四阶段：补测试

1. 协议库接口测试；
2. `IPlcClient` 门面测试；
3. 运行时扫描与操作服务测试。

---

## 11. 与主规划文档的关系

本文件是协议接口与分层的细化架构文档。  
总体模块规划、目录落位、页面职责与实施顺序仍以：

- `Docs/03-features/plc-communication-planning.md`

为主；协议接口、请求模型、返回模型与分层边界的详细定义以本文为准。
