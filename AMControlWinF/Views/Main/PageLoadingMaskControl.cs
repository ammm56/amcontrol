using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMControlWinF.Views.Main
{
    /// <summary>
    /// 主工作区页面加载遮罩。
    /// 负责：
    /// 1. 在页面首次创建期间覆盖主工作区；
    /// 2. 使用 Progress 展示固定时长内的进度推进；
    /// 3. 根据导航会话令牌避免旧异步回调误隐藏当前遮罩。
    /// </summary>
    public partial class PageLoadingMaskControl : UserControl
    {
        private readonly Timer _animationTimer;
        private readonly Stopwatch _stopwatch;

        private int _targetDurationMs;
        private int _sessionToken;
        private bool _isCompleting;

        private const int DefaultRenderDelayMs = 60;
        private const int DefaultMinDurationMs = 800;

        public PageLoadingMaskControl()
        {
            InitializeComponent();

            _animationTimer = new Timer();
            _animationTimer.Interval = 16;
            _animationTimer.Tick += AnimationTimer_Tick;

            _stopwatch = new Stopwatch();

            Visible = false;
        }

        /// <summary>
        /// 遮罩显示前的最小绘制延迟。
        /// </summary>
        public int RenderDelayMs
        {
            get { return DefaultRenderDelayMs; }
        }

        /// <summary>
        /// 遮罩最短显示时长。
        /// </summary>
        public int MinDurationMs
        {
            get { return DefaultMinDurationMs; }
        }

        /// <summary>
        /// 开始一次新的遮罩会话，使用内部默认时长。
        /// </summary>
        public void Begin(int sessionToken)
        {
            BeginCore(DefaultMinDurationMs, sessionToken);
        }

        /// <summary>
        /// 结束当前会话并隐藏。
        /// 仅当传入令牌仍为当前会话时生效。
        /// </summary>
        public async Task CompleteAndHideAsync(int sessionToken)
        {
            if (sessionToken != _sessionToken || IsDisposed)
                return;

            if (_isCompleting)
                return;

            _isCompleting = true;

            var elapsed = (int)_stopwatch.ElapsedMilliseconds;
            var remain = _targetDurationMs - elapsed;
            if (remain > 0)
                await Task.Delay(remain);

            if (sessionToken != _sessionToken || IsDisposed)
                return;

            _animationTimer.Stop();
            _stopwatch.Stop();

            progressLoading.Value = 1F;

            await Task.Delay(30);

            if (sessionToken != _sessionToken || IsDisposed)
                return;

            HideImmediately();
        }

        /// <summary>
        /// 立即隐藏并重置。
        /// </summary>
        public void HideImmediately()
        {
            _isCompleting = false;

            _animationTimer.Stop();
            _stopwatch.Reset();

            if (!progressLoading.IsDisposed)
            {
                progressLoading.Loading = false;
                progressLoading.Value = 0F;
            }

            Visible = false;
        }

        private void BeginCore(int durationMs, int sessionToken)
        {
            _sessionToken = sessionToken;
            _isCompleting = false;
            _targetDurationMs = durationMs <= 0 ? 1 : durationMs;

            _animationTimer.Stop();
            _stopwatch.Reset();
            _stopwatch.Start();

            progressLoading.Loading = false;
            progressLoading.Value = 0F;

            Visible = true;
            BringToFront();

            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (!Visible || IsDisposed)
            {
                _animationTimer.Stop();
                return;
            }

            float value;
            if (_targetDurationMs <= 0)
            {
                value = 0.95F;
            }
            else
            {
                value = (float)_stopwatch.ElapsedMilliseconds / _targetDurationMs;
                if (value < 0F)
                    value = 0F;
                if (value > 0.95F)
                    value = 0.95F;
            }

            progressLoading.Value = value;

            if (value >= 0.95F)
                _animationTimer.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer.Stop();
                _animationTimer.Dispose();
                _stopwatch.Stop();
            }

            base.Dispose(disposing);
        }
    }
}