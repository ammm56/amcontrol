using ProtocolLib.CommonLib.Common;
using System;
using System.Globalization;

namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 比较值
    /// </summary>
    public class M_CompareValue
    {
        public string value { get; set; } = "";
        public string type { get; set; } = "string";

        private static object GetTypedValue(M_CompareValue item)
        {
            if (item == null) return string.Empty;

            string dataType = (item.type ?? "string").ToLowerInvariant();
            string text = item.value ?? string.Empty;

            switch (dataType)
            {
                case "bool":
                    return ToBoolByte(text);
                case "byte":
                    return Convert.ToByte(text, CultureInfo.InvariantCulture);
                case "int16":
                    return Convert.ToInt16(text, CultureInfo.InvariantCulture);
                case "uint16":
                    return Convert.ToUInt16(text, CultureInfo.InvariantCulture);
                case "int32":
                    return Convert.ToInt32(text, CultureInfo.InvariantCulture);
                case "uint32":
                    return Convert.ToUInt32(text, CultureInfo.InvariantCulture);
                case "int64":
                    return Convert.ToInt64(text, CultureInfo.InvariantCulture);
                case "uint64":
                    return Convert.ToUInt64(text, CultureInfo.InvariantCulture);
                case "single":
                    return Convert.ToSingle(text, CultureInfo.InvariantCulture);
                case "double":
                    return Convert.ToDouble(text, CultureInfo.InvariantCulture);
                default:
                    return text;
            }
        }

        private static byte ToBoolByte(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            text = text.Trim();
            if (text == "1") return 1;
            if (text == "0") return 0;

            bool result;
            if (bool.TryParse(text, out result))
            {
                return result ? (byte)1 : (byte)0;
            }

            throw new FormatException("布尔值转换失败");
        }

        private static int CompareCore(M_CompareValue left, M_CompareValue right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (ReferenceEquals(left, null)) return -1;
            if (ReferenceEquals(right, null)) return 1;

            if (string.Equals(left.type, "string", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(right.type, "string", StringComparison.OrdinalIgnoreCase))
            {
                return string.Compare(left.value, right.value, StringComparison.Ordinal);
            }

            try
            {
                object l = GetTypedValue(left);
                object r = GetTypedValue(right);
                return CompareTypedValue(l, r);
            }
            catch (Exception)
            {
                Console.WriteLine(
                    string.Format("值转换错误，默认string比较。L={0} LT={1} R={2} RT={3}",
                    left.value, left.type, right.value, right.type));

                return string.Compare(left.value, right.value, StringComparison.Ordinal);
            }
        }

        private static int CompareTypedValue(object left, object right)
        {
            if (left == null && right == null) return 0;
            if (left == null) return -1;
            if (right == null) return 1;

            if (left.GetType() == right.GetType())
            {
                IComparable comparable = left as IComparable;
                if (comparable != null)
                {
                    return comparable.CompareTo(right);
                }
            }

            decimal dl = Convert.ToDecimal(left, CultureInfo.InvariantCulture);
            decimal dr = Convert.ToDecimal(right, CultureInfo.InvariantCulture);
            return dl.CompareTo(dr);
        }

        public static bool operator >(M_CompareValue L, M_CompareValue R)
        {
            return CompareCore(L, R) > 0;
        }

        public static bool operator <(M_CompareValue L, M_CompareValue R)
        {
            return CompareCore(L, R) < 0;
        }

        public static bool operator ==(M_CompareValue L, M_CompareValue R)
        {
            return CompareCore(L, R) == 0;
        }

        public static bool operator !=(M_CompareValue L, M_CompareValue R)
        {
            return CompareCore(L, R) != 0;
        }

        public static bool operator >=(M_CompareValue L, M_CompareValue R)
        {
            return CompareCore(L, R) >= 0;
        }

        public static bool operator <=(M_CompareValue L, M_CompareValue R)
        {
            return CompareCore(L, R) <= 0;
        }

        public override bool Equals(object obj)
        {
            M_CompareValue other = obj as M_CompareValue;
            if (other == null) return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (value ?? string.Empty).GetHashCode();
                hash = hash * 23 + ((type ?? string.Empty).ToLowerInvariant().GetHashCode());
                return hash;
            }
        }
    }

    /// <summary>
    /// 条件比较
    /// </summary>
    public class M_Compare
    {
        public M_CompareValue leftvalue { get; set; } = new M_CompareValue();
        public string operation { get; set; } = "";
        public M_CompareValue rightvalue { get; set; } = new M_CompareValue();

        public bool Compare()
        {
            return _compare(operation, leftvalue, rightvalue);
        }

        private bool _compare(string operation, M_CompareValue L, M_CompareValue R)
        {
            try
            {
                switch (operation)
                {
                    case ">":
                        return L > R;
                    case ">=":
                        return L >= R;
                    case "<":
                        return L < R;
                    case "<=":
                        return L <= R;
                    case "=":
                        return L == R;
                    case "!=":
                        return L != R;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("M_Compare 异常=" + ex.Message);
                return false;
            }
        }
    }
}