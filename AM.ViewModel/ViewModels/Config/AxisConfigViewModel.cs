using AM.Model;
using AM.Model.MotionCard;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 轴参数包装器 
    /// 把 纯净模型 (AM.Model.AxisConfig) 变成 UI 使用的模型
    /// 使用 partial 是为了配合 Source Generator。.net framework不使用，手动定义。
    /// </summary>
    public partial class AxisConfigViewModel : ObservableObject
    {
        /// <summary>
        /// 内部持有纯净的数据模型
        /// </summary>
        private readonly AxisConfig _model;

        public AxisConfigViewModel(AxisConfig model)
        {
            _model = model;
        }

        /// <summary>
        /// 暴露 Model 给驱动层使用（如果需要同步修改）
        /// </summary>
        /// <returns></returns>
        public AxisConfig GetModel() => _model;

        public short AxisId
        {
            get => _model.AxisId;
            set { if (_model.AxisId != value) { _model.AxisId = value; OnPropertyChanged(nameof(AxisId)); } }
        }

        public string Name
        {
            get => _model.Name;
            set { if (_model.Name != value) { _model.Name = value; OnPropertyChanged(nameof(Name)); } }
        }

        public short LogicalAxis
        {
            get => _model.LogicalAxis;
            set { if (_model.LogicalAxis != value) { _model.LogicalAxis = value; OnPropertyChanged(nameof(LogicalAxis)); } }
        }

        public short PhysicalCore
        {
            get => _model.PhysicalCore;
            set { if (_model.PhysicalCore != value) { _model.PhysicalCore = value; OnPropertyChanged(nameof(PhysicalCore)); } }
        }

        public short PhysicalAxis
        {
            get => _model.PhysicalAxis;
            set { if (_model.PhysicalAxis != value) { _model.PhysicalAxis = value; OnPropertyChanged(nameof(PhysicalAxis)); } }
        }

        public bool AlarmEnabled
        {
            get => _model.AlarmEnabled;
            set { if (_model.AlarmEnabled != value) { _model.AlarmEnabled = value; OnPropertyChanged(nameof(AlarmEnabled)); } }
        }

        public bool AlarmInvert
        {
            get => _model.AlarmInvert;
            set { if (_model.AlarmInvert != value) { _model.AlarmInvert = value; OnPropertyChanged(nameof(AlarmInvert)); } }
        }

        public short PulseMode
        {
            get => _model.PulseMode;
            set { if (_model.PulseMode != value) { _model.PulseMode = value; OnPropertyChanged(nameof(PulseMode)); } }
        }

        public bool EncoderExternal
        {
            get => _model.EncoderExternal;
            set { if (_model.EncoderExternal != value) { _model.EncoderExternal = value; OnPropertyChanged(nameof(EncoderExternal)); } }
        }

        public bool EncoderInvert
        {
            get => _model.EncoderInvert;
            set { if (_model.EncoderInvert != value) { _model.EncoderInvert = value; OnPropertyChanged(nameof(EncoderInvert)); } }
        }

        public bool LimitHomeInvert
        {
            get => _model.LimitHomeInvert;
            set { if (_model.LimitHomeInvert != value) { _model.LimitHomeInvert = value; OnPropertyChanged(nameof(LimitHomeInvert)); } }
        }

        public short LimitMode
        {
            get => _model.LimitMode;
            set { if (_model.LimitMode != value) { _model.LimitMode = value; OnPropertyChanged(nameof(LimitMode)); } }
        }

        public double Lead
        {
            get => _model.Lead;
            set
            {
                if (_model.Lead != value)
                {
                    _model.Lead = value;
                    OnPropertyChanged(nameof(Lead));
                    OnPropertyChanged(nameof(K));// 通知关联的计算属性刷新
                }
            }
        }

        public int PulsePerRev
        {
            get => _model.PulsePerRev;
            set
            {
                if (_model.PulsePerRev != value)
                {
                    _model.PulsePerRev = value;
                    OnPropertyChanged(nameof(PulsePerRev));
                    OnPropertyChanged(nameof(K));
                }
            }
        }

        public double GearRatio
        {
            get => _model.GearRatio;
            set
            {
                if (_model.GearRatio != value)
                {
                    _model.GearRatio = value;
                    OnPropertyChanged(nameof(GearRatio));
                    OnPropertyChanged(nameof(K));
                }
            }
        }

        /// <summary>
        /// 脉冲系数 (只读，由 Model 计算)
        /// </summary>
        public double K => _model.K;
        /// <summary>
        /// 手写属性或使用 [ObservableProperty] 包装逻辑
        /// </summary>
        public double Acc
        {
            get => _model.Acc;
            set
            {
                // 1. 先进行范围校验（ViewModel 也要校验）
                double validated = value.LimitTo(0.001, 1.0);
                // 2. 检查值是否真的改变了
                if (_model.Acc != validated)
                {
                    // 3. 直接修改 Model 的私有字段或属性
                    _model.Acc = validated;
                    // 4. 手动触发属性变更通知，让 UI 刷新
                    OnPropertyChanged(nameof(Acc));
                }
            }
        }

        public double Dec
        {
            get => _model.Dec;
            set
            {
                double validated = value.LimitTo(0.001, 1.0);
                if (_model.Dec != validated)
                {
                    _model.Dec = validated;
                    OnPropertyChanged(nameof(Dec));
                }
            }
        }

        public short SmoothTime
        {
            get => _model.SmoothTime;
            set
            {
                short validated = value.LimitTo((short)0, (short)256);
                if (_model.SmoothTime != validated)
                {
                    _model.SmoothTime = validated;
                    OnPropertyChanged(nameof(SmoothTime));
                }
            }
        }

        /// <summary>
        /// config中添加到视图
        /// </summary>
        public short StandardHomeMode
        {
            get => _model.StandardHomeMode;
            set
            {
                if (_model.StandardHomeMode != value)
                {
                    _model.StandardHomeMode = value;
                    OnPropertyChanged(nameof(StandardHomeMode));
                }
            }
        }

        public double HomeSearchVelocity
        {
            get => _model.HomeSearchVelocity;
            set
            {
                if (_model.HomeSearchVelocity != value)
                {
                    _model.HomeSearchVelocity = value;
                    OnPropertyChanged(nameof(HomeSearchVelocity));
                }
            }
        }

        public double IndexSearchVelocity
        {
            get => _model.IndexSearchVelocity;
            set
            {
                if (_model.IndexSearchVelocity != value)
                {
                    _model.IndexSearchVelocity = value;
                    OnPropertyChanged(nameof(IndexSearchVelocity));
                }
            }
        }

        public int HomeOffset
        {
            get => _model.HomeOffset;
            set
            {
                if (_model.HomeOffset != value)
                {
                    _model.HomeOffset = value;
                    OnPropertyChanged(nameof(HomeOffset));
                }
            }
        }

        public bool HomeCheck
        {
            get => _model.HomeCheck;
            set
            {
                if (_model.HomeCheck != value)
                {
                    _model.HomeCheck = value;
                    OnPropertyChanged(nameof(HomeCheck));
                }
            }
        }

        public bool HomeAutoZeroPos
        {
            get => _model.HomeAutoZeroPos;
            set
            {
                if (_model.HomeAutoZeroPos != value)
                {
                    _model.HomeAutoZeroPos = value;
                    OnPropertyChanged(nameof(HomeAutoZeroPos));
                }
            }
        }

        public int HomeTimeoutMs
        {
            get => _model.HomeTimeoutMs;
            set
            {
                int validated = value < 1000 ? 1000 : value;
                if (_model.HomeTimeoutMs != validated)
                {
                    _model.HomeTimeoutMs = validated;
                    OnPropertyChanged(nameof(HomeTimeoutMs));
                }
            }
        }

        public bool IsServoOn
        {
            get => _model.IsServoOn;
            set { if (_model.IsServoOn != value) { _model.IsServoOn = value; OnPropertyChanged(nameof(IsServoOn)); } }
        }

        private bool _isSelected;

        /// <summary>
        /// UI 状态，不在 AM.Model 中
        /// </summary>
        public bool IsSelected { get => _isSelected; set => SetProperty(ref _isSelected, value); }
    }
}
