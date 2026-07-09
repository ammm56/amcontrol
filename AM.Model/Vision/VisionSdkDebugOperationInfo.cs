using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.Model.Vision
{
    /// <summary>
    /// 视觉 SDK 调试操作元信息。
    /// </summary>
    public sealed class VisionSdkDebugOperationInfo
    {
        public VisionSdkDebugOperationInfo(
            VisionSdkDebugOperationKey key,
            string displayName,
            string groupName,
            bool requiresRuntime,
            bool requiresTriggerSource,
            bool requiresModelDeployment,
            bool usesCameraImage,
            bool usesTemporaryImageFile,
            bool shouldSaveCallRecord,
            bool requiresConfirm)
        {
            Key = key;
            DisplayName = displayName;
            GroupName = groupName;
            RequiresRuntime = requiresRuntime;
            RequiresTriggerSource = requiresTriggerSource;
            RequiresModelDeployment = requiresModelDeployment;
            UsesCameraImage = usesCameraImage;
            UsesTemporaryImageFile = usesTemporaryImageFile;
            ShouldSaveCallRecord = shouldSaveCallRecord;
            RequiresConfirm = requiresConfirm;
        }

        public VisionSdkDebugOperationKey Key { get; private set; }

        public string DisplayName { get; private set; }

        public string GroupName { get; private set; }

        public bool RequiresRuntime { get; private set; }

        public bool RequiresTriggerSource { get; private set; }

        public bool RequiresModelDeployment { get; private set; }

        public bool UsesCameraImage { get; private set; }

        public bool UsesTemporaryImageFile { get; private set; }

        public bool ShouldSaveCallRecord { get; private set; }

        public bool RequiresConfirm { get; private set; }
    }

    /// <summary>
    /// 视觉 SDK 调试操作目录。
    /// </summary>
    public static class VisionSdkDebugOperationCatalog
    {
        private static readonly List<VisionSdkDebugOperationInfo> ItemsInternal =
            new List<VisionSdkDebugOperationInfo>
            {
                M(VisionSdkDebugOperationKey.GetModelDeploymentRuntimeStatus, "Model Runtime Status", "模型部署 Runtime", false, false, false, false, false, false),
                M(VisionSdkDebugOperationKey.GetModelDeploymentRuntimeHealth, "Model Runtime Health", "模型部署 Runtime", false, false, false, false, false, false),
                M(VisionSdkDebugOperationKey.StartModelDeploymentRuntime, "Start Model Runtime", "模型部署 Runtime", false, false, false, false, false, true),
                M(VisionSdkDebugOperationKey.StopModelDeploymentRuntime, "Stop Model Runtime", "模型部署 Runtime", false, false, false, false, false, true),
                M(VisionSdkDebugOperationKey.ResetModelDeploymentRuntime, "Reset Model Runtime", "模型部署 Runtime", false, false, false, false, false, true),
                M(VisionSdkDebugOperationKey.WarmupModelDeploymentRuntime, "Warmup Model Runtime", "模型部署 Runtime", false, false, false, false, false, true),

                M(VisionSdkDebugOperationKey.InvokeConfiguredModelDeployment, "Invoke Model Config", "模型部署同步调用", false, false, false, false, true, false),
                M(VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageBytes, "Invoke Model Bytes", "模型部署同步调用", false, false, true, false, true, false),
                M(VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageBase64, "Invoke Model Base64", "模型部署同步调用", false, false, true, false, true, false),
                M(VisionSdkDebugOperationKey.InvokeModelDeploymentWithImageFromFile, "Invoke Model File", "模型部署同步调用", false, false, true, true, true, false),

                D(VisionSdkDebugOperationKey.GetRuntimeHealth, "Runtime Health", "Workflow App Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetRuntime, "Get Runtime", "Workflow App Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.ListProjectRuntimes, "List Project Runtimes", "Workflow App Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.ListRuntimeInstances, "List Instances", "Workflow App Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetRuntimeEvents, "Runtime Events", "Workflow App Runtime 查询", true, false, false, false, false, false),

                D(VisionSdkDebugOperationKey.StartRuntime, "Start Runtime", "Workflow App Runtime 控制", true, false, false, false, false, true),
                D(VisionSdkDebugOperationKey.StopRuntime, "Stop Runtime", "Workflow App Runtime 控制", true, false, false, false, false, true),
                D(VisionSdkDebugOperationKey.RestartRuntime, "Restart Runtime", "Workflow App Runtime 控制", true, false, false, false, false, true),

                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResult, "Invoke AppResult", "Workflow App Runtime 调用", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBytes, "Invoke Image Bytes", "Workflow App Runtime 调用", true, false, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBase64, "Invoke Image Base64", "Workflow App Runtime 调用", true, false, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageFromFile, "Invoke Image File", "Workflow App Runtime 调用", true, false, true, true, true, false),

                D(VisionSdkDebugOperationKey.ListTriggerSources, "List TriggerSources", "Trigger 查询/控制", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetTriggerSource, "Get TriggerSource", "Trigger 查询/控制", false, true, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetTriggerSourceHealth, "Trigger Health", "Trigger 查询/控制", false, true, false, false, false, false),
                D(VisionSdkDebugOperationKey.EnableTriggerSource, "Enable Trigger", "Trigger 查询/控制", false, true, false, false, false, true),
                D(VisionSdkDebugOperationKey.DisableTriggerSource, "Disable Trigger", "Trigger 查询/控制", false, true, false, false, false, true),

                D(VisionSdkDebugOperationKey.InvokeZeroMqEvent, "ZeroMQ Event", "Trigger 调用", false, true, false, false, false, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqConfiguredImage, "ZeroMQ Config Image", "Trigger 调用", false, true, false, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqBgr24, "ZeroMQ BGR24", "Trigger 调用", false, true, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqImageBytes, "ZeroMQ Image Bytes", "Trigger 调用", false, true, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqImageBase64, "ZeroMQ Image Base64", "Trigger 调用", false, true, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqImageFromFile, "ZeroMQ Image File", "Trigger 调用", false, true, true, true, true, false)
            };

        public static IReadOnlyList<VisionSdkDebugOperationInfo> All
        {
            get { return ItemsInternal; }
        }

        public static VisionSdkDebugOperationInfo Get(VisionSdkDebugOperationKey key)
        {
            var item = ItemsInternal.FirstOrDefault(x => x.Key == key);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("key", key, "未知视觉 SDK 调试操作。");
            }

            return item;
        }

        private static VisionSdkDebugOperationInfo D(
            VisionSdkDebugOperationKey key,
            string displayName,
            string groupName,
            bool requiresRuntime,
            bool requiresTriggerSource,
            bool usesCameraImage,
            bool usesTemporaryImageFile,
            bool shouldSaveCallRecord,
            bool requiresConfirm)
        {
            return new VisionSdkDebugOperationInfo(
                key,
                displayName,
                groupName,
                requiresRuntime,
                requiresTriggerSource,
                false,
                usesCameraImage,
                usesTemporaryImageFile,
                shouldSaveCallRecord,
                requiresConfirm);
        }

        private static VisionSdkDebugOperationInfo M(
            VisionSdkDebugOperationKey key,
            string displayName,
            string groupName,
            bool requiresRuntime,
            bool requiresTriggerSource,
            bool usesCameraImage,
            bool usesTemporaryImageFile,
            bool shouldSaveCallRecord,
            bool requiresConfirm)
        {
            return new VisionSdkDebugOperationInfo(
                key,
                displayName,
                groupName,
                requiresRuntime,
                requiresTriggerSource,
                true,
                usesCameraImage,
                usesTemporaryImageFile,
                shouldSaveCallRecord,
                requiresConfirm);
        }
    }
}
