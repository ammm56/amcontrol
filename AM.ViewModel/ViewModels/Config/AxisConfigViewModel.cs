using AM.Model.MotionCard;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 轴参数包装器。
    /// 把纯净模型 AxisConfig 转成 UI 使用模型。
    /// </summary>
    public partial class AxisConfigViewModel : ObservableObject
    {
        /// <summary>
        /// 内部模型。
        /// </summary>
        private readonly AxisConfig _model;

        public AxisConfigViewModel(AxisConfig model)
        {
            _model = model;
        }

        /// <summary>
        /// 返回内部模型。
        /// </summary>
        public AxisConfig GetModel()
        {
            return _model;
        }

        public short AxisId
        {
            get { return _model.AxisId; }
            set
            {
                if (_model.AxisId != value)
                {
                    _model.AxisId = value;
                    OnPropertyChanged(nameof(AxisId));
                }
            }
        }

        public string Name
        {
            get { return _model.Name; }
            set
            {
                if (_model.Name != value)
                {
                    _model.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public short LogicalAxis
        {
            get { return _model.LogicalAxis; }
            set
            {
                if (_model.LogicalAxis != value)
                {
                    _model.LogicalAxis = value;
                    OnPropertyChanged(nameof(LogicalAxis));
                }
            }
        }

        public short PhysicalCore
        {
            get { return _model.PhysicalCore; }
            set
            {
                if (_model.PhysicalCore != value)
                {
                    _model.PhysicalCore = value;
                    OnPropertyChanged(nameof(PhysicalCore));
                }
            }
        }

        public short PhysicalAxis
        {
            get { return _model.PhysicalAxis; }
            set
            {
                if (_model.PhysicalAxis != value)
                {
                    _model.PhysicalAxis = value;
                    OnPropertyChanged(nameof(PhysicalAxis));
                }
            }
        }

        public bool AlarmEnabled
        {
            get { return _model.AlarmEnabled; }
            set
            {
                if (_model.AlarmEnabled != value)
                {
                    _model.AlarmEnabled = value;
                    OnPropertyChanged(nameof(AlarmEnabled));
                }
            }
        }

        public bool AlarmInvert
        {
            get { return _model.AlarmInvert; }
            set
            {
                if (_model.AlarmInvert != value)
                {
                    _model.AlarmInvert = value;
                    OnPropertyChanged(nameof(AlarmInvert));
                }
            }
        }

        public bool EnableInvert
        {
            get { return _model.EnableInvert; }
            set
            {
                if (_model.EnableInvert != value)
                {
                    _model.EnableInvert = value;
                    OnPropertyChanged(nameof(EnableInvert));
                }
            }
        }

        public short PulseMode
        {
            get { return _model.PulseMode; }
            set
            {
                if (_model.PulseMode != value)
                {
                    _model.PulseMode = value;
                    OnPropertyChanged(nameof(PulseMode));
                }
            }
        }

        public short DefaultMoveMode
        {
            get { return _model.DefaultMoveMode; }
            set
            {
                if (_model.DefaultMoveMode != value)
                {
                    _model.DefaultMoveMode = value;
                    OnPropertyChanged(nameof(DefaultMoveMode));
                }
            }
        }

        public bool EncoderExternal
        {
            get { return _model.EncoderExternal; }
            set
            {
                if (_model.EncoderExternal != value)
                {
                    _model.EncoderExternal = value;
                    OnPropertyChanged(nameof(EncoderExternal));
                }
            }
        }

        public bool EncoderInvert
        {
            get { return _model.EncoderInvert; }
            set
            {
                if (_model.EncoderInvert != value)
                {
                    _model.EncoderInvert = value;
                    OnPropertyChanged(nameof(EncoderInvert));
                }
            }
        }

        public bool LimitHomeInvert
        {
            get { return _model.LimitHomeInvert; }
            set
            {
                if (_model.LimitHomeInvert != value)
                {
                    _model.LimitHomeInvert = value;
                    OnPropertyChanged(nameof(LimitHomeInvert));
                }
            }
        }

        public short LimitMode
        {
            get { return _model.LimitMode; }
            set
            {
                if (_model.LimitMode != value)
                {
                    _model.LimitMode = value;
                    OnPropertyChanged(nameof(LimitMode));
                }
            }
        }

        public short TriggerEdge
        {
            get { return _model.TriggerEdge; }
            set
            {
                if (_model.TriggerEdge != value)
                {
                    _model.TriggerEdge = value;
                    OnPropertyChanged(nameof(TriggerEdge));
                }
            }
        }

        public double Lead
        {
            get { return _model.Lead; }
            set
            {
                if (_model.Lead != value)
                {
                    _model.Lead = value;
                    OnPropertyChanged(nameof(Lead));
                    OnPropertyChanged(nameof(K));
                }
            }
        }

        public int PulsePerRev
        {
            get { return _model.PulsePerRev; }
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
            get { return _model.GearRatio; }
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
        /// 脉冲当量。
        /// </summary>
        public double K
        {
            get { return _model.K; }
        }

        public double DefaultVelocity
        {
            get { return _model.DefaultVelocity; }
            set
            {
                double validated = value.LimitTo(0.001, 200.0);
                if (_model.DefaultVelocity != validated)
                {
                    _model.DefaultVelocity = validated;
                    OnPropertyChanged(nameof(DefaultVelocity));
                }
            }
        }

        public double JogVelocity
        {
            get { return _model.JogVelocity; }
            set
            {
                double validated = value.LimitTo(0.001, 100.0);
                if (_model.JogVelocity != validated)
                {
                    _model.JogVelocity = validated;
                    OnPropertyChanged(nameof(JogVelocity));
                }
            }
        }

        public double Acc
        {
            get { return _model.Acc; }
            set
            {
                double validated = value.LimitTo(0.001, 20.0);
                if (_model.Acc != validated)
                {
                    _model.Acc = validated;
                    OnPropertyChanged(nameof(Acc));
                }
            }
        }

        public double Dec
        {
            get { return _model.Dec; }
            set
            {
                double validated = value.LimitTo(0.001, 20.0);
                if (_model.Dec != validated)
                {
                    _model.Dec = validated;
                    OnPropertyChanged(nameof(Dec));
                }
            }
        }

        public short SmoothTime
        {
            get { return _model.SmoothTime; }
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

        public double HomeDeceleration
        {
            get { return _model.HomeDeceleration; }
            set
            {
                double validated = value.LimitTo(0.001, 20.0);
                if (_model.HomeDeceleration != validated)
                {
                    _model.HomeDeceleration = validated;
                    OnPropertyChanged(nameof(HomeDeceleration));
                }
            }
        }

        public double NormalStopDeceleration
        {
            get { return _model.NormalStopDeceleration; }
            set
            {
                double validated = value.LimitTo(0.001, 20.0);
                if (_model.NormalStopDeceleration != validated)
                {
                    _model.NormalStopDeceleration = validated;
                    OnPropertyChanged(nameof(NormalStopDeceleration));
                }
            }
        }

        public double EmergencyStopDeceleration
        {
            get { return _model.EmergencyStopDeceleration; }
            set
            {
                double validated = value.LimitTo(0.001, 20.0);
                if (_model.EmergencyStopDeceleration != validated)
                {
                    _model.EmergencyStopDeceleration = validated;
                    OnPropertyChanged(nameof(EmergencyStopDeceleration));
                }
            }
        }

        public short StandardHomeMode
        {
            get { return _model.StandardHomeMode; }
            set
            {
                if (_model.StandardHomeMode != value)
                {
                    _model.StandardHomeMode = value;
                    OnPropertyChanged(nameof(StandardHomeMode));
                }
            }
        }

        public short ResetDirection
        {
            get { return _model.ResetDirection; }
            set
            {
                if (_model.ResetDirection != value)
                {
                    _model.ResetDirection = value;
                    OnPropertyChanged(nameof(ResetDirection));
                }
            }
        }

        public double HomeSearchVelocity
        {
            get { return _model.HomeSearchVelocity; }
            set
            {
                double validated = value.LimitTo(0.001, 50.0);
                if (_model.HomeSearchVelocity != validated)
                {
                    _model.HomeSearchVelocity = validated;
                    OnPropertyChanged(nameof(HomeSearchVelocity));
                }
            }
        }

        public double IndexSearchVelocity
        {
            get { return _model.IndexSearchVelocity; }
            set
            {
                double validated = value.LimitTo(0.001, 20.0);
                if (_model.IndexSearchVelocity != validated)
                {
                    _model.IndexSearchVelocity = validated;
                    OnPropertyChanged(nameof(IndexSearchVelocity));
                }
            }
        }

        public int HomeOffset
        {
            get { return _model.HomeOffset; }
            set
            {
                if (_model.HomeOffset != value)
                {
                    _model.HomeOffset = value;
                    OnPropertyChanged(nameof(HomeOffset));
                }
            }
        }

        public int HomeMaxDistance
        {
            get { return _model.HomeMaxDistance; }
            set
            {
                if (_model.HomeMaxDistance != value)
                {
                    _model.HomeMaxDistance = value;
                    OnPropertyChanged(nameof(HomeMaxDistance));
                }
            }
        }

        public int IndexMaxDistance
        {
            get { return _model.IndexMaxDistance; }
            set
            {
                if (_model.IndexMaxDistance != value)
                {
                    _model.IndexMaxDistance = value;
                    OnPropertyChanged(nameof(IndexMaxDistance));
                }
            }
        }

        public int EscapeStep
        {
            get { return _model.EscapeStep; }
            set
            {
                if (_model.EscapeStep != value)
                {
                    _model.EscapeStep = value;
                    OnPropertyChanged(nameof(EscapeStep));
                }
            }
        }

        public short IndexSearchDirection
        {
            get { return _model.IndexSearchDirection; }
            set
            {
                if (_model.IndexSearchDirection != value)
                {
                    _model.IndexSearchDirection = value;
                    OnPropertyChanged(nameof(IndexSearchDirection));
                }
            }
        }

        public bool HomeCheck
        {
            get { return _model.HomeCheck; }
            set
            {
                if (_model.HomeCheck != value)
                {
                    _model.HomeCheck = value;
                    OnPropertyChanged(nameof(HomeCheck));
                }
            }
        }

        public bool HomeUseHomeSignal
        {
            get { return _model.HomeUseHomeSignal; }
            set
            {
                if (_model.HomeUseHomeSignal != value)
                {
                    _model.HomeUseHomeSignal = value;
                    OnPropertyChanged(nameof(HomeUseHomeSignal));
                }
            }
        }

        public bool HomeUseIndexSignal
        {
            get { return _model.HomeUseIndexSignal; }
            set
            {
                if (_model.HomeUseIndexSignal != value)
                {
                    _model.HomeUseIndexSignal = value;
                    OnPropertyChanged(nameof(HomeUseIndexSignal));
                }
            }
        }

        public bool HomeUseLimitSignal
        {
            get { return _model.HomeUseLimitSignal; }
            set
            {
                if (_model.HomeUseLimitSignal != value)
                {
                    _model.HomeUseLimitSignal = value;
                    OnPropertyChanged(nameof(HomeUseLimitSignal));
                }
            }
        }

        public bool HomeAutoZeroPos
        {
            get { return _model.HomeAutoZeroPos; }
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
            get { return _model.HomeTimeoutMs; }
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

        public bool SoftLimitEnabled
        {
            get { return _model.SoftLimitEnabled; }
            set
            {
                if (_model.SoftLimitEnabled != value)
                {
                    _model.SoftLimitEnabled = value;
                    OnPropertyChanged(nameof(SoftLimitEnabled));
                }
            }
        }

        public double SoftLimitPositive
        {
            get { return _model.SoftLimitPositive; }
            set
            {
                if (_model.SoftLimitPositive != value)
                {
                    _model.SoftLimitPositive = value;
                    OnPropertyChanged(nameof(SoftLimitPositive));
                }
            }
        }

        public double SoftLimitNegative
        {
            get { return _model.SoftLimitNegative; }
            set
            {
                if (_model.SoftLimitNegative != value)
                {
                    _model.SoftLimitNegative = value;
                    OnPropertyChanged(nameof(SoftLimitNegative));
                }
            }
        }

        public int EnableDelayMs
        {
            get { return _model.EnableDelayMs; }
            set
            {
                if (_model.EnableDelayMs != value)
                {
                    _model.EnableDelayMs = value;
                    OnPropertyChanged(nameof(EnableDelayMs));
                }
            }
        }

        public int DisableDelayMs
        {
            get { return _model.DisableDelayMs; }
            set
            {
                if (_model.DisableDelayMs != value)
                {
                    _model.DisableDelayMs = value;
                    OnPropertyChanged(nameof(DisableDelayMs));
                }
            }
        }

        public int EStopId
        {
            get { return _model.EStopId; }
            set
            {
                if (_model.EStopId != value)
                {
                    _model.EStopId = value;
                    OnPropertyChanged(nameof(EStopId));
                }
            }
        }

        public int StopId
        {
            get { return _model.StopId; }
            set
            {
                if (_model.StopId != value)
                {
                    _model.StopId = value;
                    OnPropertyChanged(nameof(StopId));
                }
            }
        }

        public bool IsServoOn
        {
            get { return _model.IsServoOn; }
            set
            {
                if (_model.IsServoOn != value)
                {
                    _model.IsServoOn = value;
                    OnPropertyChanged(nameof(IsServoOn));
                }
            }
        }

        private bool _isSelected;

        /// <summary>
        /// UI 状态，不在模型中。
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
    }
}