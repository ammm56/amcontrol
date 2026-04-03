using AM.Core.Context;
using AM.PageModel.MotionConfig;
using System;
using System.Windows.Forms;

namespace AMControlWinF.Views.MotionConfig
{
    /// <summary>
    /// 控制卡详情窗口。
    /// 使用统一的 AntdUI.Window + TextureBackgroundControl 风格。
    /// 无底部按钮，点击窗口外区域自动关闭。
    /// </summary>
    public partial class MotionCardDetailDialog : AntdUI.Window
    {
        private bool _closingByDeactivate;
        private MotionCardManagementPageModel.MotionCardViewItem _item;

        public MotionCardDetailDialog()
        {
            InitializeComponent();
            BindEvents();
            ApplyThemeFromConfig();
        }

        public MotionCardDetailDialog(MotionCardManagementPageModel.MotionCardViewItem item)
            : this()
        {
            Bind(item);
        }

        public void Bind(MotionCardManagementPageModel.MotionCardViewItem item)
        {
            _item = item;

            Text = "控制卡详情";
            labelDialogTitle.Text = "控制卡详情";
            labelDialogDescription.Text = item == null
                ? "查看控制卡详细信息。"
                : "查看控制卡【" + (string.IsNullOrWhiteSpace(item.DisplayName) ? item.Name : item.DisplayName) + "】的详细信息。";

            motionCardDetailControl.Bind(item);
        }

        private void BindEvents()
        {
            Shown += MotionCardDetailDialog_Shown;
            Deactivate += MotionCardDetailDialog_Deactivate;

            KeyPreview = true;
            KeyDown += MotionCardDetailDialog_KeyDown;
        }

        private void MotionCardDetailDialog_Shown(object sender, EventArgs e)
        {
            motionCardDetailControl.Focus();
        }

        private void MotionCardDetailDialog_Deactivate(object sender, EventArgs e)
        {
            if (_closingByDeactivate || !Visible)
                return;

            _closingByDeactivate = true;
            try
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            finally
            {
                _closingByDeactivate = false;
            }
        }

        private void MotionCardDetailDialog_KeyDown(object sender, KeyEventArgs e)
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