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
                D(VisionSdkDebugOperationKey.GetRuntimeHealth, "Runtime Health", "Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetRuntime, "Get Runtime", "Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.ListProjectRuntimes, "List Project Runtimes", "Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.ListRuntimeInstances, "List Instances", "Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetRuntimeEvents, "Runtime Events", "Runtime 查询", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.CheckRuntimeFlow, "Check Flow", "Runtime 查询", true, false, false, false, false, false),

                D(VisionSdkDebugOperationKey.StartRuntime, "Start Runtime", "Runtime 控制", true, false, false, false, false, true),
                D(VisionSdkDebugOperationKey.StopRuntime, "Stop Runtime", "Runtime 控制", true, false, false, false, false, true),
                D(VisionSdkDebugOperationKey.RestartRuntime, "Restart Runtime", "Runtime 控制", true, false, false, false, false, true),

                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResult, "Invoke AppResult", "Runtime 调用", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.RunRuntime, "Run Runtime", "Runtime 调用", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBytes, "Invoke Image Bytes", "Runtime 调用", true, false, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageBase64, "Invoke Image Base64", "Runtime 调用", true, false, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeRuntimeAppResultWithImageFromFile, "Invoke Image File", "Runtime 调用", true, false, true, true, true, false),
                D(VisionSdkDebugOperationKey.RunRuntimeWithImageBytes, "Run Image Bytes", "Runtime 调用", true, false, true, false, true, false),
                D(VisionSdkDebugOperationKey.RunRuntimeWithImageBase64, "Run Image Base64", "Runtime 调用", true, false, true, false, true, false),
                D(VisionSdkDebugOperationKey.RunRuntimeWithImageFromFile, "Run Image File", "Runtime 调用", true, false, true, true, true, false),

                D(VisionSdkDebugOperationKey.ListTriggerSources, "List TriggerSources", "Trigger 查询/控制", true, false, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetTriggerSource, "Get TriggerSource", "Trigger 查询/控制", false, true, false, false, false, false),
                D(VisionSdkDebugOperationKey.GetTriggerSourceHealth, "Trigger Health", "Trigger 查询/控制", false, true, false, false, false, false),
                D(VisionSdkDebugOperationKey.EnableTriggerSource, "Enable Trigger", "Trigger 查询/控制", false, true, false, false, false, true),
                D(VisionSdkDebugOperationKey.DisableTriggerSource, "Disable Trigger", "Trigger 查询/控制", false, true, false, false, false, true),

                D(VisionSdkDebugOperationKey.InvokeZeroMqEvent, "ZeroMQ Event", "ZeroMQ 调用", false, true, false, false, false, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqConfiguredImage, "ZeroMQ Config Image", "ZeroMQ 调用", false, true, false, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqImageBytes, "ZeroMQ Image Bytes", "ZeroMQ 调用", false, true, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqImageBase64, "ZeroMQ Image Base64", "ZeroMQ 调用", false, true, true, false, true, false),
                D(VisionSdkDebugOperationKey.InvokeZeroMqImageFromFile, "ZeroMQ Image File", "ZeroMQ 调用", false, true, true, true, true, false)
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
                usesCameraImage,
                usesTemporaryImageFile,
                shouldSaveCallRecord,
                requiresConfirm);
        }
    }
}
