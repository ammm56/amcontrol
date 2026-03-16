using AM.Model.Entity;
using AM.Model.MotionCard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AM.DBService.Mappers
{
    /// <summary>
    /// 将数据库轴参数记录映射为强类型 AxisConfig。
    /// </summary>
    public static class AxisConfigMapper
    {
        private static readonly Dictionary<string, PropertyInfo> _axisConfigProperties =
            typeof(AxisConfig)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 根据数据库参数列表构建 AxisConfig。
        /// </summary>
        public static AxisConfig Build(IEnumerable<ConfigAxisArg> axisArgs, short axisId, string axisName = null)
        {
            var config = new AxisConfig
            {
                AxisId = axisId,
                Name = axisName
            };

            Apply(config, axisArgs);
            return config;
        }

        /// <summary>
        /// 将数据库参数覆盖到现有 AxisConfig。
        /// </summary>
        public static void Apply(AxisConfig config, IEnumerable<ConfigAxisArg> axisArgs)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (axisArgs == null) return;

            foreach (var arg in axisArgs)
            {
                if (arg == null || string.IsNullOrWhiteSpace(arg.ParamName))
                    continue;

                PropertyInfo property;
                if (!_axisConfigProperties.TryGetValue(arg.ParamName, out property))
                    continue;

                object convertedValue;
                if (!TryConvertValue(arg, property.PropertyType, out convertedValue))
                    continue;

                property.SetValue(config, convertedValue, null);
            }
        }

        /// <summary>
        /// 将参数值转换为目标属性类型。
        /// </summary>
        private static bool TryConvertValue(ConfigAxisArg arg, Type targetType, out object value)
        {
            value = null;

            try
            {
                var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                var rawValue = arg.ParamSetVal;
                var valueType = (arg.ParamValueType ?? string.Empty).Trim();

                if (underlyingType == typeof(bool))
                {
                    value = rawValue != 0D;
                    return true;
                }

                if (underlyingType == typeof(short))
                {
                    value = Convert.ToInt16(Math.Round(rawValue, MidpointRounding.AwayFromZero), CultureInfo.InvariantCulture);
                    return true;
                }

                if (underlyingType == typeof(int))
                {
                    value = Convert.ToInt32(Math.Round(rawValue, MidpointRounding.AwayFromZero), CultureInfo.InvariantCulture);
                    return true;
                }

                if (underlyingType == typeof(long))
                {
                    value = Convert.ToInt64(Math.Round(rawValue, MidpointRounding.AwayFromZero), CultureInfo.InvariantCulture);
                    return true;
                }

                if (underlyingType == typeof(float))
                {
                    value = Convert.ToSingle(rawValue, CultureInfo.InvariantCulture);
                    return true;
                }

                if (underlyingType == typeof(double))
                {
                    value = Convert.ToDouble(rawValue, CultureInfo.InvariantCulture);
                    return true;
                }

                if (underlyingType == typeof(decimal))
                {
                    value = Convert.ToDecimal(rawValue, CultureInfo.InvariantCulture);
                    return true;
                }

                if (underlyingType.IsEnum)
                {
                    value = System.Enum.ToObject(
                        underlyingType,
                        Convert.ToInt32(Math.Round(rawValue, MidpointRounding.AwayFromZero), CultureInfo.InvariantCulture));
                    return true;
                }

                // 当前方案仅处理数值型 + 布尔型 + 枚举
                if (string.Equals(valueType, "Bool", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(valueType, "Int16", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(valueType, "Int32", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(valueType, "Double", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(valueType, "Enum", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}