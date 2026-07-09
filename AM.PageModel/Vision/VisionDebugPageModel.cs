using AM.CameraService.OpenCv;
using AM.Core.Context;
using AM.DBService.Services.Camera;
using AM.DBService.Services.Vision;
using AM.Model.Camera;
using AM.Model.Common;
using AM.Model.Entity.Device;
using AM.Model.Entity.Vision;
using AM.Model.Interfaces.Camera;
using AM.Model.Interfaces.Vision;
using AM.Model.Vision;
using AM.VisionService.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AM.PageModel.Vision
{
    /// <summary>
    /// 视觉调试页面模型。
    /// 负责本项目相机取图、amvision SDK 调试调用与本项目调用记录落库。
    /// </summary>
    public sealed class VisionDebugPageModel : IDisposable
    {
        private const string DefaultVisionImageDirectory = "Images\\VisionDebug";

        private readonly CameraConfigCrudService _cameraConfigService;
        private readonly ICameraRuntimeService _cameraRuntime;
        private readonly VisionWorkflowRunnerProvider _runnerProvider;
        private readonly VisionSdkDebugService _debugService;
        private readonly IVisionCallRecordService _recordService;
        private readonly SemaphoreSlim _cameraSync = new SemaphoreSlim(1, 1);
        private readonly HashSet<string> _openedCameraCodes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private List<CameraConfigEntity> _cameras = new List<CameraConfigEntity>();
        private List<string> _runtimeNames = new List<string>();
        private List<string> _triggerSourceNames = new List<string>();
        private List<string> _modelDeploymentNames = new List<string>();
        private bool _disposed;

        public VisionDebugPageModel()
            : this(
                new CameraConfigCrudService(),
                new OpenCvCameraRuntimeService(),
                VisionWorkflowRunnerProvider.Shared,
                new VisionSdkDebugService(VisionWorkflowRunnerProvider.Shared),
                new VisionCallRecordService())
        {
        }

        public VisionDebugPageModel(
            CameraConfigCrudService cameraConfigService,
            ICameraRuntimeService cameraRuntime,
            VisionWorkflowRunnerProvider runnerProvider,
            VisionSdkDebugService debugService,
            IVisionCallRecordService recordService)
        {
            _cameraConfigService = cameraConfigService ?? throw new ArgumentNullException("cameraConfigService");
            _cameraRuntime = cameraRuntime ?? throw new ArgumentNullException("cameraRuntime");
            _runnerProvider = runnerProvider ?? throw new ArgumentNullException("runnerProvider");
            _debugService = debugService ?? throw new ArgumentNullException("debugService");
            _recordService = recordService ?? throw new ArgumentNullException("recordService");
            ExecutionMode = VisionSdkDebugExecutionMode.Single;
        }

        public IReadOnlyList<CameraConfigEntity> Cameras
        {
            get { return _cameras; }
        }

        public IReadOnlyList<string> RuntimeNames
        {
            get { return _runtimeNames; }
        }

        public IReadOnlyList<string> TriggerSourceNames
        {
            get { return _triggerSourceNames; }
        }

        public IReadOnlyList<string> ModelDeploymentNames
        {
            get { return _modelDeploymentNames; }
        }

        public CameraConfigEntity SelectedCamera { get; private set; }

        public string SelectedRuntimeName { get; private set; }

        public string SelectedTriggerSourceName { get; private set; }

        public string SelectedModelDeploymentName { get; private set; }

        public string ModelInputUri { get; private set; }

        public string ModelInputFileId { get; private set; }

        public string ModelInferenceTaskId { get; private set; }

        public VisionSdkDebugExecutionMode ExecutionMode { get; set; }

        public CameraFrame LastInputFrame { get; private set; }

        public CameraPreviewFrame LastPreviewFrame { get; private set; }

        public string LastInputImagePath { get; private set; }

        public VisionSdkDebugResult LastResult { get; private set; }

        public int OpenedCameraCount
        {
            get { return _openedCameraCodes.Count; }
        }

        public bool IsSelectedCameraKnownOpen
        {
            get
            {
                return SelectedCamera != null &&
                       !string.IsNullOrWhiteSpace(SelectedCamera.CameraCode) &&
                       _openedCameraCodes.Contains(SelectedCamera.CameraCode);
            }
        }

        public async Task<Result> LoadAsync()
        {
            ThrowIfDisposed();

            var cameraResult = await Task.Run(() => _cameraConfigService.QueryAll()).ConfigureAwait(false);
            if (cameraResult.Success)
            {
                _cameras = cameraResult.Items
                    .Where(x => x != null)
                    .OrderBy(x => x.Id)
                    .ToList();
                SelectedCamera = _cameras.FirstOrDefault(x => x.IsEnabled) ?? _cameras.FirstOrDefault();
            }
            else
            {
                _cameras = new List<CameraConfigEntity>();
                SelectedCamera = null;
            }

            var sdkResult = await RefreshSdkKeysAsync().ConfigureAwait(false);
            if (!cameraResult.Success)
            {
                return Result.Fail(cameraResult.Code, cameraResult.Message, cameraResult.Source);
            }

            if (!sdkResult.Success)
            {
                return sdkResult;
            }

            return Result.Ok("视觉调试数据加载完成", ResultSource.UI);
        }

        public async Task<Result> RefreshSdkKeysAsync()
        {
            ThrowIfDisposed();

            try
            {
                var init = await Task.Run(() => _runnerProvider.EnsureInitialized()).ConfigureAwait(false);
                if (!init.Success)
                {
                    _runtimeNames = new List<string>();
                    _triggerSourceNames = new List<string>();
                    _modelDeploymentNames = new List<string>();
                    SelectedRuntimeName = null;
                    SelectedTriggerSourceName = null;
                    SelectedModelDeploymentName = null;
                    return init;
                }

                _runtimeNames = _runnerProvider.RuntimeNames
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(x => x)
                    .ToList();

                _triggerSourceNames = _runnerProvider.TriggerSourceNames
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(x => x)
                    .ToList();

                _modelDeploymentNames = _runnerProvider.ModelDeploymentNames
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(x => x)
                    .ToList();

                if (string.IsNullOrWhiteSpace(SelectedRuntimeName) ||
                    !_runtimeNames.Any(x => string.Equals(x, SelectedRuntimeName, StringComparison.OrdinalIgnoreCase)))
                {
                    SelectedRuntimeName = _runtimeNames.FirstOrDefault();
                }

                if (string.IsNullOrWhiteSpace(SelectedTriggerSourceName) ||
                    !_triggerSourceNames.Any(x => string.Equals(x, SelectedTriggerSourceName, StringComparison.OrdinalIgnoreCase)))
                {
                    SelectedTriggerSourceName = _triggerSourceNames.FirstOrDefault();
                }

                if (string.IsNullOrWhiteSpace(SelectedModelDeploymentName) ||
                    !_modelDeploymentNames.Any(x => string.Equals(x, SelectedModelDeploymentName, StringComparison.OrdinalIgnoreCase)))
                {
                    SelectedModelDeploymentName = _modelDeploymentNames.FirstOrDefault();
                }

                return Result.Ok("视觉 SDK 配置刷新完成", ResultSource.Unknown);
            }
            catch (Exception ex)
            {
                _runtimeNames = new List<string>();
                _triggerSourceNames = new List<string>();
                _modelDeploymentNames = new List<string>();
                SelectedRuntimeName = null;
                SelectedTriggerSourceName = null;
                SelectedModelDeploymentName = null;
                return Result.Fail(-1, "视觉 SDK 配置刷新失败: " + ex.Message, ResultSource.Unknown);
            }
        }

        public void SelectCamera(string cameraCode)
        {
            SelectedCamera = string.IsNullOrWhiteSpace(cameraCode)
                ? null
                : _cameras.FirstOrDefault(x => string.Equals(x.CameraCode, cameraCode, StringComparison.OrdinalIgnoreCase));
        }

        public void SelectRuntime(string runtimeName)
        {
            SelectedRuntimeName = string.IsNullOrWhiteSpace(runtimeName) ? null : runtimeName.Trim();
        }

        public void SelectTriggerSource(string triggerSourceName)
        {
            SelectedTriggerSourceName = string.IsNullOrWhiteSpace(triggerSourceName) ? null : triggerSourceName.Trim();
        }

        public void SelectModelDeployment(string modelDeploymentName)
        {
            SelectedModelDeploymentName = string.IsNullOrWhiteSpace(modelDeploymentName) ? null : modelDeploymentName.Trim();
        }

        public void SetModelDebugArguments(
            string inputUri,
            string inputFileId,
            string inferenceTaskId)
        {
            ModelInputUri = NormalizeOptional(inputUri);
            ModelInputFileId = NormalizeOptional(inputFileId);
            ModelInferenceTaskId = NormalizeOptional(inferenceTaskId);
        }

        public async Task<Result> OpenSelectedCameraAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            var camera = SelectedCamera;
            if (camera == null)
            {
                return Result.Fail(1, "请先选择相机", ResultSource.UI);
            }

            return await WithCameraLockAsync(() =>
            {
                var result = _cameraRuntime.Open(camera);
                if (result.Success)
                {
                    _openedCameraCodes.Add(camera.CameraCode);
                }

                return result;
            }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> CloseSelectedCameraAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            var camera = SelectedCamera;
            if (camera == null)
            {
                return Result.Fail(1, "请先选择相机", ResultSource.UI);
            }

            return await WithCameraLockAsync(() =>
            {
                var result = _cameraRuntime.Close(camera.CameraCode);
                if (result.Success)
                {
                    _openedCameraCodes.Remove(camera.CameraCode);
                }

                return result;
            }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> ShowSelectedCameraSettingsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            var openResult = await EnsureSelectedCameraOpenAsync(cancellationToken).ConfigureAwait(false);
            if (!openResult.Success)
            {
                return openResult;
            }

            return await WithCameraLockAsync(
                () => _cameraRuntime.ShowSettings(SelectedCamera.CameraCode),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result<bool>> IsSelectedCameraOpenAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            var camera = SelectedCamera;
            if (camera == null)
            {
                return Result<bool>.OkItem(false, "未选择相机", ResultSource.UI);
            }

            return await WithCameraLockAsync(
                () => _cameraRuntime.IsOpen(camera.CameraCode),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result<CameraPreviewFrame>> GrabPreviewFrameAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            var openResult = await EnsureSelectedCameraOpenAsync(cancellationToken).ConfigureAwait(false);
            if (!openResult.Success)
            {
                return Result<CameraPreviewFrame>.Fail(openResult.Code, openResult.Message, openResult.Source);
            }

            var camera = SelectedCamera;
            var result = await WithCameraLockAsync(
                () => _cameraRuntime.GrabPreviewFrame(camera.CameraCode),
                cancellationToken).ConfigureAwait(false);

            if (result.Success)
            {
                LastPreviewFrame = result.Item;
            }

            return result;
        }

        public async Task<Result<CameraFrame>> GrabInputFrameAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            var openResult = await EnsureSelectedCameraOpenAsync(cancellationToken).ConfigureAwait(false);
            if (!openResult.Success)
            {
                return Result<CameraFrame>.Fail(openResult.Code, openResult.Message, openResult.Source);
            }

            var camera = SelectedCamera;
            var result = await WithCameraLockAsync(
                () => _cameraRuntime.GrabFrame(camera.CameraCode),
                cancellationToken).ConfigureAwait(false);

            if (result.Success)
            {
                LastInputFrame = result.Item;
            }

            return result;
        }

        public async Task<Result<VisionSdkDebugResult>> ExecuteOperationAsync(
            VisionSdkDebugOperationKey operationKey,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();

            LastInputFrame = null;
            LastInputImagePath = null;

            var info = VisionSdkDebugOperationCatalog.Get(operationKey);
            var request = new VisionSdkDebugRequest
            {
                OperationKey = operationKey,
                RuntimeName = SelectedRuntimeName,
                TriggerSourceName = SelectedTriggerSourceName,
                ModelDeploymentName = SelectedModelDeploymentName,
                ModelInputUri = ModelInputUri,
                ModelInputFileId = ModelInputFileId,
                ModelInferenceTaskId = ModelInferenceTaskId
            };

            long cameraCaptureEncodeMs = 0L;
            if (info.UsesCameraImage)
            {
                var cameraStopwatch = Stopwatch.StartNew();
                var imageResult = await PrepareCameraImageAsync(info, request, cancellationToken).ConfigureAwait(false);
                cameraStopwatch.Stop();
                cameraCaptureEncodeMs = cameraStopwatch.ElapsedMilliseconds;
                if (!imageResult.Success)
                {
                    var failed = BuildLocalFailureResult(info, imageResult.Message, cameraCaptureEncodeMs);
                    LastResult = failed;
                    await SaveCallRecordAsync(failed).ConfigureAwait(false);
                    return new Result<VisionSdkDebugResult>(
                        false,
                        imageResult.Code,
                        imageResult.Message,
                        imageResult.Source)
                    {
                        Item = failed
                    };
                }
            }

            var result = await _debugService.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
            LastResult = result.Item;

            if (LastResult != null && LastResult.ShouldSaveCallRecord)
            {
                ApplyTotalTiming(LastResult, cameraCaptureEncodeMs);
                await SaveCallRecordAsync(LastResult).ConfigureAwait(false);
            }
            else if (LastResult != null)
            {
                ApplyTotalTiming(LastResult, cameraCaptureEncodeMs);
            }

            return result;
        }

        public int ResolvePreviewIntervalMs()
        {
            var camera = SelectedCamera;
            var cameraFps = camera == null || camera.Fps <= 0 ? 30 : camera.Fps;
            var previewFps = camera == null || camera.PreviewFps <= 0
                ? cameraFps
                : Math.Min(camera.PreviewFps, cameraFps);

            return Math.Max(15, 1000 / Math.Max(1, previewFps));
        }

        private async Task<Result> PrepareCameraImageAsync(
            VisionSdkDebugOperationInfo info,
            VisionSdkDebugRequest request,
            CancellationToken cancellationToken)
        {
            var grabResult = await GrabInputFrameAsync(cancellationToken).ConfigureAwait(false);
            if (!grabResult.Success || grabResult.Item == null)
            {
                return Result.Fail(grabResult.Code, grabResult.Message, grabResult.Source);
            }

            var frame = grabResult.Item;
            if (frame.EncodedBytes == null || frame.EncodedBytes.Length == 0)
            {
                return Result.Fail(2, "相机取图结果为空", ResultSource.Camera);
            }

            LastInputFrame = frame;
            LastInputImagePath = await Task.Run(() => SaveVisionInputImage(frame), cancellationToken).ConfigureAwait(false);
            request.MediaType = string.IsNullOrWhiteSpace(frame.MediaType) ? "image/jpeg" : frame.MediaType;

            if (info.UsesTemporaryImageFile)
            {
                request.ImagePath = LastInputImagePath;
            }
            else if (operationUsesBase64(info.Key))
            {
                request.ImageBase64 = Convert.ToBase64String(frame.EncodedBytes);
            }
            else
            {
                request.ImageBytes = frame.EncodedBytes;
            }

            return Result.Ok("相机取图已准备", ResultSource.Camera);
        }

        private async Task<Result> EnsureSelectedCameraOpenAsync(CancellationToken cancellationToken)
        {
            var camera = SelectedCamera;
            if (camera == null || string.IsNullOrWhiteSpace(camera.CameraCode))
            {
                return Result.Fail(1, "请先选择相机", ResultSource.UI);
            }

            var state = await WithCameraLockAsync(
                () => _cameraRuntime.IsOpen(camera.CameraCode),
                cancellationToken).ConfigureAwait(false);
            if (state.Success && state.Item)
            {
                _openedCameraCodes.Add(camera.CameraCode);
                return Result.Ok("相机已连接", ResultSource.Camera);
            }

            return await OpenSelectedCameraAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task SaveCallRecordAsync(VisionSdkDebugResult result)
        {
            if (result == null || !result.ShouldSaveCallRecord)
            {
                return;
            }

            var frame = LastInputFrame;
            var camera = SelectedCamera;
            var entity = new VisionCallRecordEntity
            {
                CameraId = camera == null ? (int?)null : camera.Id,
                CameraCode = camera == null ? null : camera.CameraCode,
                CallMode = result.OperationKey.ToString(),
                RuntimeName = result.RuntimeName,
                TriggerSourceName = result.TriggerSourceName,
                ModelDeploymentName = result.ModelDeploymentName,
                ImagePath = LastInputImagePath,
                MediaType = frame == null ? null : frame.MediaType,
                ImageBytesLength = frame == null ? 0 : frame.EncodedBytesLength,
                RequestTime = result.RequestTime == default(DateTime) ? DateTime.Now : result.RequestTime,
                ElapsedMs = result.ElapsedMs,
                IsSuccess = result.IsSuccess,
                State = result.State,
                WorkflowRunId = result.WorkflowRunId,
                ResponseJson = result.ResponseJson,
                ErrorMessage = result.ErrorMessage,
                OperatorUserId = UserContext.Instance.CurrentUser == null
                    ? (int?)null
                    : UserContext.Instance.CurrentUser.Id,
                CreateTime = DateTime.Now
            };

            await Task.Run(() => _recordService.Save(entity)).ConfigureAwait(false);
        }

        private VisionSdkDebugResult BuildLocalFailureResult(
            VisionSdkDebugOperationInfo info,
            string message,
            long cameraCaptureEncodeMs)
        {
            var cameraMs = ToIntElapsed(cameraCaptureEncodeMs);
            return new VisionSdkDebugResult
            {
                OperationKey = info.Key,
                OperationName = info.DisplayName,
                RuntimeName = SelectedRuntimeName,
                TriggerSourceName = SelectedTriggerSourceName,
                ModelDeploymentName = SelectedModelDeploymentName,
                RequestTime = DateTime.Now,
                ElapsedMs = cameraMs,
                CameraCaptureEncodeMs = cameraMs,
                SdkInvokeMs = 0,
                ResponseProcessMs = 0,
                TotalElapsedMs = cameraMs,
                IsSuccess = false,
                ErrorMessage = message,
                ResponseJson = "{\"error\":\"" + EscapeJson(message) + "\"}",
                ShouldSaveCallRecord = info.ShouldSaveCallRecord
            };
        }

        private string SaveVisionInputImage(CameraFrame frame)
        {
            var directory = ResolveVisionImageDirectory();
            Directory.CreateDirectory(directory);

            var fileName = string.Format(
                "{0}_{1:yyyyMMdd_HHmmss_fff}_{2}{3}",
                SanitizeFileName(frame.CameraCode),
                frame.Timestamp == default(DateTime) ? DateTime.Now : frame.Timestamp,
                string.IsNullOrWhiteSpace(frame.FrameId) ? Guid.NewGuid().ToString("N") : frame.FrameId,
                ResolveImageExtension(frame.MediaType));

            var path = Path.Combine(directory, fileName);
            File.WriteAllBytes(path, frame.EncodedBytes);
            return path;
        }

        private static bool operationUsesBase64(VisionSdkDebugOperationKey key)
        {
            return key == VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBase64 ||
                   key == VisionSdkDebugOperationKey.RunRuntimeWithImageBase64 ||
                   key == VisionSdkDebugOperationKey.InvokeZeroMqImageBase64 ||
                   key == VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageBase64 ||
                   key == VisionSdkDebugOperationKey.RunModelDeploymentWithImageBase64;
        }

        private static void ApplyTotalTiming(VisionSdkDebugResult result, long cameraCaptureEncodeMs)
        {
            if (result == null)
            {
                return;
            }

            result.CameraCaptureEncodeMs = ToIntElapsed(cameraCaptureEncodeMs);
            result.TotalElapsedMs = ToIntElapsed(
                (long)result.CameraCaptureEncodeMs +
                result.SdkInvokeMs +
                result.ResponseProcessMs);
            result.ElapsedMs = result.TotalElapsedMs;
        }

        private static string ResolveVisionImageDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultVisionImageDirectory);
        }

        private static string ResolveImageExtension(string mediaType)
        {
            if (string.Equals(mediaType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return ".png";
            }

            if (string.Equals(mediaType, "image/bmp", StringComparison.OrdinalIgnoreCase))
            {
                return ".bmp";
            }

            return ".jpg";
        }

        private static string SanitizeFileName(string value)
        {
            var text = string.IsNullOrWhiteSpace(value) ? "CAMERA" : value.Trim();
            foreach (var invalid in Path.GetInvalidFileNameChars())
            {
                text = text.Replace(invalid, '_');
            }

            return text;
        }

        private static string EscapeJson(string value)
        {
            return string.IsNullOrEmpty(value)
                ? string.Empty
                : value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string NormalizeOptional(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static int ToIntElapsed(long elapsedMs)
        {
            if (elapsedMs <= 0)
            {
                return 0;
            }

            return elapsedMs > int.MaxValue ? int.MaxValue : (int)elapsedMs;
        }

        private async Task<TResult> WithCameraLockAsync<TResult>(
            Func<TResult> action,
            CancellationToken cancellationToken)
        {
            await _cameraSync.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return await Task.Run(action, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _cameraSync.Release();
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _cameraRuntime.Dispose();
            _cameraSync.Dispose();
        }
    }
}
