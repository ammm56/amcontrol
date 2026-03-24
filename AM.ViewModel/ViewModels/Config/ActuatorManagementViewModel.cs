using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.Entity.Motion.Actuator;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 执行器配置管理页面 ViewModel。
    /// 统一管理气缸、真空、灯塔、夹爪四类第三层对象的 CRUD。
    /// </summary>
    public class ActuatorManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ── CRUD 服务 ────────────────────────────────────────────────────

        private readonly MotionCylinderConfigCrudService _cylinderCrud = new MotionCylinderConfigCrudService();
        private readonly MotionVacuumConfigCrudService _vacuumCrud = new MotionVacuumConfigCrudService();
        private readonly MotionStackLightConfigCrudService _stackLightCrud = new MotionStackLightConfigCrudService();
        private readonly MotionGripperConfigCrudService _gripperCrud = new MotionGripperConfigCrudService();

        // ── 原始数据集合 ──────────────────────────────────────────────────

        private readonly ObservableCollection<CylinderConfigEntity> _cylinders = new ObservableCollection<CylinderConfigEntity>();
        private readonly ObservableCollection<VacuumConfigEntity> _vacuums = new ObservableCollection<VacuumConfigEntity>();
        private readonly ObservableCollection<StackLightConfigEntity> _stackLights = new ObservableCollection<StackLightConfigEntity>();
        private readonly ObservableCollection<GripperConfigEntity> _grippers = new ObservableCollection<GripperConfigEntity>();

        public ObservableCollection<CylinderConfigEntity> Cylinders { get { return _cylinders; } }
        public ObservableCollection<VacuumConfigEntity> Vacuums { get { return _vacuums; } }
        public ObservableCollection<StackLightConfigEntity> StackLights { get { return _stackLights; } }
        public ObservableCollection<GripperConfigEntity> Grippers { get { return _grippers; } }

        // ── 统一列表项（左侧显示） ────────────────────────────────────────

        private readonly ObservableCollection<ActuatorListItem> _allActuatorItems = new ObservableCollection<ActuatorListItem>();

        public ObservableCollection<ActuatorListItem> AllActuatorItems { get { return _allActuatorItems; } }

        public bool HasActuatorItems { get { return _allActuatorItems.Count > 0; } }

        // ── 选中项跟踪 ────────────────────────────────────────────────────

        private ActuatorListItem _selectedActuatorItem;

        public ActuatorListItem SelectedActuatorItem
        {
            get { return _selectedActuatorItem; }
            set
            {
                _selectedActuatorItem = value;
                OnPropertyChanged(nameof(SelectedActuatorItem));
                OnPropertyChanged(nameof(IsActuatorSelected));
            }
        }

        public bool IsActuatorSelected { get { return _selectedActuatorItem != null; } }

        // ── 加载方法 ──────────────────────────────────────────────────────

        public async Task LoadCylindersAsync()
        {
            var result = await Task.Run(() => _cylinderCrud.QueryAll());
            _cylinders.Clear();
            if (result.Success && result.Items != null)
                foreach (var item in result.Items)
                    _cylinders.Add(item);
        }

        public async Task LoadVacuumsAsync()
        {
            var result = await Task.Run(() => _vacuumCrud.QueryAll());
            _vacuums.Clear();
            if (result.Success && result.Items != null)
                foreach (var item in result.Items)
                    _vacuums.Add(item);
        }

        public async Task LoadStackLightsAsync()
        {
            var result = await Task.Run(() => _stackLightCrud.QueryAll());
            _stackLights.Clear();
            if (result.Success && result.Items != null)
                foreach (var item in result.Items)
                    _stackLights.Add(item);
        }

        public async Task LoadGrippersAsync()
        {
            var result = await Task.Run(() => _gripperCrud.QueryAll());
            _grippers.Clear();
            if (result.Success && result.Items != null)
                foreach (var item in result.Items)
                    _grippers.Add(item);
        }

        /// <summary>
        /// 重建统一列表（根据当前类型筛选）
        /// </summary>
        /// <summary>
        /// 根据类型筛选重建统一列表
        /// </summary>
        public void RebuildUnifiedList(string typeFilter)
        {
            _allActuatorItems.Clear();

            if (typeFilter == "All" || typeFilter == "Cylinder")
                foreach (var c in _cylinders)
                    _allActuatorItems.Add(new ActuatorListItem
                    {
                        ActuatorType = "Cylinder",
                        TypeDisplay = "气缸",
                        Name = c.Name,
                        DisplayName = c.DisplayName,
                        IsEnabled = c.IsEnabled,
                        SourceEntity = c
                    });

            if (typeFilter == "All" || typeFilter == "Vacuum")
                foreach (var v in _vacuums)
                    _allActuatorItems.Add(new ActuatorListItem
                    {
                        ActuatorType = "Vacuum",
                        TypeDisplay = "真空",
                        Name = v.Name,
                        DisplayName = v.DisplayName,
                        IsEnabled = v.IsEnabled,
                        SourceEntity = v
                    });

            if (typeFilter == "All" || typeFilter == "StackLight")
                foreach (var s in _stackLights)
                    _allActuatorItems.Add(new ActuatorListItem
                    {
                        ActuatorType = "StackLight",
                        TypeDisplay = "灯塔",
                        Name = s.Name,
                        DisplayName = s.DisplayName,
                        IsEnabled = s.IsEnabled,
                        SourceEntity = s
                    });

            if (typeFilter == "All" || typeFilter == "Gripper")
                foreach (var g in _grippers)
                    _allActuatorItems.Add(new ActuatorListItem
                    {
                        ActuatorType = "Gripper",
                        TypeDisplay = "夹爪",
                        Name = g.Name,
                        DisplayName = g.DisplayName,
                        IsEnabled = g.IsEnabled,
                        SourceEntity = g
                    });

            OnPropertyChanged(nameof(HasActuatorItems));
        }

        // ── 保存与删除 ────────────────────────────────────────────────────

        public Result SaveCylinder(CylinderConfigEntity entity) { return _cylinderCrud.Save(entity); }
        public Result DeleteCylinder(string name) { return _cylinderCrud.DeleteByName(name); }
        public Result SaveVacuum(VacuumConfigEntity entity) { return _vacuumCrud.Save(entity); }
        public Result DeleteVacuum(string name) { return _vacuumCrud.DeleteByName(name); }
        public Result SaveStackLight(StackLightConfigEntity entity) { return _stackLightCrud.Save(entity); }
        public Result DeleteStackLight(string name) { return _stackLightCrud.DeleteByName(name); }
        public Result SaveGripper(GripperConfigEntity entity) { return _gripperCrud.Save(entity); }
        public Result DeleteGripper(string name) { return _gripperCrud.DeleteByName(name); }
    }

    /// <summary>
    /// 执行器列表项统一包装（用于左侧列表显示）
    /// </summary>
    public class ActuatorListItem
    {
        /// <summary>
        /// 执行器类型：Cylinder / Vacuum / StackLight / Gripper
        /// </summary>
        public string ActuatorType { get; set; }

        /// <summary>
        /// 类型显示名称：气缸 / 真空 / 灯塔 / 夹爪
        /// </summary>
        public string TypeDisplay { get; set; }

        /// <summary>
        /// 执行器名称（唯一标识）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 原始实体对象（CylinderConfigEntity / VacuumConfigEntity 等）
        /// </summary>
        public object SourceEntity { get; set; }
    }
}