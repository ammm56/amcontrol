using System;

namespace ProtocolLib.CommonLib.Model
{
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