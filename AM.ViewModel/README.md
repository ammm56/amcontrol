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
















