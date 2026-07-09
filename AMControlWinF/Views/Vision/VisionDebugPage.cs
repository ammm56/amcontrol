using AM.Model.Camera;
using AM.Model.Common;
using AM.Model.Entity.Device;
using AM.Model.Vision;
using AM.PageModel.Vision;
using AMControlWinF.Tools;
using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Button = AntdUI.Button;

namespace AMControlWinF.Views.Vision
{
    /// <summary>
    /// 视觉调试页面。
    /// 本页面只做 amcontrol 相机取图、仓库内 amvision .NET SDK 按钮调用、原始结果展示与调用记录落库。
    /// </summary>
    public partial class VisionDebugPage : UserControl
    {
        private readonly VisionDebugPageModel _model;
        private readonly System.Windows.Forms.Timer _previewTimer;
        private readonly Dictionary<string, string> _cameraDisplayMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Button, VisionSdkDebugOperationKey> _operationMap =
            new Dictionary<Button, VisionSdkDebugOperationKey>();

        private CancellationTokenSource _continuousTokenSource;
        private bool _isLoaded;
        private bool _isBinding;
        private bool _isBusy;
        private bool _isPreviewRunning;
        private bool _isPreviewTickBusy;

        public VisionDebugPage()
        {
            InitializeComponent();

            _model = new VisionDebugPageModel();
            _previewTimer = new System.Windows.Forms.Timer();

            InitializeOperationButtons();
            InitializeStaticUi();
            BindEvents();
            RefreshActionState();
        }

        private void DisposeRuntimeResources()
        {
            StopPreview();

            if (_continuousTokenSource != null)
            {
                _continuousTokenSource.Cancel();
                _continuousTokenSource.Dispose();
                _continuousTokenSource = null;
            }

            if (_previewTimer != null)
            {
                _previewTimer.Dispose();
            }

            if (_model != null)
            {
                _model.Dispose();
            }
        }

        private void InitializeStaticUi()
        {
            cameraLivePreview.SetTitle("实时预览");
            cameraLivePreview.ClearImage("未打开相机");

            inputImagePreview.SetTitle("调用输入图");
            inputImagePreview.ClearImage("等待取图");

            selectExecutionMode.Items.Clear();
            selectExecutionMode.Items.Add("单次");
            selectExecutionMode.Items.Add("连续");
            selectExecutionMode.SelectedValue = "单次";
        }

        private void InitializeOperationButtons()
        {
            flowOperations.SuspendLayout();
            try
            {
                flowOperations.Controls.Clear();
                _operationMap.Clear();

                foreach (var group in VisionSdkDebugOperationCatalog.All.GroupBy(x => x.GroupName))
                {
                    flowOperations.Controls.Add(CreateOperationSection(group.Key, group.ToList()));
                }
            }
            finally
            {
                flowOperations.ResumeLayout(true);
            }
        }

        private Control CreateOperationSection(string title, IList<VisionSdkDebugOperationInfo> operations)
        {
            var panel = new System.Windows.Forms.Panel
            {
                Width = 296,
                Height = 42 + ((operations.Count + 1) / 2) * 42,
                Margin = new Padding(0, 0, 0, 10)
            };

            var label = new AntdUI.Label
            {
                Text = title,
                Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(296, 28),
                Margin = new Padding(0)
            };
            panel.Controls.Add(label);

            var buttons = new FlowLayoutPanel
            {
                Location = new Point(0, 32),
                Size = new Size(296, panel.Height - 32),
                Margin = new Padding(0),
                AutoScroll = false,
                WrapContents = true
            };

            foreach (var operation in operations)
            {
                var button = new Button
                {
                    Text = operation.DisplayName,
                    Tag = operation.Key,
                    Size = new Size(140, 34),
                    Margin = new Padding(0, 0, 8, 8),
                    Radius = 8,
                    Ghost = true,
                    WaveSize = 0,
                    Type = TTypeMini.Default
                };

                button.Click += async (s, e) => await ExecuteOperationFromButtonAsync((Button)s);
                _operationMap[button] = operation.Key;
                buttons.Controls.Add(button);
            }

            panel.Controls.Add(buttons);
            return panel;
        }

