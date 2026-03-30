using AM.Core.Context;
using AM.PageModel.Common;
using AM.PageModel.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace AM.PageModel.Main
{
    /// <summary>
    /// WinForms 主窗体页面模型。
    /// </summary>
    public class MainWindowModel : BindableBase
    {
        private List<NavPrimaryDef> _primaryItems;
        private List<NavPageDef> _secondaryItems;
        private NavPrimaryDef _selectedPrimary;
        private NavPageDef _selectedSecondary;

        public MainWindowModel()
        {
            _primaryItems = new List<NavPrimaryDef>();
            _secondaryItems = new List<NavPageDef>();
        }

        public IList<NavPrimaryDef> PrimaryItems
        {
            get { return _primaryItems; }
        }

        public IList<NavPageDef> SecondaryItems
        {
            get { return _secondaryItems; }
        }

        public NavPrimaryDef SelectedPrimary
        {
            get { return _selectedPrimary; }
            set { SetProperty(ref _selectedPrimary, value); }
        }

        public NavPageDef SelectedSecondary
        {
            get { return _selectedSecondary; }
            set { SetProperty(ref _selectedSecondary, value); }
        }

        public string CurrentUserDisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserContext.Instance.UserName))
                {
                    return UserContext.Instance.LoginName ?? "未登录";
                }

                return UserContext.Instance.UserName;
            }
        }

        public string CurrentRoleDisplayName
        {
            get
            {
                if (UserContext.Instance.IsAdmin || UserContext.Instance.HasRole("Am"))
                {
                    return "管理员";
                }

                if (UserContext.Instance.HasRole("Engineer"))
                {
                    return "工程师";
                }

                if (UserContext.Instance.HasRole("Operator"))
                {
                    return "操作员";
                }

                return "用户";
            }
        }

        public void LoadNavigation()
        {
            _primaryItems = NavigationCatalog
                .GetPrimaryItems()
                .Where(CanAccessPrimary)
                .ToList();

            OnPropertyChanged(nameof(PrimaryItems));

            if (_primaryItems.Count > 0)
            {
                SelectedPrimary = _primaryItems[0];
                LoadSecondaryItems(SelectedPrimary.Key);
            }
            else
            {
                _secondaryItems = new List<NavPageDef>();
                SelectedPrimary = null;
                SelectedSecondary = null;
                OnPropertyChanged(nameof(SecondaryItems));
            }
        }

        public void LoadSecondaryItems(string moduleKey)
        {
            _secondaryItems = NavigationCatalog
                .GetSecondaryItems(moduleKey)
                .Where(CanAccessPage)
                .ToList();

            OnPropertyChanged(nameof(SecondaryItems));

            SelectedSecondary = _secondaryItems.Count > 0 ? _secondaryItems[0] : null;
        }

        private static bool CanAccessPrimary(NavPrimaryDef primary)
        {
            return NavigationCatalog
                .GetSecondaryItems(primary.Key)
                .Any(CanAccessPage);
        }

        private static bool CanAccessPage(NavPageDef page)
        {
            return page != null && UserContext.Instance.HasPagePermission(page.PageKey);
        }
    }
}