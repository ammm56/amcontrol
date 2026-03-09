using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AM.Model.Common
{
    /// <summary>
    /// 继承属性更改通知接口，模型继承调用属性更改通知
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发属性更改通知
        /// </summary>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 比较值并更新，同时自动触发通知（解决 set 简写问题）
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">引用私有字段</param>
        /// <param name="newValue">新值</param>
        /// <param name="propertyName">属性名（自动获取）</param>
        /// <returns>是否更新成功</returns>
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            // 如果值没变，直接返回，避免无效的 UI 刷新
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
