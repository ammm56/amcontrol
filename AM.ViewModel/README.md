AM.ViewModel (类库项目)
├── 📂 Base                  // 存放 ViewModel 基类和公共逻辑
│   ├── ViewModelBase.cs    // 继承自 ObservableObject
│   └── PageViewModelBase.cs // 专门给 Page 使用的基类
├── 📂 ViewModels            // 核心功能 ViewModel 存放处（按模块分文件夹）
│   ├── 📂 Main             // 主窗体相关
│   │   └── MainViewModel.cs
│   ├── 📂 Config           // 参数配置模块
│   │   ├── ConfigViewModel.cs     // 配置主页面
│   │   └── AxisConfigViewModel.cs // 轴参数包装器（包装 AM.Model.AxisConfig）
│   ├── 📂 Debug            // 调试/手动操作模块
│   │   └── DebugViewModel.cs
│   └── 📂 Monitor          // 运行监控模块
│       └── MonitorViewModel.cs
├── 📂 Common				// (可选) 如果有复杂的跨模块自定义
├── 📂 Messages              // 存放 CommunityToolkit.Mvvm 的 Messenger 消息类
│   └── LogMessage.cs       // 用于跨页面发送日志通知
├── 📂 Services              // 存放逻辑层的单例或助手（非硬件驱动）
│   └── NavigationService.cs // 处理 SideMenu 导航逻辑
└── 📂 Converters            // (可选) 逻辑层相关的转换逻辑


| 类型                        | 优点                                           | 缺点                                              |
| ------------------------- | -------------------------------------------- | ----------------------------------------------- |
| `List<T>`                 | 内存占用小，普通存储数据                                 | **不支持 UI 自动刷新**，修改列表不会通知 DataGrid 或 ListView 更新 |
| `ObservableCollection<T>` | 内置 **INotifyCollectionChanged**，支持 UI 绑定自动刷新 | 相比 List 略慢，但工业场景完全可以接受                          |
DataGrid / ListView / ComboBox 等控件绑定 必须能自动更新
当保存、删除、刷新数据时，UI 会立即反映变化
如果用 List<T>，必须手动触发整个控件刷新或重绑定，代码冗长且容易出错
结论：工业 WPF 中，所有绑定 UI 的列表都用 ObservableCollection<T>。

ViewModel中方法用 AsyncRelayCommand 包裹，而不直接在构造函数访问
设计意图
异步执行，防止阻塞 UI
数据库操作可能耗时（IO、磁盘、网络）
如果在构造函数里直接 QueryAll() → UI线程会阻塞 → 界面卡顿或无响应
可控执行时机
构造函数只初始化 ViewModel，不直接执行操作
UI 或其他逻辑调用 LoadCommand.ExecuteAsync() 时才执行加载
支持 延迟加载 / 按需刷新
统一命令绑定
WPF Command 可以直接绑定按钮
AsyncRelayCommand 封装了 异步 + CanExecute + 异常处理
UI 可以禁用按钮，避免重复点击
              |









