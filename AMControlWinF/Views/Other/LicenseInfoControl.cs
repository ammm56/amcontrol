using AM.Core.Context;
using AM.DBService.Services.System;
using AM.Model.Common;
using AM.Model.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AMControlWinF.Views.Other
{
    /// <summary>
    /// 许可证面板。
    /// 左侧展示授权申请信息与申请入口，右侧展示授权结果、时间期限与授权范围。
    /// </summary>
    public partial class LicenseInfoControl : UserControl
    {
        private readonly HardwareInfoCollector _hardwareInfoCollector;
        private readonly DeviceLicenseApplyClient _deviceLicenseApplyClient;

        private string _lastApplyMessage;

        public LicenseInfoControl()
        {
            InitializeComponent();

            _hardwareInfoCollector = new HardwareInfoCollector();
            _deviceLicenseApplyClient = new DeviceLicenseApplyClient();
            _lastApplyMessage = string.Empty;

            InitializeEnvironmentInputs();
            ApplyLanguage();
            RefreshLicenseView();
        }

        private void InitializeEnvironmentInputs()
        {
            selectNetworkMode.Items.AddRange(new object[] { "Online", "Offline" });
            selectNetworkMode.SelectedValue = "Online";

            buttonApplyLicense.Click += ButtonApplyLicense_Click;
            buttonRefresh.Click += ButtonRefresh_Click;
        }

        private async void ButtonApplyLicense_Click(object sender, EventArgs e)
        {
            SetButtonsEnabled(false);
            try
            {
                Result<LicenseApplyResponse> result = await _deviceLicenseApplyClient
                    .ApplyCurrentDeviceAsync(
                        inputSiteCode.Text,
                        inputCustomerCode.Text,
                        GetSelectedNetworkMode())
                    .ConfigureAwait(true);

                _lastApplyMessage = string.IsNullOrWhiteSpace(result.Message)
                    ? (IsEnglishLanguage() ? "Completed." : "已完成。")
                    : result.Message;

                RefreshLicenseView();
            }
            finally
            {
                SetButtonsEnabled(true);
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshLicenseView();
        }

        private void SetButtonsEnabled(bool enabled)
        {
            buttonApplyLicense.Enabled = enabled;
            buttonRefresh.Enabled = enabled;
        }

        private void RefreshLicenseView()
        {
            bool isEn = IsEnglishLanguage();
            labelSoftwareValue.Text = BuildSoftwareInfoText(isEn);
            labelHardwareValue.Text = BuildHardwareInfoText(isEn);
            labelResultValue.Text = BuildResultText(isEn);
            labelTimeValue.Text = BuildTimeText(isEn);
            labelScopeValue.Text = BuildScopeText(isEn);
        }

        private void ApplyLanguage()
        {
            bool isEn = IsEnglishLanguage();

            labelLeftTitle.Text = isEn ? "License Request" : "授权申请";
            labelSoftwareTitle.Text = isEn ? "Software identity" : "软件身份";
            labelHardwareTitle.Text = isEn ? "Device binding" : "设备绑定";
            labelEnvironmentTitle.Text = isEn ? "Apply environment" : "申请环境";
            labelSiteCode.Text = isEn ? "Site code" : "站点编码";
            labelCustomerCode.Text = isEn ? "Customer code" : "客户编码";
            labelNetworkMode.Text = isEn ? "Network mode" : "联网模式";
            inputSiteCode.PlaceholderText = isEn ? "Optional site code" : "可选站点编码";
            inputCustomerCode.PlaceholderText = isEn ? "Optional customer code" : "可选客户编码";
            buttonRefresh.Text = isEn ? "Refresh" : "刷新";
            buttonApplyLicense.Text = isEn ? "Apply License" : "申请授权";
            labelRightTitle.Text = isEn ? "License Result" : "授权结果";
            labelTimeTitle.Text = isEn ? "Validity window" : "时间期限";
            labelScopeTitle.Text = isEn ? "Authorized scope" : "授权范围";
        }

        private string BuildSoftwareInfoText(bool isEn)
        {
            var setting = ConfigContext.Instance.Config.Setting;

            return string.Join(Environment.NewLine, new[]
            {
                (isEn ? "App Name: " : "应用名称：") + BackendServiceConfigHelper.GetDesktopAppName(),
                (isEn ? "App Code: " : "应用编码：") + BackendServiceConfigHelper.GetDesktopAppCode(),
                (isEn ? "Category: " : "应用分类：") + BackendServiceConfigHelper.GetDesktopAppCategory(),
                (isEn ? "Edition: " : "版本版型：") + SafeValue(BackendServiceConfigHelper.GetDesktopAppEdition(), isEn),
                (isEn ? "Version: " : "程序集版本：") + AM.Tools.Tools.GetAppVersionText(),
                (isEn ? "UI Platform: " : "界面平台：") + BackendServiceConfigHelper.GetDesktopAppUiPlatform(),
                (isEn ? "Framework: " : "目标框架：") + BackendServiceConfigHelper.GetDesktopAppTargetFramework(),
                (isEn ? "Vendor: " : "供应商：") + BackendServiceConfigHelper.GetDesktopAppVendor(),
                (isEn ? "Backend URL: " : "后端地址：") + SafeValue(setting.BackendServiceUrl, isEn)
            });
        }

        private string BuildHardwareInfoText(bool isEn)
        {
            Result<DeviceHardwareInfo> hardwareResult = _hardwareInfoCollector.CollectCurrent();
            DeviceHardwareInfo hardware = hardwareResult.Item ?? new DeviceHardwareInfo();

            return string.Join(Environment.NewLine, new[]
            {
                (isEn ? "ClientId: " : "客户端ID：") + SafeValue(hardware.ClientId, isEn),
                (isEn ? "MachineCode: " : "设备编码：") + SafeValue(hardware.MachineCode, isEn),
                (isEn ? "MachineName: " : "设备名称：") + SafeValue(hardware.MachineName, isEn),
                (isEn ? "MachineModel: " : "设备型号：") + SafeValue(hardware.MachineModel, isEn),
                (isEn ? "OS Version: " : "操作系统：") + SafeValue(hardware.OsVersion, isEn),
                (isEn ? "CPU Id: " : "CPU标识：") + SafeValue(hardware.CpuId, isEn),
                (isEn ? "BIOS: " : "BIOS序列号：") + SafeValue(hardware.BiosSerialNumber, isEn),
                (isEn ? "Mainboard: " : "主板序列号：") + SafeValue(hardware.MainboardSerialNumber, isEn),
                (isEn ? "Disk: " : "磁盘序列号：") + SafeValue(hardware.DiskSerialNumber, isEn),
                (isEn ? "MAC: " : "网卡MAC：") + SafeValue(hardware.MacAddress, isEn)
            });
        }

        private string BuildResultText(bool isEn)
        {
            DeviceLicenseState state = LicenseRuntimeContext.Instance.Current;
            LicenseValidationResult validation = state == null ? null : state.ValidationResult;

            string statusText;
            if (state == null || !state.HasLicense)
                statusText = isEn ? "No license" : "未加载授权";
            else if (state.IsValid)
                statusText = isEn ? "Valid" : "有效";
            else if (state.IsExpired)
                statusText = isEn ? "Expired" : "已过期";
            else
                statusText = isEn ? "Invalid" : "无效";

            string lastApply = string.IsNullOrWhiteSpace(_lastApplyMessage)
                ? (isEn ? "N/A" : "暂无")
                : _lastApplyMessage;

            return string.Join(Environment.NewLine, new[]
            {
                (isEn ? "Status: " : "授权状态：") + statusText,
                (isEn ? "LicenseId: " : "授权ID：") + SafeValue(state == null ? string.Empty : state.LicenseId, isEn),
                (isEn ? "Message: " : "说明信息：") + SafeValue(state == null ? string.Empty : state.Message, isEn),
                (isEn ? "Signature valid: " : "签名校验：") + BoolText(state != null && state.IsSignatureValid, isEn),
                (isEn ? "Grace period: " : "宽限期中：") + BoolText(state != null && state.IsInGracePeriod, isEn),
                (isEn ? "Validation code: " : "校验代码：") + SafeValue(validation == null ? string.Empty : validation.ErrorCode, isEn),
                (isEn ? "Validated at: " : "校验时间：") + FormatDateTime(validation == null ? (DateTime?)null : validation.ValidatedAt, isEn),
                (isEn ? "Last apply: " : "最近申请：") + lastApply
            });
        }

        private string BuildTimeText(bool isEn)
        {
            DeviceLicenseState state = LicenseRuntimeContext.Instance.Current;
            DeviceLicense license = state == null ? null : state.License;
            DeviceLicenseValidity validity = license == null ? null : license.Validity;

            return string.Join(Environment.NewLine, new[]
            {
                (isEn ? "Issued at: " : "签发时间：") + FormatDateTime(license == null ? (DateTime?)null : license.IssueTime, isEn),
                (isEn ? "Effective at: " : "生效时间：") + FormatDateTime(validity == null ? (DateTime?)null : validity.NotBefore, isEn),
                (isEn ? "Expires at: " : "到期时间：") + FormatDateTime(validity == null ? (DateTime?)null : validity.ExpiresAt, isEn),
                (isEn ? "License type: " : "授权类型：") + SafeValue(validity == null ? string.Empty : validity.LicenseType, isEn),
                (isEn ? "Grace days: " : "宽限天数：") + (validity == null ? "0" : validity.GraceDays.ToString()),
                (isEn ? "Expired: " : "是否过期：") + BoolText(state != null && state.IsExpired, isEn)
            });
        }

        private string BuildScopeText(bool isEn)
        {
            DeviceLicenseState state = LicenseRuntimeContext.Instance.Current;
            List<string> lines = new List<string>();
            lines.Add(isEn ? "Modules:" : "授权模块：");
            lines.Add(JoinLines(state == null ? null : state.ModuleKeys, isEn));
            lines.Add(string.Empty);
            lines.Add(isEn ? "Pages:" : "授权页面：");
            lines.Add(JoinLines(state == null ? null : state.PageKeys, isEn));
            return string.Join(Environment.NewLine, lines);
        }

        private string GetSelectedNetworkMode()
        {
            return selectNetworkMode.SelectedValue == null
                ? "Online"
                : selectNetworkMode.SelectedValue.ToString();
        }

        private bool IsEnglishLanguage()
        {
            return AM.Tools.Tools.IsEnglishLanguage(AM.Tools.Tools.GetCurrentLanguage());
        }

        private static string JoinLines(IEnumerable<string> values, bool isEn)
        {
            if (values == null)
                return isEn ? "None" : "无";

            string[] items = values.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return items.Length <= 0 ? (isEn ? "None" : "无") : string.Join(Environment.NewLine, items);
        }

        private static string BoolText(bool value, bool isEn)
        {
            return value ? (isEn ? "Yes" : "是") : (isEn ? "No" : "否");
        }

        private static string SafeValue(string value, bool isEn)
        {
            return string.IsNullOrWhiteSpace(value) ? (isEn ? "N/A" : "未提供") : value;
        }

        private static string FormatDateTime(DateTime? value, bool isEn)
        {
            return value.HasValue ? value.Value.ToString("yyyy-MM-dd HH:mm:ss") : (isEn ? "N/A" : "未提供");
        }
    }
}
