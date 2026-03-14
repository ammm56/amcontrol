using AM.Core.Context;
using AM.Core.Reporter;
using AM.Model.Entity;
using AM.Model.Interfaces.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AM.ViewModel.ViewModels.Config
{
    public class ConfigAxisArgViewModel : ObservableObject
    {
        private readonly IConfigAxisArgService _configAxisArgService;
        private readonly IAppReporter _reporter;

        public ConfigAxisArgViewModel(IConfigAxisArgService axisService)
        {
            _configAxisArgService = axisService ?? throw new ArgumentNullException(nameof(axisService));
            _reporter = SystemContext.Instance.Reporter;

            AxisParams = new ObservableCollection<ConfigAxisArg>();
            LoadCommand = new AsyncRelayCommand(LoadAxisParamsAsync);
            SaveCommand = new AsyncRelayCommand<ConfigAxisArg>(SaveAxisParamAsync);
            DeleteCommand = new AsyncRelayCommand<ConfigAxisArg>(DeleteAxisParamAsync);
        }

        #region Properties

        private ConfigAxisArg _selectedAxisParam;
        public ConfigAxisArg SelectedAxisParam
        {
            get => _selectedAxisParam;
            set => SetProperty(ref _selectedAxisParam, value);
        }

        private ObservableCollection<ConfigAxisArg> _axisParams;
        public ObservableCollection<ConfigAxisArg> AxisParams
        {
            get => _axisParams;
            private set => SetProperty(ref _axisParams, value);
        }

        #endregion

        #region Commands 中继命令

        public IAsyncRelayCommand LoadCommand { get; }
        public IAsyncRelayCommand<ConfigAxisArg> SaveCommand { get; }
        public IAsyncRelayCommand<ConfigAxisArg> DeleteCommand { get; }

        #endregion

        #region Methods

        /// <summary>
        /// 异步加载所有轴参数
        /// </summary>
        private async Task LoadAxisParamsAsync()
        {
            try
            {
                var result = await Task.Run(() => _configAxisArgService.QueryAll());
                if (!result.Success)
                {
                    _reporter?.Warn("ConfigAxisArgViewModel", result.Message, result.Code);
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    AxisParams.Clear();
                    foreach (var item in result.Items)
                    {
                        AxisParams.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                _reporter?.Error("ConfigAxisArgViewModel", ex, "LoadAxisParamsAsync failed");
            }
        }

        /// <summary>
        /// 保存或更新轴参数
        /// </summary>
        private async Task SaveAxisParamAsync(ConfigAxisArg param)
        {
            if (param == null) return;

            try
            {
                var result = await Task.Run(() => _configAxisArgService.Save(param));
                if (!result.Success)
                {
                    _reporter?.Warn("ConfigAxisArgViewModel", result.Message, result.Code);
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var existing = AxisParams.FirstOrDefault(a =>
                        a.Axis == param.Axis &&
                        a.ParamName == param.ParamName &&
                        a.ParamName_Cn == param.ParamName_Cn);

                    if (existing != null)
                    {
                        var index = AxisParams.IndexOf(existing);
                        AxisParams[index] = param;
                    }
                    else
                    {
                        AxisParams.Add(param);
                    }
                });
            }
            catch (Exception ex)
            {
                _reporter?.Error("ConfigAxisArgViewModel", ex, "SaveAxisParamAsync failed");
            }
        }

        /// <summary>
        /// 删除轴参数
        /// </summary>
        private async Task DeleteAxisParamAsync(ConfigAxisArg param)
        {
            if (param == null) return;

            try
            {
                var result = await Task.Run(() => _configAxisArgService.Delete(param.Axis, param.ParamName, param.ParamName_Cn));
                if (!result.Success)
                {
                    _reporter?.Warn("ConfigAxisArgViewModel", result.Message, result.Code);
                    return;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    AxisParams.Remove(param);
                    if (SelectedAxisParam == param)
                    {
                        SelectedAxisParam = null;
                    }
                });
            }
            catch (Exception ex)
            {
                _reporter?.Error("ConfigAxisArgViewModel", ex, "DeleteAxisParamAsync failed");
            }
        }

        #endregion
    }
}
