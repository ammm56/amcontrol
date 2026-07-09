namespace AM.Model.Vision
{
    /// <summary>
    /// 视觉 SDK 调试操作键。
    /// 与 Vision.Debug 页面按钮一一对应。
    /// </summary>
    public enum VisionSdkDebugOperationKey
    {
        GetRuntimeHealth = 0,
        GetRuntime = 1,
        ListProjectRuntimes = 2,
        ListRuntimeInstances = 3,
        GetRuntimeEvents = 4,
        StartRuntime = 10,
        StopRuntime = 11,
        RestartRuntime = 12,
        InvokeRuntimeAppResult = 20,
        InvokeRuntimeAppResultWithImageBytes = 22,
        InvokeRuntimeAppResultWithImageBase64 = 23,
        InvokeRuntimeAppResultWithImageFromFile = 24,
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
        InvokeZeroMqBgr24 = 65,

        StartModelDeploymentRuntime = 100,
        StopModelDeploymentRuntime = 101,
        ResetModelDeploymentRuntime = 102,
        WarmupModelDeploymentRuntime = 103,
        GetModelDeploymentRuntimeStatus = 104,
        GetModelDeploymentRuntimeHealth = 105,

        InvokeConfiguredModelDeployment = 120,
        InvokeModelDeploymentWithImageBytes = 121,
        InvokeModelDeploymentWithImageBase64 = 122,
        InvokeModelDeploymentWithImageFromFile = 123
    }
}
