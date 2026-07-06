using AM.Model.Common;
using AM.Model.Interfaces.Vision;
using AM.Model.Vision;
using Amvision.Workflows;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AM.VisionService.Runtime
{
    /// <summary>
    /// 本项目视觉调用服务，WinForms 页面不直接访问 amvision SDK。
    /// </summary>
    public sealed class VisionInvokeService : IVisionInvokeService
    {
        private readonly VisionWorkflowRunnerProvider _runnerProvider;

        public VisionInvokeService()
            : this(VisionWorkflowRunnerProvider.Shared)
        {
        }

        public VisionInvokeService(VisionWorkflowRunnerProvider runnerProvider)
        {
            _runnerProvider = runnerProvider ?? throw new ArgumentNullException(nameof(runnerProvider));
        }

        public async Task<Result<VisionInvokeResult>> InvokeZeroMqImageBytesAsync(
            string triggerSourceName,
            byte[] imageBytes,
            string mediaType = "image/jpeg",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestTime = DateTime.Now;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                ValidateImageBytes(imageBytes);

                var result = await _runnerProvider
                    .GetRunner()
                    .InvokeZeroMqImageBytesAsync(triggerSourceName, imageBytes, NormalizeMediaType(mediaType), cancellationToken)
                    .ConfigureAwait(false);

                stopwatch.Stop();

                return Result<VisionInvokeResult>.OkItem(
                    BuildZeroMqResult(triggerSourceName, result, requestTime, stopwatch.ElapsedMilliseconds),
                    "ZeroMQ Trigger 视觉调用完成",
                    ResultSource.Unknown);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return Result<VisionInvokeResult>.Fail(
                    -1,
                    "ZeroMQ Trigger 视觉调用失败: " + ex.Message,
                    ResultSource.Unknown);
            }
        }

        public async Task<Result<VisionInvokeResult>> InvokeRuntimeAppResultWithImageBytesAsync(
            string runtimeName,
            byte[] imageBytes,
            string mediaType = "image/jpeg",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var requestTime = DateTime.Now;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                ValidateImageBytes(imageBytes);

                var result = await _runnerProvider
                    .GetRunner()
                    .InvokeRuntimeAppResultWithImageBytesAsync(runtimeName, imageBytes, NormalizeMediaType(mediaType), cancellationToken)
                    .ConfigureAwait(false);

                stopwatch.Stop();

                return Result<VisionInvokeResult>.OkItem(
                    BuildRuntimeResult(runtimeName, result, requestTime, stopwatch.ElapsedMilliseconds),
                    "HTTP Runtime AppResult 视觉调用完成",
                    ResultSource.Unknown);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return Result<VisionInvokeResult>.Fail(
                    -1,
                    "HTTP Runtime AppResult 视觉调用失败: " + ex.Message,
                    ResultSource.Unknown);
            }
        }

        private static VisionInvokeResult BuildZeroMqResult(
            string triggerSourceName,
            TriggerResult result,
            DateTime requestTime,
            long elapsedMs)
        {
            return new VisionInvokeResult
            {
                CallMode = VisionCallMode.ZeroMqTrigger.ToString(),
                TriggerSourceName = triggerSourceName,
                RequestTime = requestTime,
                ElapsedMs = ToIntElapsed(elapsedMs),
                IsSuccess = result != null && string.IsNullOrWhiteSpace(result.ErrorMessage),
                State = result == null ? null : result.State,
                WorkflowRunId = result == null ? null : result.WorkflowRunId,
                ResponseJson = result == null ? null : JsonSerializer.Serialize(result),
                ErrorMessage = result == null ? "TriggerResult 为空" : result.ErrorMessage
            };
        }

        private static VisionInvokeResult BuildRuntimeResult(
            string runtimeName,
            WorkflowAppResultResponse result,
            DateTime requestTime,
            long elapsedMs)
        {
            return new VisionInvokeResult
            {
                CallMode = VisionCallMode.HttpRuntimeAppResult.ToString(),
                RuntimeName = runtimeName,
                RequestTime = requestTime,
                ElapsedMs = ToIntElapsed(elapsedMs),
                IsSuccess = true,
                State = "Completed",
                ResponseJson = result == null ? null : result.BodyJson.GetRawText()
            };
        }

        private static int ToIntElapsed(long elapsedMs)
        {
            return elapsedMs > int.MaxValue ? int.MaxValue : (int)elapsedMs;
        }

        private static string NormalizeMediaType(string mediaType)
        {
            return string.IsNullOrWhiteSpace(mediaType) ? "image/jpeg" : mediaType.Trim();
        }

        private static void ValidateImageBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("图片 bytes 不能为空", nameof(imageBytes));
            }
        }
    }
}
