using ProtocolLib.CommonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionFunc
    {

        /// <summary>
		/// 拷贝当前的实例数组，是基于引用层的浅拷贝，如果类型为值类型，那就是深度拷贝，如果类型为引用类型，就是浅拷贝
		/// </summary>
		/// <typeparam name="T">类型对象</typeparam>
		/// <param name="value">数组对象</param>
		/// <returns>拷贝的结果内容</returns>
		public static T[] CopyArray<T>(this T[] value)
        {
            if (value == null) return null;
            T[] buffer = new T[value.Length];
            Array.Copy(value, buffer, value.Length);
            return buffer;
        }
        public static string ToArrayString<T>(this T[] value) => ToolBasic.ArrayFormat(value);
        public static string ToArrayString<T>(this T[] value, string format) => ToolBasic.ArrayFormat(value, format);
        /// <summary>
        /// 将字符串数组转为实际的数据数组
        /// 例如字符串格式[1,2,3,4,5]，可以转成实际的数组对象
        /// </summary>
        /// <typeparam name="T">类型对象</typeparam>
        /// <param name="value">字符串数据</param>
        /// <param name="selector">转换方法</param>
        /// <returns></returns>
        public static T[] ToStringArray<T>(this string value, Func<string, T> selector)
        {
            if (value.IndexOf('[') >= 0) value = value.Replace("[", "");
            if (value.IndexOf(']') >= 0) value = value.Replace("]", "");

            string[] splits = value.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            return splits.Select(selector).ToArray();
        }
        /// <summary>
        /// 将字符串数组转为实际的数据数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T[] ToStringArray<T>(this string value)
        {
            Type type = typeof(T);
            if (type == typeof(byte)) return (T[])(object)value.ToStringArray(byte.Parse);
            else if (type == typeof(sbyte)) return (T[])(object)value.ToStringArray(sbyte.Parse);
            else if (type == typeof(bool)) return (T[])(object)value.ToStringArray(bool.Parse);
            else if (type == typeof(short)) return (T[])(object)value.ToStringArray(short.Parse);
            else if (type == typeof(ushort)) return (T[])(object)value.ToStringArray(ushort.Parse);
            else if (type == typeof(int)) return (T[])(object)value.ToStringArray(int.Parse);
            else if (type == typeof(uint)) return (T[])(object)value.ToStringArray(uint.Parse);
            else if (type == typeof(long)) return (T[])(object)value.ToStringArray(long.Parse);
            else if (type == typeof(ulong)) return (T[])(object)value.ToStringArray(ulong.Parse);
            else if (type == typeof(float)) return (T[])(object)value.ToStringArray(float.Parse);
            else if (type == typeof(double)) return (T[])(object)value.ToStringArray(double.Parse);
            else if (type == typeof(DateTime)) return (T[])(object)value.ToStringArray(DateTime.Parse);
            else if (type == typeof(string)) return (T[])(object)value.ToStringArray(m => m);
            else throw new Exception("use ToArray<T>(Func<string,T>) method instead");
        }

        public static string ToHexString(this byte[] InBytes) 
            => ToolBasic.Byte2HexString(InBytes);
        public static string ToHexString(this byte[] InBytes, char segment) 
            => ToolBasic.Byte2HexString(InBytes, segment);
        public static string ToHexString(this byte[] InBytes, char segment, int newLineCount)
            => ToolBasic.Byte2HexString(InBytes, segment, newLineCount);
        public static byte[] ToHexBytes(this string value)
            => ToolBasic.HexString2Bytes(value);

        public static bool GetBoolOnIndex(this byte value, int offset) 
            => ToolBasic.BoolOnByteIndex(value, offset);
        public static byte[] ToByteArray(this bool[] array)
            => ToolBasic.BoolArrayToByte(array);
        public static bool[] ToBoolArray(this byte[] InBytes, int length) 
            => ToolBasic.ByteToBoolArray(InBytes, length);
        public static bool[] ToBoolArray(this byte[] InBytes) 
            => ToolBasic.ByteToBoolArray(InBytes);

        public static bool GetBoolValue(this byte[] bytes, int bytIndex, int boolIndex)
            => ToolBasic.BoolOnByteIndex(bytes[bytIndex], boolIndex);
        public static bool GetBoolByIndex(this byte[] bytes, int boolIndex)
            => ToolBasic.BoolOnByteIndex(bytes[boolIndex / 8], boolIndex % 8);
        public static bool GetBoolByIndex(this byte byt, int boolIndex)
            => ToolBasic.BoolOnByteIndex(byt, boolIndex % 8);
        public static byte SetBoolByIndex(this byte byt, int boolIndex, bool value)
            => ToolBasic.SetBoolOnByteIndex(byt, boolIndex, value);

        public static T[] SelectMiddle<T>(this T[] value, int index, int length) 
            => ToolBasic.ArraySelectMiddle(value, index, length);
        public static T[] SelectBegin<T>(this T[] value, int length)
            => ToolBasic.ArraySelectBegin(value, length);
        public static T[] SelectLast<T>(this T[] value, int length)
            => ToolBasic.ArraySelectLast(value, length);
        public static T[] RemoveDouble<T>(this T[] value, int leftLength, int rightLength)
            => ToolBasic.ArrayRemoveDouble(value, leftLength, rightLength);
        public static T[] RemoveBegin<T>(this T[] value, int length)
            => ToolBasic.ArrayRemoveBegin(value, length);
        public static T[] RemoveLast<T>(this T[] value, int length) 
            => ToolBasic.ArrayRemoveLast(value, length);

        /// <summary>
        /// 将指定数据添加到数组的每个元素上
        /// 会修改原数组，不适用byte类型
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="array">原始数据</param>
        /// <param name="value">数据值</param>
        /// <returns></returns>
        public static T[] IncreaseBy<T>(this T[] array, T value)
        {
            if (typeof(T) == typeof(byte))
            {
                ParameterExpression firstArg = Expression.Parameter(typeof(int), "first");
                ParameterExpression secondArg = Expression.Parameter(typeof(int), "second");
                Expression body = Expression.Add(firstArg, secondArg);

                Expression<Func<int, int, int>> adder = Expression.Lambda<Func<int, int, int>>(body, firstArg, secondArg);
                Func<int, int, int> addDelegate = adder.Compile();
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = (T)(object)(byte)addDelegate(Convert.ToInt32(array[i]), Convert.ToInt32(value));
                }
            }
            else
            {
                ParameterExpression firstArg = Expression.Parameter(typeof(T), "first");
                ParameterExpression secondArg = Expression.Parameter(typeof(T), "second");
                Expression body = Expression.Add(firstArg, secondArg);

                Expression<Func<T, T, T>> adder = Expression.Lambda<Func<T, T, T>>(body, firstArg, secondArg);
                Func<T, T, T> addDelegate = adder.Compile();
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = addDelegate(array[i], value);
                }
            }
            return array;
        }

        /// <summary>
        /// 启动接收数据，需要传入回调方法，传递对象
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="callback"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static M_OperateResult BeginReceiveResult(this Socket socket, AsyncCallback callback, object obj)
        {
            try
            {
                socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, callback, obj);
                return M_OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                socket?.Close();
                return new M_OperateResult(ex.Message);
            }
        }
        /// <summary>
        /// 启动接收数据，需要传入回调方法，传递对象默认为socket本身
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static M_OperateResult BeginReceiveResult(this Socket socket, AsyncCallback callback)
        {
            return BeginReceiveResult(socket, callback, socket);
        }
        /// <summary>
        /// 结束挂起的异步读取，返回读取的字节数
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ar"></param>
        /// <returns></returns>
        public static M_OperateResult<int> EndReceiveResult(this Socket socket, IAsyncResult ar)
        {
            try
            {
                return M_OperateResult.CreateSuccessResult(socket.EndReceive(ar));
            }
            catch (Exception ex)
            {
                socket?.Close();
                return new M_OperateResult<int>(ex.Message);
            }
        }

        /// <summary>
        /// 据英文小数点进行切割字符串
        /// 例如 "100.1" 返回 ["100","1"]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] SplitDot(this string str) => str.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// 当前对象的JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        //public static string ToJsonString(this object obj, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.Indented) => Newtonsoft.Json.JsonConvert.SerializeObject(obj, formatting);
    }
}
