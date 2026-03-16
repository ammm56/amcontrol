using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Interfaces.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 轴强类型配置编辑页 ViewModel。
    /// 面向普通操作人员和工艺配置页面。
    /// </summary>
    public class AxisConfigEditorPageViewModel : ObservableObject
    {
        private readonly IAxisConfigAppService _axisConfigAppService;
        private readonly IAppReporter _reporter;

        public AxisConfigEditorPageViewModel(IAxisConfigAppService axisConfigAppService)
        {
            _axisConfigAppService = axisConfigAppService ?? throw new ArgumentNullException(nameof(axisConfigAppService));
            _reporter = SystemContext.Instance.Reporter;

            AxisConfigs = new ObservableCollection<AxisConfigEditorViewModel>();

            LoadCommand = new AsyncRelayCommand(LoadAsync);
            SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
            ReloadFromDatabaseCommand = new AsyncRelayCommand(ReloadFromDatabaseAsync);
        }

        #region Properties

        private AxisConfigEditorViewModel _selectedAxisConfig;
        public AxisConfigEditorViewModel SelectedAxisConfig
        {
            get { return _selectedAxisConfig; }
            set
            {
                if (SetProperty(ref _selectedAxisConfig, value))
                {
                    SaveCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<AxisConfigEditorViewModel> AxisConfigs { get; }

        #endregion

        #region Commands

        public IAsyncRelayCommand LoadCommand { get; }

        public IAsyncRelayCommand SaveCommand { get; }

        public IAsyncRelayCommand ReloadFromDatabaseCommand { get; }

        #endregion

        #region Methods

        private bool CanSave()
        {
            return SelectedAxisConfig != null;
        }

        /// <summary>
        /// 加载当前有效轴配置。
        /// </summary>
        private async Task LoadAsync()
        {
            try
            {
                var result = await Task.Run(() => _axisConfigAppService.QueryAll());
                if (!result.Success)
                {
                    _reporter?.Warn("AxisConfigEditorPageViewModel", result.Message, result.Code);
                    return;
                }

                AxisConfigs.Clear();
                foreach (var item in result.Items.OrderBy(p => p.LogicalAxis))
                {
                    AxisConfigs.Add(new AxisConfigEditorViewModel(item));
                }

                SelectedAxisConfig = AxisConfigs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _reporter?.Error("AxisConfigEditorPageViewModel", ex, "LoadAsync failed");
            }
        }

        /// <summary>
        /// 保存当前选中的轴配置。
        /// </summary>
        private async Task SaveAsync()
        {
            if (SelectedAxisConfig == null) return;

            try
            {
                var result = await Task.Run(() => _axisConfigAppService.Save(SelectedAxisConfig.GetModel()));
                if (!result.Success)
                {
                    _reporter?.Warn("AxisConfigEditorPageViewModel", result.Message, result.Code);
                    return;
                }

                _reporter?.Info("AxisConfigEditorPageViewModel", "轴配置保存成功");
            }
            catch (Exception ex)
            {
                _reporter?.Error("AxisConfigEditorPageViewModel", ex, "SaveAsync failed");
            }
        }

        /// <summary>
        /// 从数据库重新覆盖运行时配置，并刷新页面。
        /// </summary>
        private async Task ReloadFromDatabaseAsync()
        {
            try
            {
                var reloadResult = await Task.Run(() => _axisConfigAppService.ReloadFromDatabase());
                if (!reloadResult.Success)
                {
                    _reporter?.Warn("AxisConfigEditorPageViewModel", reloadResult.Message, reloadResult.Code);
                    return;
                }

                await LoadAsync();
            }
            catch (Exception ex)
            {
                _reporter?.Error("AxisConfigEditorPageViewModel", ex, "ReloadFromDatabaseAsync failed");
            }
        }

        #endregion
    }
}