using System;
using System.Globalization;

namespace ProtocolLib.CommonLib.Model
{
    /// <summary>
    /// 带类型的值对象
    /// </summary>
    public class M_TypedValue
    {
        public string value { get; set; } = "";
        public string type { get; set; } = "string";

        private static string NormalizeType(string leftType, string rightType)
        {
            string l = string.IsNullOrEmpty(leftType) ? "string" : leftType.ToLowerInvariant();
            string r = string.IsNullOrEmpty(rightType) ? "string" : rightType.ToLowerInvariant();

            if (l != r)
            {
                throw new InvalidOperationException("参与运算或比较的两个值类型不一致");
            }

            return l;
        }

        private static object GetTypedValue(M_TypedValue item)
        {
            if (item == null) return string.Empty;

            string dataType = string.IsNullOrEmpty(item.type) ? "string" : item.type.ToLowerInvariant();
            string text = item.value ?? string.Empty;

            switch (dataType)
            {
                case "bool":
                    return ParseBooleanToByte(text);
                case "byte":
                    return byte.Parse(text, CultureInfo.InvariantCulture);
                case "int16":
                    return short.Parse(text, CultureInfo.InvariantCulture);
                case "uint16":
                    return ushort.Parse(text, CultureInfo.InvariantCulture);
                case "int32":
                    return int.Parse(text, CultureInfo.InvariantCulture);
                case "uint32":
                    return uint.Parse(text, CultureInfo.InvariantCulture);
                case "int64":
                    return long.Parse(text, CultureInfo.InvariantCulture);
                case "uint64":
                    return ulong.Parse(text, CultureInfo.InvariantCulture);
                case "single":
                    return float.Parse(text, CultureInfo.InvariantCulture);
                case "double":
                    return double.Parse(text, CultureInfo.InvariantCulture);
                case "string":
                default:
                    return text;
            }
        }

        private static byte ParseBooleanToByte(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            string valueText = text.Trim();
            if (valueText == "1") return 1;
            if (valueText == "0") return 0;

            bool result;
            if (bool.TryParse(valueText, out result))
            {
                return result ? (byte)1 : (byte)0;
            }

            throw new FormatException("布尔值转换失败");
        }

