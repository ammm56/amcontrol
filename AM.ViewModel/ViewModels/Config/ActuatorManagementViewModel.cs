using AM.DBService.Services.Motion.Actuator;
using AM.Model.Common;
using AM.Model.Entity.Motion.Actuator;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 执行器配置管理页面 ViewModel。
    /// 统一管理气缸、真空、灯塔、夹爪四类第三层对象的 CRUD。
    /// </summary>
    public class ActuatorManagementViewModel
    {
        // ── CRUD 服务 ────────────────────────────────────────────────────

        private readonly MotionCylinderConfigCrudService _cylinderCrud = new MotionCylinderConfigCrudService();
        private readonly MotionVacuumConfigCrudService _vacuumCrud = new MotionVacuumConfigCrudService();
        private readonly MotionStackLightConfigCrudService _stackLightCrud = new MotionStackLightConfigCrudService();
        private readonly MotionGripperConfigCrudService _gripperCrud = new MotionGripperConfigCrudService();

        // ── 气缸 ─────────────────────────────────────────────────────────

        private readonly ObservableCollection<CylinderConfigEntity> _cylinders
            = new ObservableCollection<CylinderConfigEntity>();

        public ObservableCollection<CylinderConfigEntity> Cylinders { get { return _cylinders; } }

        public async Task LoadCylindersAsync()
        {
            var result = await Task.Run(() => _cylinderCrud.QueryAll());
            _cylinders.Clear();
            if (result.Success && result.Items != null)
            {
                foreach (var item in result.Items)
                    _cylinders.Add(item);
            }
        }

        public Result SaveCylinder(CylinderConfigEntity entity)
        {
            return _cylinderCrud.Save(entity);
        }

        public Result DeleteCylinder(string name)
        {
            return _cylinderCrud.DeleteByName(name);
        }

        // ── 真空 ─────────────────────────────────────────────────────────

        private readonly ObservableCollection<VacuumConfigEntity> _vacuums
            = new ObservableCollection<VacuumConfigEntity>();

        public ObservableCollection<VacuumConfigEntity> Vacuums { get { return _vacuums; } }

        public async Task LoadVacuumsAsync()
        {
            var result = await Task.Run(() => _vacuumCrud.QueryAll());
            _vacuums.Clear();
            if (result.Success && result.Items != null)
            {
                foreach (var item in result.Items)
                    _vacuums.Add(item);
            }
        }

        public Result SaveVacuum(VacuumConfigEntity entity)
        {
            return _vacuumCrud.Save(entity);
        }

        public Result DeleteVacuum(string name)
        {
            return _vacuumCrud.DeleteByName(name);
        }

        // ── 灯塔 ─────────────────────────────────────────────────────────

        private readonly ObservableCollection<StackLightConfigEntity> _stackLights
            = new ObservableCollection<StackLightConfigEntity>();

        public ObservableCollection<StackLightConfigEntity> StackLights { get { return _stackLights; } }

        public async Task LoadStackLightsAsync()
        {
            var result = await Task.Run(() => _stackLightCrud.QueryAll());
            _stackLights.Clear();
            if (result.Success && result.Items != null)
            {
                foreach (var item in result.Items)
                    _stackLights.Add(item);
            }
        }

        public Result SaveStackLight(StackLightConfigEntity entity)
        {
            return _stackLightCrud.Save(entity);
        }

        public Result DeleteStackLight(string name)
        {
            return _stackLightCrud.DeleteByName(name);
        }

        // ── 夹爪 ─────────────────────────────────────────────────────────

        private readonly ObservableCollection<GripperConfigEntity> _grippers
            = new ObservableCollection<GripperConfigEntity>();

        public ObservableCollection<GripperConfigEntity> Grippers { get { return _grippers; } }

        public async Task LoadGrippersAsync()
        {
            var result = await Task.Run(() => _gripperCrud.QueryAll());
            _grippers.Clear();
            if (result.Success && result.Items != null)
            {
                foreach (var item in result.Items)
                    _grippers.Add(item);
            }
        }

        public Result SaveGripper(GripperConfigEntity entity)
        {
            return _gripperCrud.Save(entity);
        }

        public Result DeleteGripper(string name)
        {
            return _gripperCrud.DeleteByName(name);
        }
    }
}