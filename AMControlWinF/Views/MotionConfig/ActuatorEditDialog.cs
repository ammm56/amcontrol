using AM.Core.Context;
using AM.DBService.Services.Motion.Topology;
using AM.Model.Entity.Motion.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 执行器新增/编辑对话框。
    /// 仅负责承载具体编辑控件。
    /// </summary>
    public partial class ActuatorEditDialog : AntdUI.Window
    {
        private readonly string _actuatorType;
        private readonly object _sourceEntity;
        private readonly bool _isAdd;
        private readonly MotionIoMapCrudService _ioMapService;
        private List<MotionIoMapEntity> _allIoItems;
        private Control _editorControl;

        public ActuatorEditDialog(string actuatorType, object entity, bool isAdd)
        {
            InitializeComponent();

            _actuatorType = NormalizeActuatorType(actuatorType);
            _sourceEntity = entity;
            _isAdd = isAdd;
            _ioMapService = new MotionIoMapCrudService();
            _allIoItems = new List<MotionIoMapEntity>();

            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
            LoadIoItems();
            BuildEditorControl();
        }

        public string ResultActuatorType { get; private set; }

        public object ResultEntity { get; private set; }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
            KeyPreview = true;
            KeyDown += ActuatorEditDialog_KeyDown;
        }

        private void ApplyMode()
        {
            var typeText = GetActuatorTypeDisplayName(_actuatorType);

            Text = _isAdd ? "新增" + typeText : "编辑" + typeText;
            labelDialogTitle.Text = _isAdd ? "新增" + typeText : "编辑" + typeText;
            labelDialogDescription.Text = _isAdd
                ? "填写" + typeText + "配置"
                : "修改" + typeText + "配置";

            buttonOk.Text = "保存";
        }

        private void LoadIoItems()
        {
            var result = _ioMapService.QueryAll();
            if (result.Success && result.Items != null)
            {
                _allIoItems = result.Items
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.LogicalBit)
                    .ToList();
            }
            else
            {
                _allIoItems = new List<MotionIoMapEntity>();
            }
        }

        private void BuildEditorControl()
        {
            panelEditorHost.SuspendLayout();
            try
            {
                panelEditorHost.Controls.Clear();
                _editorControl = CreateEditorControl();
                if (_editorControl != null)
                {
                    _editorControl.Dock = DockStyle.Fill;
                    panelEditorHost.Controls.Add(_editorControl);
                }
            }
            finally
            {
                panelEditorHost.ResumeLayout();
            }
        }

        private Control CreateEditorControl()
        {
            if (string.Equals(_actuatorType, "Cylinder", StringComparison.OrdinalIgnoreCase))
            {
                var control = new CylinderEditControl();
                control.Bind(_sourceEntity as AM.Model.Entity.Motion.Actuator.CylinderConfigEntity, _isAdd, _allIoItems);
                return control;
            }

            if (string.Equals(_actuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase))
            {
                var control = new VacuumEditControl();
                control.Bind(_sourceEntity as AM.Model.Entity.Motion.Actuator.VacuumConfigEntity, _isAdd, _allIoItems);
                return control;
            }

            if (string.Equals(_actuatorType, "StackLight", StringComparison.OrdinalIgnoreCase))
            {
                var control = new StackLightEditControl();
                control.Bind(_sourceEntity as AM.Model.Entity.Motion.Actuator.StackLightConfigEntity, _isAdd, _allIoItems);
                return control;
            }

            if (string.Equals(_actuatorType, "Gripper", StringComparison.OrdinalIgnoreCase))
            {
                var control = new GripperEditControl();
                control.Bind(_sourceEntity as AM.Model.Entity.Motion.Actuator.GripperConfigEntity, _isAdd, _allIoItems);
                return control;
            }

            return null;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            object entity;
            if (!TryBuildEntity(out entity))
                return;

            ResultActuatorType = _actuatorType;
            ResultEntity = entity;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool TryBuildEntity(out object entity)
        {
            entity = null;

            var cylinderControl = _editorControl as CylinderEditControl;
            if (cylinderControl != null)
            {
                AM.Model.Entity.Motion.Actuator.CylinderConfigEntity result;
                if (!cylinderControl.TryBuildEntity(out result))
                    return false;

                entity = result;
                return true;
            }

            var vacuumControl = _editorControl as VacuumEditControl;
            if (vacuumControl != null)
            {
                AM.Model.Entity.Motion.Actuator.VacuumConfigEntity result;
                if (!vacuumControl.TryBuildEntity(out result))
                    return false;

                entity = result;
                return true;
            }

            var stackLightControl = _editorControl as StackLightEditControl;
            if (stackLightControl != null)
            {
                AM.Model.Entity.Motion.Actuator.StackLightConfigEntity result;
                if (!stackLightControl.TryBuildEntity(out result))
                    return false;

                entity = result;
                return true;
            }

            var gripperControl = _editorControl as GripperEditControl;
            if (gripperControl != null)
            {
                AM.Model.Entity.Motion.Actuator.GripperConfigEntity result;
                if (!gripperControl.TryBuildEntity(out result))
                    return false;

                entity = result;
                return true;
            }

            return false;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ActuatorEditDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            if (e.KeyCode == Keys.Enter && !(ActiveControl is TextBoxBase))
            {
                e.SuppressKeyPress = true;
                ButtonOk_Click(sender, EventArgs.Empty);
            }
        }

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = !string.IsNullOrWhiteSpace(theme) &&
                             (string.Equals(theme, "SkinDark", StringComparison.OrdinalIgnoreCase) ||
                              string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase));

            if (isDarkMode)
            {
                AntdUI.Config.IsDark = true;
            }
            else
            {
                AntdUI.Config.IsLight = true;
            }

            textureBackgroundDialog.SetTheme(isDarkMode);
        }

        private static string NormalizeActuatorType(string actuatorType)
        {
            if (string.IsNullOrWhiteSpace(actuatorType))
                return "Cylinder";
            if (string.Equals(actuatorType, "Cylinder", StringComparison.OrdinalIgnoreCase))
                return "Cylinder";
            if (string.Equals(actuatorType, "Vacuum", StringComparison.OrdinalIgnoreCase))
                return "Vacuum";
            if (string.Equals(actuatorType, "StackLight", StringComparison.OrdinalIgnoreCase))
                return "StackLight";
            if (string.Equals(actuatorType, "Gripper", StringComparison.OrdinalIgnoreCase))
                return "Gripper";

            return actuatorType.Trim();
        }

        private static string GetActuatorTypeDisplayName(string actuatorType)
        {
            switch (NormalizeActuatorType(actuatorType))
            {
                case "Cylinder": return "气缸";
                case "Vacuum": return "真空";
                case "StackLight": return "灯塔";
                case "Gripper": return "夹爪";
                default: return "执行器";
            }
        }
    }
}