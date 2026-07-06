using AM.Core.Base;
using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Common;
using AM.Model.License;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AM.DBService.Services.System
{
    /// <summary>
    /// 授权页面权限收口辅助服务。
    /// 负责把“用户原始页面权限”与“当前授权状态”收口为最终可见页面集合。
    /// </summary>
    public class LicensePagePermissionHelper : ServiceBase
    {
        private static readonly Dictionary<string, string[]> PageKeyAliases =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "Vision.Monitor", new[] { "Vision.Workbench" } },
                { "Vision.Result", new[] { "Vision.Record" } },
                { "Vision.Calibrate", new[] { "Vision.Debug" } }
            };

        protected override string MessageSourceName
        {
            get { return "LicensePagePermissionHelper"; }
        }

        protected override ResultSource DefaultResultSource
        {
            get { return ResultSource.Unknown; }
        }

        public LicensePagePermissionHelper()
            : base()
        {
        }

        public LicensePagePermissionHelper(IAppReporter reporter)
            : base(reporter)
        {
        }

        /// <summary>
        /// 根据当前授权运行时状态生成最终页面权限。
        /// 有效授权时：用户页面权限 ∩ 授权页面。
        /// 无效授权时：用户页面权限 ∩ 最小功能页面白名单。
        /// </summary>
        public Result<string> GetEffectivePageKeys(IEnumerable<string> userPageKeys)
        {
            try
            {
                List<string> normalizedUserPageKeys = NormalizeKeys(userPageKeys);
                DeviceLicenseState licenseState = LicenseRuntimeContext.Instance.Current;

                if (licenseState != null && licenseState.IsValid)
                {
                    List<string> effectivePageKeys = IntersectKeys(
                        normalizedUserPageKeys,
                        ExpandAuthorizedPageKeys(licenseState.PageKeys));

                    return OkListLogOnly(effectivePageKeys, "已按有效授权页面收口用户页面权限");
                }

                List<string> minimalModePageKeys = IntersectKeys(normalizedUserPageKeys, LicenseConstants.DefaultOpenPageKeys);
                return OkListLogOnly(minimalModePageKeys, "当前无有效授权，已按最小功能模式收口用户页面权限");
            }
            catch (Exception ex)
            {
                return Fail<string>(-1, "收口授权页面权限异常", ReportChannels.Log, ex);
            }
        }

        /// <summary>
        /// 对页面键集合做去空值、去重处理。
        /// </summary>
        private static List<string> NormalizeKeys(IEnumerable<string> keys)
        {
            return keys == null
                ? new List<string>()
                : keys
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
        }

        /// <summary>
        /// 对两个页面键集合求交集。
        /// 保持左集合顺序，比较规则忽略大小写。
        /// </summary>
        private static List<string> IntersectKeys(IEnumerable<string> left, IEnumerable<string> right)
        {
            HashSet<string> rightSet = new HashSet<string>(NormalizeKeys(right), StringComparer.OrdinalIgnoreCase);

            return NormalizeKeys(left)
                .Where(rightSet.Contains)
                .ToList();
        }

        /// <summary>
        /// 兼容历史授权模板中的页面重命名。
        /// 当前只覆盖旧视觉导航迁移到 Workbench/Debug/Record 的同模块重命名，不扩展任何新模块授权。
        /// </summary>
        private static List<string> ExpandAuthorizedPageKeys(IEnumerable<string> pageKeys)
        {
            var result = NormalizeKeys(pageKeys);
            var set = new HashSet<string>(result, StringComparer.OrdinalIgnoreCase);

            foreach (var pageKey in result.ToList())
            {
                string[] aliases;
                if (!PageKeyAliases.TryGetValue(pageKey, out aliases) || aliases == null)
                {
                    continue;
                }

                foreach (var alias in aliases)
                {
                    if (!string.IsNullOrWhiteSpace(alias) && set.Add(alias))
                    {
                        result.Add(alias);
                    }
                }
            }

            return result;
        }
    }
}
