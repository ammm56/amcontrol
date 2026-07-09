using AM.Model.Common;
using AM.Model.Vision;
using Amvision.Workflows;
using Amvision.Workflows.Console;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AM.VisionService.Runtime
{
    /// <summary>
    /// 视觉 SDK 调试服务。
    /// 仅面向 Vision.Debug 页面按钮调用，不承载生产流程业务语义。
    /// </summary>
    public sealed class VisionSdkDebugService
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly VisionWorkflowRunnerProvider _runnerProvider;

        public VisionSdkDebugService()
            : this(VisionWorkflowRunnerProvider.Shared)
        {
        }

        public VisionSdkDebugService(VisionWorkflowRunnerProvider runnerProvider)
        {
            _runnerProvider = runnerProvider ?? throw new ArgumentNullException("runnerProvider");
        }

        public async Task<Result<VisionSdkDebugResult>> ExecuteAsync(
            VisionSdkDebugRequest request,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestTime = DateTime.Now;
            var stopwatch = Stopwatch.StartNew();
            var invokeStopwatch = new Stopwatch();
            var processStopwatch = new Stopwatch();
            long sdkInvokeMs = 0L;
            long responseProcessMs = 0L;

            try
            {
                ValidateRequest(request);

                var runner = _runnerProvider.GetRunner();
                invokeStopwatch.Start();
                object response = await ExecuteCoreAsync(runner, request, cancellationToken).ConfigureAwait(false);
                invokeStopwatch.Stop();
                sdkInvokeMs = invokeStopwatch.ElapsedMilliseconds;

                processStopwatch.Start();
                var result = BuildResult(request, response, requestTime);
                processStopwatch.Stop();
                responseProcessMs = processStopwatch.ElapsedMilliseconds;
                stopwatch.Stop();

                ApplyTiming(result, 0L, sdkInvokeMs, responseProcessMs);
                return Result<VisionSdkDebugResult>.OkItem(
                    result,
                    result.OperationName + " 调用完成",
                    ResultSource.Unknown);
            }
            catch (Exception ex)
            {
                if (invokeStopwatch.IsRunning)
                {
                    invokeStopwatch.Stop();
                }

                if (processStopwatch.IsRunning)
                {
                    processStopwatch.Stop();
                }

                stopwatch.Stop();
                sdkInvokeMs = invokeStopwatch.ElapsedMilliseconds;
                responseProcessMs = processStopwatch.ElapsedMilliseconds;

                var failed = BuildFailedResult(request, ex, requestTime);
                ApplyTiming(failed, 0L, sdkInvokeMs, responseProcessMs);
                if (failed.ElapsedMs <= 0)
                {
                    failed.ElapsedMs = ToIntElapsed(stopwatch.ElapsedMilliseconds);
                    failed.TotalElapsedMs = failed.ElapsedMs;
                }

                return new Result<VisionSdkDebugResult>(
                    false,
                    -1,
                    failed.OperationName + " 调用失败: " + ex.Message,
                    ResultSource.Unknown)
                {
                    Item = failed
                };
            }
        }

        private static async Task<object> ExecuteCoreAsync(
            WorkflowOperationRunner runner,
            VisionSdkDebugRequest request,
            CancellationToken cancellationToken)
        {
            switch (request.OperationKey)
            {
                case VisionSdkDebugOperationKey.GetRuntimeHealth:
                    return await runner.GetRuntimeHealthAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.GetRuntime:
                    return await runner.GetRuntimeAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.ListProjectRuntimes:
                    return await runner.ListProjectRuntimesAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.ListRuntimeInstances:
                    return await runner.ListRuntimeInstancesAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.GetRuntimeEvents:
                    return await runner.GetRuntimeEventsAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);

                case VisionSdkDebugOperationKey.StartRuntime:
                    return await runner.StartRuntimeAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.StopRuntime:
                    return await runner.StopRuntimeAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.RestartRuntime:
                    return await runner.RestartRuntimeAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);

                case VisionSdkDebugOperationKey.InvokeRuntimeAppResult:
                    return await runner.InvokeRuntimeAppResultAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBytes:
                    return await runner.InvokeRuntimeAppResultWithImageBytesAsync(
                        request.RuntimeName,
                        request.ImageBytes,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBase64:
                    return await runner.InvokeRuntimeAppResultWithImageBase64Async(
                        request.RuntimeName,
                        request.ImageBase64,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageFromFile:
                    return await runner.InvokeRuntimeAppResultWithImageFromFileAsync(
                        request.RuntimeName,
                        request.ImagePath,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);

                case VisionSdkDebugOperationKey.ListTriggerSources:
                    return await runner.ListTriggerSourcesAsync(request.RuntimeName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.GetTriggerSource:
                    return await runner.GetTriggerSourceAsync(request.TriggerSourceName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.GetTriggerSourceHealth:
                    return await runner.GetTriggerSourceHealthAsync(request.TriggerSourceName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.EnableTriggerSource:
                    return await runner.EnableTriggerSourceAsync(request.TriggerSourceName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.DisableTriggerSource:
                    return await runner.DisableTriggerSourceAsync(request.TriggerSourceName, cancellationToken).ConfigureAwait(false);

                case VisionSdkDebugOperationKey.InvokeZeroMqEvent:
                    return await runner.InvokeZeroMqEventAsync(request.TriggerSourceName, null, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeZeroMqConfiguredImage:
                    return await runner.InvokeZeroMqConfiguredImageAsync(request.TriggerSourceName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeZeroMqBgr24:
                    return await runner.InvokeZeroMqBgr24Async(
                        request.TriggerSourceName,
                        request.Bgr24Bytes,
                        request.Bgr24Width,
                        request.Bgr24Height,
                        cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeZeroMqImageBytes:
                    return await runner.InvokeZeroMqImageBytesAsync(
                        request.TriggerSourceName,
                        request.ImageBytes,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeZeroMqImageBase64:
                    return await runner.InvokeZeroMqImageBase64Async(
                        request.TriggerSourceName,
                        request.ImageBase64,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeZeroMqImageFromFile:
                    return await runner.InvokeZeroMqImageFromFileAsync(
                        request.TriggerSourceName,
                        request.ImagePath,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);

                case VisionSdkDebugOperationKey.StartModelDeploymentRuntime:
                    return await runner.StartModelDeploymentRuntimeAsync(request.ModelDeploymentName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.StopModelDeploymentRuntime:
                    return await runner.StopModelDeploymentRuntimeAsync(request.ModelDeploymentName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.ResetModelDeploymentRuntime:
                    return await runner.ResetModelDeploymentRuntimeAsync(request.ModelDeploymentName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.WarmupModelDeploymentRuntime:
                    return await runner.WarmupModelDeploymentRuntimeAsync(request.ModelDeploymentName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.GetModelDeploymentRuntimeStatus:
                    return await runner.GetModelDeploymentRuntimeStatusAsync(request.ModelDeploymentName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.GetModelDeploymentRuntimeHealth:
                    return await runner.GetModelDeploymentRuntimeHealthAsync(request.ModelDeploymentName, cancellationToken).ConfigureAwait(false);

                case VisionSdkDebugOperationKey.InvokeConfiguredModelDeployment:
                    return await runner.InvokeConfiguredModelDeploymentAsync(request.ModelDeploymentName, cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageBytes:
                    return await runner.InvokeModelDeploymentWithImageBytesAsync(
                        request.ModelDeploymentName,
                        request.ImageBytes,
                        null,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageBase64:
                    return await runner.InvokeModelDeploymentWithImageBase64Async(
                        request.ModelDeploymentName,
                        request.ImageBase64,
                        cancellationToken).ConfigureAwait(false);
                case VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageFromFile:
                    return await runner.InvokeModelDeploymentWithImageFromFileAsync(
                        request.ModelDeploymentName,
                        request.ImagePath,
                        NormalizeMediaType(request.MediaType),
                        cancellationToken).ConfigureAwait(false);
                default:
                    throw new ArgumentOutOfRangeException("OperationKey", request.OperationKey, "未知视觉 SDK 调试操作。");
            }
        }

        private static void ValidateRequest(VisionSdkDebugRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var info = VisionSdkDebugOperationCatalog.Get(request.OperationKey);
            if (info.RequiresRuntime && string.IsNullOrWhiteSpace(request.RuntimeName))
            {
                throw new ArgumentException("Runtime Key 不能为空", "RuntimeName");
            }

            if (info.RequiresTriggerSource && string.IsNullOrWhiteSpace(request.TriggerSourceName))
            {
                throw new ArgumentException("Trigger Key 不能为空", "TriggerSourceName");
            }

            if (info.RequiresModelDeployment && string.IsNullOrWhiteSpace(request.ModelDeploymentName))
            {
                throw new ArgumentException("Model Deployment Key 不能为空", "ModelDeploymentName");
            }

            if (info.UsesCameraImage)
            {
                if (UsesBgr24Image(request.OperationKey))
                {
                    if (request.Bgr24Bytes == null || request.Bgr24Bytes.Length == 0)
                    {
                        throw new ArgumentException("BGR24 bytes 不能为空", "Bgr24Bytes");
                    }

                    if (request.Bgr24Width <= 0)
                    {
                        throw new ArgumentException("BGR24 宽度必须大于 0", "Bgr24Width");
                    }

                    if (request.Bgr24Height <= 0)
                    {
                        throw new ArgumentException("BGR24 高度必须大于 0", "Bgr24Height");
                    }
                }
                else if (UsesBase64Image(request.OperationKey))
                {
                    if (string.IsNullOrWhiteSpace(request.ImageBase64))
                    {
                        throw new ArgumentException("图片 base64 不能为空", "ImageBase64");
                    }
                }
                else if (info.UsesTemporaryImageFile)
                {
                    if (string.IsNullOrWhiteSpace(request.ImagePath))
                    {
                        throw new ArgumentException("图片路径不能为空", "ImagePath");
                    }
                }
                else if (request.ImageBytes == null || request.ImageBytes.Length == 0)
                {
                    throw new ArgumentException("图片 bytes 不能为空", "ImageBytes");
                }
            }
        }

        private static VisionSdkDebugResult BuildResult(
            VisionSdkDebugRequest request,
            object response,
            DateTime requestTime)
        {
            var info = VisionSdkDebugOperationCatalog.Get(request.OperationKey);
            var result = new VisionSdkDebugResult
            {
                OperationKey = request.OperationKey,
                OperationName = info.DisplayName,
                RuntimeName = request.RuntimeName,
                TriggerSourceName = request.TriggerSourceName,
                ModelDeploymentName = request.ModelDeploymentName,
                RequestTime = requestTime,
                IsSuccess = true,
                ResponseJson = SerializeResponse(response),
                ShouldSaveCallRecord = info.ShouldSaveCallRecord
            };

            FillKnownFields(result, response);
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                result.IsSuccess = false;
            }

            return result;
        }

        private static VisionSdkDebugResult BuildFailedResult(
            VisionSdkDebugRequest request,
            Exception ex,
            DateTime requestTime)
        {
            var operationKey = request == null
                ? VisionSdkDebugOperationKey.GetRuntimeHealth
                : request.OperationKey;
            var info = VisionSdkDebugOperationCatalog.Get(operationKey);

            return new VisionSdkDebugResult
            {
                OperationKey = operationKey,
                OperationName = info.DisplayName,
                RuntimeName = request == null ? null : request.RuntimeName,
                TriggerSourceName = request == null ? null : request.TriggerSourceName,
                ModelDeploymentName = request == null ? null : request.ModelDeploymentName,
                RequestTime = requestTime,
                IsSuccess = false,
                ErrorMessage = ex == null ? null : ex.Message,
                ResponseJson = ex == null ? null : SerializeException(ex),
                ShouldSaveCallRecord = info.ShouldSaveCallRecord
            };
        }

        private static void FillKnownFields(VisionSdkDebugResult result, object response)
        {
            var triggerResult = response as TriggerResult;
            if (triggerResult != null)
            {
                result.State = triggerResult.State;
                result.WorkflowRunId = triggerResult.WorkflowRunId;
                result.ErrorMessage = triggerResult.ErrorMessage;
                return;
            }

            var run = response as WorkflowRunResponse;
            if (run != null)
            {
                result.State = run.State;
                result.WorkflowRunId = run.WorkflowRunId;
                result.ErrorMessage = run.ErrorMessage;
                return;
            }

            var runtime = response as WorkflowAppRuntimeResponse;
            if (runtime != null)
            {
                result.State = string.IsNullOrWhiteSpace(runtime.ObservedState)
                    ? runtime.DesiredState
                    : runtime.ObservedState;
                return;
            }

            var trigger = response as WorkflowTriggerSourceResponse;
            if (trigger != null)
            {
                result.State = string.IsNullOrWhiteSpace(trigger.ObservedState)
                    ? trigger.DesiredState
                    : trigger.ObservedState;
                return;
            }

            var triggerHealth = response as WorkflowTriggerSourceHealthResponse;
            if (triggerHealth != null)
            {
                result.State = string.IsNullOrWhiteSpace(triggerHealth.ObservedState)
                    ? triggerHealth.DesiredState
                    : triggerHealth.ObservedState;
                return;
            }

            var modelStatus = response as ModelDeploymentRuntimeStatusResponse;
            if (modelStatus != null)
            {
                result.State = string.IsNullOrWhiteSpace(modelStatus.ProcessState)
                    ? modelStatus.DesiredState
                    : modelStatus.ProcessState;
                result.WorkflowRunId = modelStatus.DeploymentInstanceId;
                result.ErrorMessage = modelStatus.LastError;
                return;
            }

            var modelWarmup = response as ModelDeploymentRuntimeWarmupResponse;
            if (modelWarmup != null)
            {
                result.State = modelWarmup.Status;
                result.WorkflowRunId = modelWarmup.DeploymentInstanceId;
                return;
            }

            var modelInference = response as ModelDeploymentInferenceResponse;
            if (modelInference != null)
            {
                result.State = "succeeded";
                result.WorkflowRunId = string.IsNullOrWhiteSpace(modelInference.InferenceTaskId)
                    ? modelInference.RequestId
                    : modelInference.InferenceTaskId;
            }
        }

        private static void ApplyTiming(
            VisionSdkDebugResult result,
            long cameraCaptureEncodeMs,
            long sdkInvokeMs,
            long responseProcessMs)
        {
            if (result == null)
            {
                return;
            }

            result.CameraCaptureEncodeMs = ToIntElapsed(cameraCaptureEncodeMs);
            result.SdkInvokeMs = ToIntElapsed(sdkInvokeMs);
            result.ResponseProcessMs = ToIntElapsed(responseProcessMs);
            result.TotalElapsedMs = ToIntElapsed(cameraCaptureEncodeMs + sdkInvokeMs + responseProcessMs);
            result.ElapsedMs = result.TotalElapsedMs;
        }

        private static string SerializeResponse(object response)
        {
            if (response == null)
            {
                return "null";
            }

            var appResult = response as WorkflowAppResultResponse;
            if (appResult != null)
            {
                return appResult.BodyJson.GetRawText();
            }

            return JsonSerializer.Serialize(response, response.GetType(), JsonOptions);
        }

        private static string SerializeException(Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                error = ex.Message,
                exception_type = ex.GetType().FullName,
                stack_trace = ex.StackTrace
            }, JsonOptions);
        }

        private static string NormalizeMediaType(string mediaType)
        {
            return string.IsNullOrWhiteSpace(mediaType) ? "image/jpeg" : mediaType.Trim();
        }

        private static bool UsesBgr24Image(VisionSdkDebugOperationKey key)
        {
            return key == VisionSdkDebugOperationKey.InvokeZeroMqBgr24;
        }

        private static bool UsesBase64Image(VisionSdkDebugOperationKey key)
        {
            return key == VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBase64 ||
                   key == VisionSdkDebugOperationKey.InvokeZeroMqImageBase64 ||
                   key == VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageBase64;
        }

        private static int ToIntElapsed(long elapsedMs)
        {
            return elapsedMs > int.MaxValue ? int.MaxValue : (int)elapsedMs;
        }
    }
}