        private void BindEvents()
        {
            Load += VisionDebugPage_Load;
            selectCamera.SelectedValueChanged += (s, e) => SelectCameraChanged();
            selectRuntime.SelectedValueChanged += (s, e) => SelectRuntimeChanged();
            selectTrigger.SelectedValueChanged += (s, e) => SelectTriggerChanged();
            selectModelDeployment.SelectedValueChanged += (s, e) => SelectModelDeploymentChanged();
            selectExecutionMode.SelectedValueChanged += (s, e) => SelectExecutionModeChanged();

            buttonRefreshConfig.Click += async (s, e) => await ReloadAsync();
            buttonOpenCamera.Click += async (s, e) => await ToggleOpenCameraAsync();
            buttonTogglePreview.Click += async (s, e) => await TogglePreviewAsync();
            buttonGrabInput.Click += async (s, e) => await GrabInputImageAsync();
            buttonCameraSettings.Click += async (s, e) => await ShowCameraSettingsAsync();
            buttonStopContinuous.Click += (s, e) => StopContinuous();
            _previewTimer.Tick += async (s, e) => await PreviewTimerTickAsync();
        }

        private async void VisionDebugPage_Load(object sender, EventArgs e)
        {
            if (_isLoaded)
            {
                return;
            }

            _isLoaded = true;
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (_isBusy)
            {
                return;
            }

            StopContinuous();
            StopPreview();
            SetBusyState(true);
            try
            {
                labelStatus.Text = "正在加载视觉调试配置...";
                await CloseSelectedCameraBeforeReloadAsync();
                var result = await _model.LoadAsync();
                BindSelections();
                RefreshStats();

                labelStatus.Text = result.Success ? "视觉调试配置已加载" : result.Message;
                if (!result.Success)
                {
                    textResponseJson.Text = BuildPlainResultText(result);
                }
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task CloseSelectedCameraBeforeReloadAsync()
        {
            if (_model.SelectedCamera == null)
            {
                return;
            }

            await _model.CloseSelectedCameraAsync();
            cameraLivePreview.ClearImage("未打开相机");
        }

        private void BindSelections()
        {
            _isBinding = true;
            try
            {
                BindCameraSelect();
                BindTextSelect(selectRuntime, _model.RuntimeNames, _model.SelectedRuntimeName);
                BindTextSelect(selectTrigger, _model.TriggerSourceNames, _model.SelectedTriggerSourceName);
                BindTextSelect(selectModelDeployment, _model.ModelDeploymentNames, _model.SelectedModelDeploymentName);
                SetSelectValue(selectExecutionMode, _model.ExecutionMode == VisionSdkDebugExecutionMode.Continuous ? "连续" : "单次");
                inputModelInputUri.Text = _model.ModelInputUri ?? string.Empty;
                inputModelInputFileId.Text = _model.ModelInputFileId ?? string.Empty;
                inputModelInferenceTaskId.Text = _model.ModelInferenceTaskId ?? string.Empty;
            }
            finally
            {
                _isBinding = false;
            }
        }

        private void BindCameraSelect()
        {
            _cameraDisplayMap.Clear();
            selectCamera.Items.Clear();

            string selectedDisplay = null;
            foreach (var camera in _model.Cameras)
            {
                var display = GetCameraDisplayText(camera);
                _cameraDisplayMap[display] = camera.CameraCode;
                selectCamera.Items.Add(display);

                if (_model.SelectedCamera != null &&
                    string.Equals(camera.CameraCode, _model.SelectedCamera.CameraCode, StringComparison.OrdinalIgnoreCase))
                {
                    selectedDisplay = display;
                }
            }

            SetSelectValue(selectCamera, selectedDisplay);
        }

        private static void BindTextSelect(Select select, IEnumerable<string> values, string selectedValue)
        {
            select.Items.Clear();
            foreach (var value in values)
            {
                select.Items.Add(value);
            }

            SetSelectValue(select, selectedValue);
        }

        private static void SetSelectValue(Select select, string value)
        {
            select.SelectedValue = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private void SelectCameraChanged()
        {
            if (_isBinding)
            {
                return;
            }

            StopPreview();
            var text = GetSelectedText(selectCamera);
            string cameraCode;
            _model.SelectCamera(_cameraDisplayMap.TryGetValue(text, out cameraCode) ? cameraCode : null);
            cameraLivePreview.ClearImage(_model.SelectedCamera == null ? "未选择相机" : "未打开相机");
            inputImagePreview.ClearImage("等待取图");
            RefreshStats();
            RefreshActionState();
        }

        private void SelectRuntimeChanged()
        {
            if (_isBinding)
            {
                return;
            }

            _model.SelectRuntime(GetSelectedText(selectRuntime));
        }

        private void SelectTriggerChanged()
        {
            if (_isBinding)
            {
                return;
            }

            _model.SelectTriggerSource(GetSelectedText(selectTrigger));
        }

        private void SelectModelDeploymentChanged()
        {
            if (_isBinding)
            {
                return;
            }

            _model.SelectModelDeployment(GetSelectedText(selectModelDeployment));
        }

        private void SelectExecutionModeChanged()
        {
            if (_isBinding)
            {
                return;
            }

            _model.ExecutionMode = string.Equals(GetSelectedText(selectExecutionMode), "连续", StringComparison.OrdinalIgnoreCase)
                ? VisionSdkDebugExecutionMode.Continuous
                : VisionSdkDebugExecutionMode.Single;
            RefreshActionState();
        }

        private async Task ToggleOpenCameraAsync()
        {
            if (_isBusy || _model.SelectedCamera == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                Result result;
                if (_model.IsSelectedCameraKnownOpen)
                {
                    StopPreview();
                    result = await _model.CloseSelectedCameraAsync();
                    cameraLivePreview.ClearImage("未打开相机");
                }
                else
                {
                    result = await _model.OpenSelectedCameraAsync();
                }

                labelStatus.Text = result.Message;
                if (!result.Success)
                {
                    PageDialogHelper.ShowWarn(this, "相机操作", result.Message);
                }

                RefreshStats();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task TogglePreviewAsync()
        {
            if (_isBusy || _model.SelectedCamera == null)
            {
                return;
            }

            if (_isPreviewRunning)
            {
                StopPreview();
                labelStatus.Text = "相机预览已暂停";
                RefreshActionState();
                return;
            }

            SetBusyState(true);
            try
            {
                var openResult = await _model.OpenSelectedCameraAsync();
                if (!openResult.Success)
                {
                    PageDialogHelper.ShowWarn(this, "开始预览", openResult.Message);
                    labelStatus.Text = openResult.Message;
                    return;
                }

                _previewTimer.Interval = _model.ResolvePreviewIntervalMs();
                _isPreviewRunning = true;
                _previewTimer.Start();
                labelStatus.Text = "相机预览已开始";
                RefreshStats();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task PreviewTimerTickAsync()
        {
            if (!_isPreviewRunning || _isPreviewTickBusy || _model.SelectedCamera == null)
            {
                return;
            }

            _isPreviewTickBusy = true;
            try
            {
                var result = await _model.GrabPreviewFrameAsync();
                if (result.Success && result.Item != null)
                {
                    cameraLivePreview.ShowPreviewFrame(result.Item);
                }
                else
                {
                    labelStatus.Text = result.Message;
                }
            }
            finally
            {
                _isPreviewTickBusy = false;
            }
        }

        private async Task GrabInputImageAsync()
        {
            if (_isBusy || _model.SelectedCamera == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await _model.GrabInputFrameAsync();
                if (result.Success && result.Item != null)
                {
                    inputImagePreview.ShowFrame(result.Item);
                    labelStatus.Text = "调用输入图已取图";
                }
                else
                {
                    PageDialogHelper.ShowWarn(this, "取图", result.Message);
                    labelStatus.Text = result.Message;
                }

                RefreshStats();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ShowCameraSettingsAsync()
        {
            if (_isBusy || _model.SelectedCamera == null)
            {
                return;
            }

            SetBusyState(true);
            try
            {
                var result = await _model.ShowSelectedCameraSettingsAsync();
                labelStatus.Text = result.Message;
                if (!result.Success)
                {
                    PageDialogHelper.ShowWarn(this, "相机设置", result.Message);
                }

                RefreshStats();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private async Task ExecuteOperationFromButtonAsync(Button button)
        {
            if (button == null || _isBusy)
            {
                return;
            }

            VisionSdkDebugOperationKey operationKey;
            if (!_operationMap.TryGetValue(button, out operationKey))
            {
                return;
            }

            var info = VisionSdkDebugOperationCatalog.Get(operationKey);
            if (info.RequiresConfirm)
            {
                var ok = PageDialogHelper.Confirm(
                    this,
                    "视觉 SDK 控制",
                    "确定执行 " + info.DisplayName + " 吗？",
                    TType.Warn);
                if (!ok)
                {
                    return;
                }
            }

            if (_model.ExecutionMode == VisionSdkDebugExecutionMode.Continuous)
            {
                await StartContinuousAsync(operationKey);
            }
            else
            {
                await ExecuteOnceAsync(operationKey, CancellationToken.None);
            }
        }

        private async Task StartContinuousAsync(VisionSdkDebugOperationKey operationKey)
        {
            if (_continuousTokenSource != null)
            {
                return;
            }

            _continuousTokenSource = new CancellationTokenSource();
            labelStatus.Text = "连续调用已开始：" + VisionSdkDebugOperationCatalog.Get(operationKey).DisplayName;
            RefreshActionState();

            try
            {
                while (!_continuousTokenSource.IsCancellationRequested)
                {
                    await ExecuteOnceAsync(operationKey, _continuousTokenSource.Token);
                    await Task.Delay(1, _continuousTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_continuousTokenSource != null)
                {
                    _continuousTokenSource.Dispose();
                    _continuousTokenSource = null;
                }

                labelStatus.Text = "连续调用已停止";
                RefreshActionState();
            }
        }

        private void StopContinuous()
        {
            if (_continuousTokenSource == null)
            {
                return;
            }

            _continuousTokenSource.Cancel();
        }

        private async Task ExecuteOnceAsync(
            VisionSdkDebugOperationKey operationKey,
            CancellationToken cancellationToken)
        {
            if (_isBusy)
            {
                return;
            }

            var info = VisionSdkDebugOperationCatalog.Get(operationKey);
            _isBusy = true;
            try
            {
                labelStatus.Text = "正在调用：" + info.DisplayName;
                SyncModelDebugArguments();
                var result = await _model.ExecuteOperationAsync(operationKey, cancellationToken);

                if (_model.LastInputFrame != null)
                {
                    inputImagePreview.ShowFrame(_model.LastInputFrame);
                }
                else if (info.UsesCameraImage)
                {
                    inputImagePreview.ClearImage("取图失败");
                }

                ShowOperationResult(result);
                FillInferenceTaskIdIfNeeded(result == null ? null : result.Item);
                RefreshStats(false);
            }
            finally
            {
                _isBusy = false;
            }
        }

        private void ShowOperationResult(Result<VisionSdkDebugResult> result)
        {
            var item = result == null ? null : result.Item;
            if (item == null)
            {
                textResponseJson.Text = result == null ? string.Empty : BuildPlainResultText(result);
                labelResultSummary.Text = result == null ? "无调用结果" : result.Message;
                labelStatus.Text = result == null ? "无调用结果" : result.Message;
                return;
            }

            textResponseJson.Text = string.IsNullOrWhiteSpace(item.ResponseJson)
                ? BuildDebugResultText(item)
                : item.ResponseJson;

            labelLastElapsedValue.Text = FormatTotalElapsed(item);
            labelResultSummary.Text = FormatTimingSummary(item);
            labelStatus.Text = result.Success ? result.Message : item.ErrorMessage ?? result.Message;
        }

        private void RefreshStats()
        {
            RefreshStats(true);
        }

        private void RefreshStats(bool refreshActions)
        {
            labelCameraCount.Text = _model.Cameras.Count.ToString();
            labelOpenedCameraCount.Text = _model.OpenedCameraCount.ToString();
            labelRuntimeCount.Text = _model.RuntimeNames.Count.ToString();
            labelTriggerCount.Text = _model.TriggerSourceNames.Count.ToString();
            labelModelDeploymentCount.Text = _model.ModelDeploymentNames.Count.ToString();

            if (_model.LastResult != null)
            {
                labelLastElapsedValue.Text = FormatTotalElapsed(_model.LastResult);
            }
            else
            {
                labelLastElapsedValue.Text = "--";
            }

            if (refreshActions)
            {
                RefreshActionState();
            }
        }

        private void SetBusyState(bool isBusy)
        {
            _isBusy = isBusy;
            RefreshActionState();
        }

        private void RefreshActionState()
        {
            var hasCamera = _model.SelectedCamera != null;
            var inContinuous = _continuousTokenSource != null;

            selectCamera.Enabled = !_isBusy && !inContinuous;
            selectRuntime.Enabled = !_isBusy && !inContinuous;
            selectTrigger.Enabled = !_isBusy && !inContinuous;
            selectModelDeployment.Enabled = !_isBusy && !inContinuous;
            selectExecutionMode.Enabled = !_isBusy && !inContinuous;
            inputModelInputUri.Enabled = !_isBusy && !inContinuous;
            inputModelInputFileId.Enabled = !_isBusy && !inContinuous;
            inputModelInferenceTaskId.Enabled = !_isBusy && !inContinuous;
            buttonRefreshConfig.Enabled = !_isBusy && !inContinuous;

            buttonOpenCamera.Enabled = !_isBusy && !inContinuous && hasCamera;
            buttonOpenCamera.Text = _model.IsSelectedCameraKnownOpen ? "关闭" : "打开";
            buttonOpenCamera.IconSvg = _model.IsSelectedCameraKnownOpen ? "CloseCircleOutlined" : "PlayCircleOutlined";

            buttonTogglePreview.Enabled = !_isBusy && !inContinuous && hasCamera;
            buttonTogglePreview.Text = _isPreviewRunning ? "暂停" : "开始";
            buttonTogglePreview.IconSvg = _isPreviewRunning ? "PauseCircleOutlined" : "VideoCameraOutlined";
            buttonGrabInput.Enabled = !_isBusy && !inContinuous && hasCamera;
            buttonCameraSettings.Enabled = !_isBusy && !inContinuous && hasCamera;

            buttonStopContinuous.Enabled = inContinuous;
            foreach (var button in _operationMap.Keys)
            {
                button.Enabled = !_isBusy && !inContinuous;
            }
        }

        private void StopPreview()
        {
            _previewTimer.Stop();
            _isPreviewRunning = false;
            _isPreviewTickBusy = false;
        }

        private static string GetSelectedText(Select select)
        {
            return select == null || select.SelectedValue == null
                ? string.Empty
                : select.SelectedValue.ToString().Trim();
        }

        private static string GetCameraDisplayText(CameraConfigEntity camera)
        {
            if (camera == null)
            {
                return string.Empty;
            }

            var name = string.IsNullOrWhiteSpace(camera.CameraName) ? camera.CameraCode : camera.CameraName.Trim();
            return name + " [" + camera.CameraCode + "]";
        }

        private static string BuildPlainResultText(Result result)
        {
            if (result == null)
            {
                return string.Empty;
            }

            return string.Format(
                "{{\r\n  \"success\": {0},\r\n  \"code\": {1},\r\n  \"message\": \"{2}\"\r\n}}",
                result.Success ? "true" : "false",
                result.Code,
                EscapeJson(result.Message));
        }

        private static string BuildDebugResultText(VisionSdkDebugResult result)
        {
            if (result == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"operation\": \"" + EscapeJson(result.OperationName) + "\",");
            builder.AppendLine("  \"success\": " + (result.IsSuccess ? "true" : "false") + ",");
            builder.AppendLine("  \"model_deployment_name\": \"" + EscapeJson(result.ModelDeploymentName) + "\",");
            builder.AppendLine("  \"inference_task_id\": \"" + EscapeJson(result.InferenceTaskId) + "\",");
            builder.AppendLine("  \"total_elapsed_ms\": " + result.TotalElapsedMs + ",");
            builder.AppendLine("  \"camera_capture_encode_ms\": " + result.CameraCaptureEncodeMs + ",");
            builder.AppendLine("  \"sdk_invoke_ms\": " + result.SdkInvokeMs + ",");
            builder.AppendLine("  \"response_process_ms\": " + result.ResponseProcessMs + ",");
            builder.AppendLine("  \"state\": \"" + EscapeJson(result.State) + "\",");
            builder.AppendLine("  \"workflow_run_id\": \"" + EscapeJson(result.WorkflowRunId) + "\",");
            builder.AppendLine("  \"error\": \"" + EscapeJson(result.ErrorMessage) + "\"");
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static string FormatTotalElapsed(VisionSdkDebugResult item)
        {
            if (item == null)
            {
                return "--";
            }

            var total = item.TotalElapsedMs > 0 ? item.TotalElapsedMs : item.ElapsedMs;
            return total + "ms";
        }

        private static string FormatTimingSummary(VisionSdkDebugResult item)
        {
            if (item == null)
            {
                return "等待调用";
            }

            var total = item.TotalElapsedMs > 0 ? item.TotalElapsedMs : item.ElapsedMs;
            return string.Format(
                "{0}  {1}  总 {2}ms  取图编码 {3}ms  SDK {4}ms  处理 {5}ms",
                item.OperationName,
                item.IsSuccess ? "成功" : "失败",
                total,
                item.CameraCaptureEncodeMs,
                item.SdkInvokeMs,
                item.ResponseProcessMs);
        }

        private void SyncModelDebugArguments()
        {
            _model.SetModelDebugArguments(
                inputModelInputUri.Text,
                inputModelInputFileId.Text,
                inputModelInferenceTaskId.Text);
        }

        private void FillInferenceTaskIdIfNeeded(VisionSdkDebugResult item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.InferenceTaskId))
            {
                return;
            }

            inputModelInferenceTaskId.Text = item.InferenceTaskId;
            _model.SetModelDebugArguments(
                inputModelInputUri.Text,
                inputModelInputFileId.Text,
                inputModelInferenceTaskId.Text);
        }

        private static string EscapeJson(string value)
        {
            return string.IsNullOrEmpty(value)
                ? string.Empty
                : value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
