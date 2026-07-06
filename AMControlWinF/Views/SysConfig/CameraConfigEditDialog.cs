using AM.Core.Context;
using AM.Model.Entity.Device;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.SysConfig
{
    /// <summary>
    /// 相机配置新增/编辑对话框。
    /// 主体参数放在 CameraConfigEditControl 中，保持页面和编辑职责边界清晰。
    /// </summary>
    public partial class CameraConfigEditDialog : AntdUI.Window
    {
        private bool _isCreateMode = true;

        public CameraConfigEditDialog()
        {
            InitializeComponent();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyMode();
        }

        public bool IsCreateMode
        {
            get { return _isCreateMode; }
            set
            {
                _isCreateMode = value;
                ApplyMode();
            }
        }

        public CameraConfigEntity ResultEntity { get; private set; }

        public void SetEntity(CameraConfigEntity entity)
        {
            editor.SetEntity(entity);
        }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
            Shown += CameraConfigEditDialog_Shown;

            KeyPreview = true;
            KeyDown += CameraConfigEditDialog_KeyDown;
        }

        private void CameraConfigEditDialog_Shown(object sender, EventArgs e)
        {
            editor.FocusFirst();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            var result = editor.BuildEntity();
            if (!result.Success || result.Item == null)
            {
                PageDialogHelper.ShowWarn(this, "输入提示", result.Message);
                return;
            }

            ResultEntity = result.Item;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CameraConfigEditDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void ApplyMode()
        {
            Text = _isCreateMode ? "新增相机" : "编辑相机";
            labelTitle.Text = _isCreateMode ? "新增相机" : "编辑相机";
            labelDescription.Text = _isCreateMode
                ? "配置 amcontrol 本项目相机的采集、编码与预览参数。"
                : "修改 amcontrol 本项目相机的采集、编码与预览参数。";
            buttonOk.Text = "保存";
        }

        private void ApplyThemeFromConfig()
        {
            var theme = ConfigContext.Instance.Config.Setting.Theme;
            var isDarkMode = IsDarkTheme(theme);

            if (isDarkMode)
            {
                Config.IsDark = true;
            }
            else
            {
                Config.IsLight = true;
            }

            textureBackgroundDialog.SetTheme(isDarkMode);
        }

        private static bool IsDarkTheme(string theme)
        {
            return !string.IsNullOrWhiteSpace(theme)
                && (string.Equals(theme, "黑色", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase));
        }
    }
}
