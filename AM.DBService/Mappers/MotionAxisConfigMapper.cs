using AM.Model.Entity.Motion;
using AM.Model.MotionCard;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AM.DBService.Mappers
{
    /// <summary>
    /// 将 motion_axis_config 映射到强类型 AxisConfig。
    /// </summary>
    public static class MotionAxisConfigMapper
    {
        private static readonly Dictionary<string, PropertyInfo> AxisConfigProperties =
            typeof(AxisConfig)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 将数据库参数覆盖到现有 AxisConfig。
        /// </summary>
        public static void Apply(AxisConfig config, IEnumerable<MotionAxisConfigEntity> configRows)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (configRows == null) return;

            foreach (var row in configRows)
            {
                if (row == null || string.IsNullOrWhiteSpace(row.ParamName))
                {
                    continue;
                }

                PropertyInfo property;
                if (!AxisConfigProperties.TryGetValue(row.ParamName, out property))
                {
                    continue;
                }

                object convertedValue;
                if (!TryConvertValue(row, property.PropertyType, out convertedValue))
                {
                    continue;
                }

                property.SetValue(config, convertedValue, null);
            }
        }

        private static bool TryConvertValue(MotionAxisConfigEntity row, Type targetType, out object value)
        {
            value = null;

            try
            {
                var rawValue = row.ParamSetValue;
                var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

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
                    value = Enum.ToObject(
                        underlyingType,
                        Convert.ToInt32(Math.Round(rawValue, MidpointRounding.AwayFromZero), CultureInfo.InvariantCulture));
                    return true;
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