        private static int CompareCore(M_TypedValue left, M_TypedValue right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (ReferenceEquals(left, null)) return -1;
            if (ReferenceEquals(right, null)) return 1;

            string resultType = NormalizeType(left.type, right.type);

            try
            {
                if (resultType == "string")
                {
                    return string.Compare(left.value, right.value, StringComparison.Ordinal);
                }

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

            IComparable comparable = left as IComparable;
            if (comparable != null && left.GetType() == right.GetType())
            {
                return comparable.CompareTo(right);
            }

            decimal dl = Convert.ToDecimal(left, CultureInfo.InvariantCulture);
            decimal dr = Convert.ToDecimal(right, CultureInfo.InvariantCulture);
            return dl.CompareTo(dr);
        }

        private static M_TypedValue Calculate(M_TypedValue left, M_TypedValue right, string op)
        {
            if (left == null || right == null)
            {
                throw new ArgumentNullException("参与运算的对象不能为空");
            }

            string resultType = NormalizeType(left.type, right.type);
            string resultValue = CalculateValue(left.value, right.value, resultType, op);

            return new M_TypedValue
            {
                type = resultType,
                value = resultValue
            };
        }

        private static string CalculateValue(string left, string right, string type, string op)
        {
            switch (type)
            {
                case "byte":
                    return CalculateByte(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "int16":
                    return CalculateInt16(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "uint16":
                    return CalculateUInt16(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "int32":
                    return CalculateInt32(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "uint32":
                    return CalculateUInt32(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "int64":
                    return CalculateInt64(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "uint64":
                    return CalculateUInt64(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "single":
                    return CalculateSingle(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "double":
                    return CalculateDouble(left, right, op).ToString(CultureInfo.InvariantCulture);
                case "string":
                    if (op == "+")
                    {
                        return (left ?? string.Empty) + (right ?? string.Empty);
                    }
                    throw new NotSupportedException("string 类型仅支持 + 运算");
                case "bool":
                    throw new NotSupportedException("bool 类型不支持四则运算");
                default:
                    throw new NotSupportedException("不支持的数据类型:" + type);
            }
        }

        private static byte CalculateByte(string left, string right, string op)
        {
            byte l = byte.Parse(left ?? "0", CultureInfo.InvariantCulture);
            byte r = byte.Parse(right ?? "0", CultureInfo.InvariantCulture);

            checked
            {
                switch (op)
                {
                    case "+": return (byte)(l + r);
                    case "-": return (byte)(l - r);
                    case "*": return (byte)(l * r);
                    case "/":
                        if (r == 0) throw new DivideByZeroException("除数不能为0");
                        return (byte)(l / r);
                    default: throw new NotSupportedException("不支持的运算符:" + op);
                }
            }
        }

        private static short CalculateInt16(string left, string right, string op)
        {
            short l = short.Parse(left ?? "0", CultureInfo.InvariantCulture);
            short r = short.Parse(right ?? "0", CultureInfo.InvariantCulture);

            checked
            {
                switch (op)
                {
                    case "+": return (short)(l + r);
                    case "-": return (short)(l - r);
                    case "*": return (short)(l * r);
                    case "/":
                        if (r == 0) throw new DivideByZeroException("除数不能为0");
                        return (short)(l / r);
                    default: throw new NotSupportedException("不支持的运算符:" + op);
                }
            }
        }

        private static ushort CalculateUInt16(string left, string right, string op)
        {
            ushort l = ushort.Parse(left ?? "0", CultureInfo.InvariantCulture);
            ushort r = ushort.Parse(right ?? "0", CultureInfo.InvariantCulture);

            checked
            {
                switch (op)
                {
                    case "+": return (ushort)(l + r);
                    case "-": return (ushort)(l - r);
                    case "*": return (ushort)(l * r);
                    case "/":
                        if (r == 0) throw new DivideByZeroException("除数不能为0");
                        return (ushort)(l / r);
                    default: throw new NotSupportedException("不支持的运算符:" + op);
                }
            }
        }

        private static int CalculateInt32(string left, string right, string op)
        {
            int l = int.Parse(left ?? "0", CultureInfo.InvariantCulture);
            int r = int.Parse(right ?? "0", CultureInfo.InvariantCulture);

            checked
            {
                switch (op)
                {
                    case "+": return l + r;
                    case "-": return l - r;
                    case "*": return l * r;
                    case "/":
                        if (r == 0) throw new DivideByZeroException("除数不能为0");
                        return l / r;
                    default: throw new NotSupportedException("不支持的运算符:" + op);
                }
            }
        }

        private static uint CalculateUInt32(string left, string right, string op)
        {
            uint l = uint.Parse(left ?? "0", CultureInfo.InvariantCulture);
            uint r = uint.Parse(right ?? "0", CultureInfo.InvariantCulture);

            checked
            {
                switch (op)
                {
                    case "+": return l + r;
                    case "-": return l - r;
                    case "*": return l * r;
                    case "/":
                        if (r == 0) throw new DivideByZeroException("除数不能为0");
                        return l / r;
                    default: throw new NotSupportedException("不支持的运算符:" + op);
                }
            }
        }

        private static long CalculateInt64(string left, string right, string op)
        {
            long l = long.Parse(left ?? "0", CultureInfo.InvariantCulture);
            long r = long.Parse(right ?? "0", CultureInfo.InvariantCulture);

            checked
            {
                switch (op)
                {
                    case "+": return l + r;
                    case "-": return l - r;
                    case "*": return l * r;
                    case "/":
                        if (r == 0) throw new DivideByZeroException("除数不能为0");
                        return l / r;
                    default: throw new NotSupportedException("不支持的运算符:" + op);
                }
            }
        }

        private static ulong CalculateUInt64(string left, string right, string op)
        {
            ulong l = ulong.Parse(left ?? "0", CultureInfo.InvariantCulture);
            ulong r = ulong.Parse(right ?? "0", CultureInfo.InvariantCulture);

            checked
            {
                switch (op)
                {
                    case "+": return l + r;
                    case "-": return l - r;
                    case "*": return l * r;
                    case "/":
                        if (r == 0) throw new DivideByZeroException("除数不能为0");
                        return l / r;
                    default: throw new NotSupportedException("不支持的运算符:" + op);
                }
            }
        }

        private static float CalculateSingle(string left, string right, string op)
        {
            float l = float.Parse(left ?? "0", CultureInfo.InvariantCulture);
            float r = float.Parse(right ?? "0", CultureInfo.InvariantCulture);

            switch (op)
            {
                case "+": return l + r;
                case "-": return l - r;
                case "*": return l * r;
                case "/":
                    if (Math.Abs(r) < float.Epsilon) throw new DivideByZeroException("除数不能为0");
                    return l / r;
                default: throw new NotSupportedException("不支持的运算符:" + op);
            }
        }

        private static double CalculateDouble(string left, string right, string op)
        {
            double l = double.Parse(left ?? "0", CultureInfo.InvariantCulture);
            double r = double.Parse(right ?? "0", CultureInfo.InvariantCulture);

            switch (op)
            {
                case "+": return l + r;
                case "-": return l - r;
                case "*": return l * r;
                case "/":
                    if (Math.Abs(r) < double.Epsilon) throw new DivideByZeroException("除数不能为0");
                    return l / r;
                default: throw new NotSupportedException("不支持的运算符:" + op);
            }
        }

        public static M_TypedValue operator +(M_TypedValue left, M_TypedValue right)
        {
            return Calculate(left, right, "+");
        }

        public static M_TypedValue operator -(M_TypedValue left, M_TypedValue right)
        {
            return Calculate(left, right, "-");
        }

        public static M_TypedValue operator *(M_TypedValue left, M_TypedValue right)
        {
            return Calculate(left, right, "*");
        }

        public static M_TypedValue operator /(M_TypedValue left, M_TypedValue right)
        {
            return Calculate(left, right, "/");
        }

        public static bool operator >(M_TypedValue left, M_TypedValue right)
        {
            return CompareCore(left, right) > 0;
        }

        public static bool operator <(M_TypedValue left, M_TypedValue right)
        {
            return CompareCore(left, right) < 0;
        }

        public static bool operator ==(M_TypedValue left, M_TypedValue right)
        {
            return CompareCore(left, right) == 0;
        }

        public static bool operator !=(M_TypedValue left, M_TypedValue right)
        {
            return CompareCore(left, right) != 0;
        }

        public static bool operator >=(M_TypedValue left, M_TypedValue right)
        {
            return CompareCore(left, right) >= 0;
        }

        public static bool operator <=(M_TypedValue left, M_TypedValue right)
        {
            return CompareCore(left, right) <= 0;
        }

        public override bool Equals(object obj)
        {
            M_TypedValue other = obj as M_TypedValue;
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

        public override string ToString()
        {
            return value ?? string.Empty;
        }
    }

    /// <summary>
    /// 条件比较
    /// </summary>
    public class M_Compare
    {
        public M_TypedValue leftvalue { get; set; } = new M_TypedValue();
        public string operation { get; set; } = "";
        public M_TypedValue rightvalue { get; set; } = new M_TypedValue();

        public bool Compare()
        {
            return _compare(operation, leftvalue, rightvalue);
        }

        private bool _compare(string operation, M_TypedValue left, M_TypedValue right)
        {
            try
            {
                switch (operation)
                {
                    case ">":
                        return left > right;
                    case ">=":
                        return left >= right;
                    case "<":
                        return left < right;
                    case "<=":
                        return left <= right;
                    case "=":
                        return left == right;
                    case "!=":
                        return left != right;
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