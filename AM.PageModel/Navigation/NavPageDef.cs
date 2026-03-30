using System;

namespace AM.PageModel.Navigation
{
    /// <summary>
    /// 二级页面定义。
    /// </summary>
    public sealed class NavPageDef
    {
        public NavPageDef(
            string moduleKey,
            string moduleName,
            string pageKey,
            string displayName,
            string description,
            string defaultRoleCodes,
            string riskLevel,
            int sortOrder)
        {
            ModuleKey = moduleKey;
            ModuleName = moduleName;
            PageKey = pageKey;
            DisplayName = displayName;
            Description = description;
            DefaultRoleCodes = defaultRoleCodes;
            RiskLevel = riskLevel;
            SortOrder = sortOrder;
        }

        public string ModuleKey { get; private set; }

        public string ModuleName { get; private set; }

        public string PageKey { get; private set; }

        public string DisplayName { get; private set; }

        public string Description { get; private set; }

        public string DefaultRoleCodes { get; private set; }

        public string RiskLevel { get; private set; }

        public int SortOrder { get; private set; }

        public string[] AllowedRoles
        {
            get
            {
                return string.IsNullOrWhiteSpace(DefaultRoleCodes)
                    ? new string[0]
                    : DefaultRoleCodes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}