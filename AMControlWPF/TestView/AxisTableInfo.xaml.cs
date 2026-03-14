using AM.DBService.Services;
using AM.Model.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace AMControlWPF
{
    /// <summary>
    /// AxisTableInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AxisTableInfo : Window
    {
        private readonly AxisTableInfoState _state = new AxisTableInfoState();

        public AxisTableInfo()
        {
            InitializeComponent();
            DataContext = _state;
            Loaded += AxisTableInfo_Loaded;
        }

        private void AxisTableInfo_Loaded(object sender, RoutedEventArgs e)
        {
            var configAxisArgService = new ConfigAxisArgService();
            var result = configAxisArgService.QueryAll();

            _state.Items.Clear();

            if (!result.Success)
            {
                _state.Message = result.Message;
                return;
            }

            foreach (var item in result.Items)
            {
                _state.Items.Add(item);
            }

            if (_state.Items.Count > 0)
            {
                _state.SelectedItem = _state.Items[0];
                _state.Message = JsonConvert.SerializeObject(_state.SelectedItem, Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                _state.Message = "暂无轴参数数据";
            }
        }

        private void dg_axis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;

            var item = e.AddedItems[0] as ConfigAxisArg;
            if (item == null) return;

            _state.SelectedItem = item;
            _state.Message = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);
        }

        private class AxisTableInfoState : INotifyPropertyChanged
        {
            private ConfigAxisArg _selectedItem;
            private string _message;

            public ObservableCollection<ConfigAxisArg> Items { get; } = new ObservableCollection<ConfigAxisArg>();

            public ConfigAxisArg SelectedItem
            {
                get { return _selectedItem; }
                set
                {
                    if (!Equals(_selectedItem, value))
                    {
                        _selectedItem = value;
                        OnPropertyChanged();
                    }
                }
            }

            public string Message
            {
                get { return _message; }
                set
                {
                    if (_message != value)
                    {
                        _message = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}
