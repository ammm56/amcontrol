namespace AM.Model.Vision
{
    /// <summary>
    /// 视觉 SDK 调试操作键。
    /// 与 Vision.Debug 页面按钮一一对应。
    /// </summary>
    public enum VisionSdkDebugOperationKey
    {
        GetSystemConfig = -10,

        GetRuntimeHealth = 0,
        GetRuntime = 1,
        ListProjectRuntimes = 2,
        ListRuntimeInstances = 3,
        GetRuntimeEvents = 4,
        CheckRuntimeFlow = 5,
        StartRuntime = 10,
        StopRuntime = 11,
        RestartRuntime = 12,
        InvokeRuntimeAppResult = 20,
        RunRuntime = 21,
        InvokeRuntimeAppResultWithImageBytes = 22,
        InvokeRuntimeAppResultWithImageBase64 = 23,
        InvokeRuntimeAppResultWithImageFromFile = 24,
        RunRuntimeWithImageBytes = 25,
        RunRuntimeWithImageBase64 = 26,
        RunRuntimeWithImageFromFile = 27,
        ListTriggerSources = 40,
        GetTriggerSource = 41,
        GetTriggerSourceHealth = 42,
        EnableTriggerSource = 43,
        DisableTriggerSource = 44,
        InvokeZeroMqEvent = 60,
        InvokeZeroMqConfiguredImage = 61,
        InvokeZeroMqImageBytes = 62,
        InvokeZeroMqImageBase64 = 63,
        InvokeZeroMqImageFromFile = 64,

        StartModelDeploymentRuntime = 100,
        StopModelDeploymentRuntime = 101,
        ResetModelDeploymentRuntime = 102,
        WarmupModelDeploymentRuntime = 103,
        GetModelDeploymentRuntimeStatus = 104,
        GetModelDeploymentRuntimeHealth = 105,

        InvokeConfiguredModelDeployment = 120,
        InvokeModelDeploymentWithImageBytes = 121,
        InvokeModelDeploymentWithImageBase64 = 122,
        InvokeModelDeploymentWithImageFromFile = 123,
        InvokeModelDeploymentWithInputUri = 124,
        InvokeModelDeploymentWithInputFileId = 125,

        RunConfiguredModelDeployment = 140,
        RunModelDeploymentWithImageBytes = 141,
        RunModelDeploymentWithImageBase64 = 142,
        RunModelDeploymentWithImageFromFile = 143,
        RunModelDeploymentWithInputUri = 144,
        RunModelDeploymentWithInputFileId = 145,
        GetModelInferenceTask = 146,
        GetModelInferenceTaskResult = 147
    }
}
