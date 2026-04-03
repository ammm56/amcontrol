using AM.Core.Context;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 控制卡删除确认对话框。
    /// </summary>
    public partial class MotionCardDeleteConfirmDialog : AntdUI.Window
    {
        public MotionCardDeleteConfirmDialog()
        {
            InitializeComponent();
            BindEvents();
            ApplyThemeFromConfig();
            ApplyDialogText();
        }

        public short TargetCardId { get; set; }

        public string TargetDisplayName { get; set; }

        public string TargetName { get; set; }

        private void BindEvents()
        {
            buttonOk.Click += ButtonOk_Click;
            buttonCancel.Click += ButtonCancel_Click;
            Shown += MotionCardDeleteConfirmDialog_Shown;

            KeyPreview = true;
            KeyDown += MotionCardDeleteConfirmDialog_KeyDown;
        }

        private void MotionCardDeleteConfirmDialog_Shown(object sender, EventArgs e)
        {
            ApplyDialogText();
            buttonOk.Focus();
        }

        private void ApplyDialogText()
        {
            Text = "删除控制卡";
            labelDialogTitle.Text = "删除控制卡";
            labelDialogDescription.Text = "请确认是否继续执行当前操作。";

            labelCardIdValue.Text = TargetCardId.ToString();
            labelDisplayNameValue.Text = string.IsNullOrWhiteSpace(TargetDisplayName) ? "-" : TargetDisplayName;
            labelInternalNameValue.Text = string.IsNullOrWhiteSpace(TargetName) ? "-" : TargetName;
            labelHintValue.Text = "删除前请确认该控制卡下不存在轴定义和 IO 映射。";
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void MotionCardDeleteConfirmDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;

            DialogResult = DialogResult.Cancel;
            Close();
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
    }
}