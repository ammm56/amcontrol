using ProtocolLib.CommonLib.Interface;
using ProtocolLib.CommonLib.Model;
using ProtocolLib.CommonLib.Model.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProtocolLib.CommonLib.Common
{
    /// <summary>
    /// 常用静态方法
    /// </summary>
    public class ToolBasic
    {

        /// <summary>
        /// 多个数组数据连接成一条数组数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static T[] ArraySplice<T>(params T[][] arrays)
        {
            int count = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                if (arrays[i]?.Length > 0)
                {
                    count += arrays[i].Length;
                }
            }
            int index = 0;
            T[] buffer = new T[count];
            for (int i = 0; i < arrays.Length; i++)
            {
                if (arrays[i]?.Length > 0)
                {
                    arrays[i].CopyTo(buffer, index);
                    index += arrays[i].Length;
                }
            }
            return buffer;
        }

        /// <summary>
        /// 数组前后移除指定位数，返回新数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">数组</param>
        /// <param name="leftlenght">前面的位数</param>
        /// <param name="rightlength">后面的位数</param>
        /// <returns></returns>
        public static T[] ArrayRemoveLR<T>(T[] value,int leftlenght,int rightlength)
        {
            if (value == null) return null;
            if (value.Length <= (leftlenght + rightlength)) return new T[0];

            T[] buffer = new T[value.Length - leftlenght - rightlength];
            Array.Copy(value, leftlenght, buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// 获得唯一GUID
        /// 长度36字节
        /// </summary>
        /// <returns></returns>
        public static string GetGUID()
        {
            return $"{Guid.NewGuid().ToString("N")}{new Random().Next(1000, 10000)}";
        }

        #region 数组格式化
        /// <summary>
        /// 将数组格式化为显示的字符串信息
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="array">数组信息</param>
        /// <returns></returns>
        public static string ArrayFormat<T>(T[] array) => ArrayFormat(array, string.Empty);
        /// <summary>
        /// 将数组格式化为显示的字符串信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ArrayFormat<T>(T[] array,string format)
        {
            if (array == null) return "NULL";
            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(string.IsNullOrEmpty(format) ? array[i].ToString() : string.Format(format, array[i]));
                if(i != array.Length - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }
        /// <summary>
        /// 将数组格式化为显示的字符串信息，去掉中括号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ArrayFormatValue<T>(T[] array) => ArrayFormatValue(array, string.Empty);
        /// <summary>
        /// 将数组格式化为显示的字符串信息，去掉中括号
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ArrayFormatValue<T>(T[] array, string format)
        {
            if (array == null) return "NULL";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(string.IsNullOrEmpty(format) ? array[i].ToString() : string.Format(format, array[i]));
                if (i != array.Length - 1)
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 将数组格式化为显示的字符串信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ArrayFormat<T>(T array) => ArrayFormat(array, string.Empty);
        /// <summary>
        /// 将数组格式化为显示的字符串信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ArrayFormat<T>(T array,string format)
        {
            StringBuilder sb = new StringBuilder("[");
            if (array is Array array1)
            {
                foreach (var item in array1)
                {
                    sb.Append(string.IsNullOrEmpty(format) ? item.ToString() : string.Format(format, item));
                    sb.Append(",");
                }
                if (array1.Length > 0 && sb[sb.Length - 1] == ',') sb.Remove(sb.Length - 1, 1);
            }
            else
            {
                sb.Append(string.IsNullOrEmpty(format) ? array.ToString() : string.Format(format, array));
            }
            sb.Append("]");
            return sb.ToString();
        }
        #endregion

        #region 数组扩展

        /// <summary>
        /// 一个通用的数组新增个数方法，会自动判断越界情况，越界的情况下，会自动的截断或是填充
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="array">原数据</param>
        /// <param name="data">等待新增的数据</param>
        /// <param name="max">原数据的最大值</param>
        public static void AddArrayData<T>(ref T[] array, T[] data, int max)
        {
            if (data == null) return;           // 数据为空
            if (data.Length == 0) return;       // 数据长度为空

            if (array.Length == max)
            {
                Array.Copy(array, data.Length, array, 0, array.Length - data.Length);
                Array.Copy(data, 0, array, array.Length - data.Length, data.Length);
            }
            else
            {
                if ((array.Length + data.Length) > max)
                {
                    T[] tmp = new T[max];
                    for (int i = 0; i < (max - data.Length); i++)
                    {
                        tmp[i] = array[i + (array.Length - max + data.Length)];
                    }
                    for (int i = 0; i < data.Length; i++)
                    {
                        tmp[tmp.Length - data.Length + i] = data[i];
                    }
                    // 更新数据
                    array = tmp;
                }
                else
                {
                    T[] tmp = new T[array.Length + data.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        tmp[i] = array[i];
                    }
                    for (int i = 0; i < data.Length; i++)
                    {
                        tmp[tmp.Length - data.Length + i] = data[i];
                    }

                    array = tmp;
                }
            }
        }

        /// <summary>
        /// 将一个数组扩充或缩短到指定长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] ArrayExpandToLength<T>(T[] data, int length)
        {
            if (data == null) return new T[length];
            if (data.Length == length) return data;
            T[] buffer = new T[length];
            Array.Copy(data, buffer, Math.Min(data.Length, buffer.Length));
            return buffer;
        }

        /// <summary>
        /// 将一个数组扩充到偶数长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T[] ArrayExpandToLengthEven<T>(T[] data)
        {
            if (data == null) return new T[0];
            if (data.Length % 2 == 1)
            {
                return ArrayExpandToLength(data, data.Length + 1);
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// 将指定的数据按照指定长度进行分割，例如int[10]，指定长度4，就分割成int[4],int[4],int[2]，然后拼接list
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">等待分割的数组</param>
        /// <param name="length">指定的长度信息</param>
        /// <returns>分割后结果内容</returns>
        /// </example>
        public static List<T[]> ArraySplitByLength<T>(T[] array, int length)
        {
            if (array == null) return new List<T[]>();

            List<T[]> result = new List<T[]>();
            int index = 0;
            while (index < array.Length)
            {
                if (index + length < array.Length)
                {
                    T[] tmp = new T[length];
                    Array.Copy(array, index, tmp, 0, length);
                    index += length;
                    result.Add(tmp);
                }
                else
                {
                    T[] tmp = new T[array.Length - index];
                    Array.Copy(array, index, tmp, 0, tmp.Length);
                    index += length;
                    result.Add(tmp);
                }
            }
            return result;
        }

        /// <summary>
        /// 将整数进行有效的拆分成数组，指定每个元素的最大值
        /// </summary>
        /// <param name="integer">整数信息</param>
        /// <param name="everyLength">单个的数组长度</param>
        /// <returns>拆分后的数组长度</returns>
        /// </example>
        public static int[] SplitIntegerToArray(int integer, int everyLength)
        {
            int[] result = new int[(integer / everyLength) + ((integer % everyLength) == 0 ? 0 : 1)];
            for (int i = 0; i < result.Length; i++)
            {
                if (i == result.Length - 1)
                {
                    result[i] = (integer % everyLength) == 0 ? everyLength : (integer % everyLength);
                }
                else
                {
                    result[i] = everyLength;
                }
            }
            return result;
        }

        #endregion

        #region 地址解析
        /// <summary>
        /// 解析地址附加参数
        /// s=100;D100
        /// </summary>
        /// <param name="address"></param>
        /// <param name="paraName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ExtractParameter(ref string address, string paraName, int defaultValue)
        {
            M_OperateResult<int> extra = ExtractParameter(ref address, paraName);
            return extra.IsSuccess ? extra.Content : defaultValue;
        }
        /// <summary>
        /// 解析地址附加参数
        /// s=100;D100
        /// </summary>
        /// <param name="address"></param>
        /// <param name="paraName"></param>
        /// <returns></returns>
        public static M_OperateResult<int> ExtractParameter(ref string address, string paraName)
        {
            try
            {
                Match match = Regex.Match(address, paraName + "=[0-9A-Fa-fx]+;");
                if (!match.Success) return new M_OperateResult<int>($"Address [{address}] can't find [{paraName}] Parameters. for example : {paraName}=1;100");

                string number = match.Value.Substring(paraName.Length + 1, match.Value.Length - paraName.Length - 2);
                int value = number.StartsWith("0x") ? Convert.ToInt32(number.Substring(2), 16) : number.StartsWith("0") ? Convert.ToInt32(number, 8) : Convert.ToInt32(number);

                address = address.Replace(match.Value, "");
                return M_OperateResult.CreateSuccessResult(value);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<int>($"Address [{address}] Get [{paraName}] Parameters failed: " + ex.Message);
            }
        }
        /// <summary>
        /// 解析地址起始地址的方法
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int ExtractStartIndex(ref string address)
        {
            try
            {
                Match match = Regex.Match(address, "\\[[0-9]+\\]$");
                if (!match.Success) return -1;

                string number = match.Value.Substring(1, match.Value.Length - 2);
                int value = Convert.ToInt32(number);

                address = address.Remove(address.Length - match.Value.Length);
                return value;
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 解析地址附加参数
        /// </summary>
        /// <param name="address"></param>
        /// <param name="defaultTransform"></param>
        /// <returns></returns>
        public static IByteTransform ExtractTransformParameter(ref string address, IByteTransform defaultTransform)
        {
            try
            {
                string paraName = "format";
                Match match = Regex.Match(address, paraName + "=(ABCD|BADC|DCBA|CDAB);", RegexOptions.IgnoreCase);
                if (!match.Success) return defaultTransform;

                string format = match.Value.Substring(paraName.Length + 1, match.Value.Length - paraName.Length - 2);
                E_DataFormat dataFormat = defaultTransform.DataFormat;

                switch (format.ToUpper())
                {
                    case "ABCD": dataFormat = E_DataFormat.ABCD; break;
                    case "BADC": dataFormat = E_DataFormat.BADC; break;
                    case "DCBA": dataFormat = E_DataFormat.DCBA; break;
                    case "CDAB": dataFormat = E_DataFormat.CDAB; break;
                    default: break;
                }

                address = address.Replace(match.Value, "");
                if (dataFormat != defaultTransform.DataFormat) return defaultTransform.CreateByDataFormat(dataFormat);
                return defaultTransform;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 切割当前的地址数据信息，根据读取的长度来分割成多次不同的读取内容，需要指定地址，总的读取长度，切割读取长度
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <param name="segment"></param>
        /// <returns></returns>
        public static M_OperateResult<int[], int[]> SplitReadLength(int address, ushort length, ushort segment)
        {
            int[] segments = ToolBasic.SplitIntegerToArray(length, segment);
            int[] addresses = new int[segments.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                if (i == 0) addresses[i] = address;
                else addresses[i] = addresses[i - 1] + segments[i - 1];
            }
            return M_OperateResult.CreateSuccessResult(addresses, segments);
        }
        /// <summary>
        /// 获取地址信息的位索引，在地址最后一个小数点的位置
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int GetBitIndexInformation(ref string address)
        {
            int bitIndex = 0;
            int lastIndex = address.LastIndexOf('.');
            if (lastIndex > 0 && lastIndex < address.Length - 1)
            {
                string bit = address.Substring(lastIndex + 1);
                if (bit.Contains("A") || bit.Contains("B") || bit.Contains("C") || bit.Contains("D") || bit.Contains("E") || bit.Contains("F"))
                {
                    bitIndex = Convert.ToInt32(bit, 16);
                }
                else
                {
                    bitIndex = Convert.ToInt32(bit);
                }
                address = address.Substring(0, lastIndex);
            }
            return bitIndex;
        }
        /// <summary>
        /// 从当前的字符串信息获取IP地址数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetIpAddressFromInput(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // 正则表达值校验Ip地址
                if (Regex.IsMatch(value, @"^[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+$"))
                {
                    if (!IPAddress.TryParse(value, out IPAddress address))
                    {
                        throw new Exception(CommonResources.Get.Language.Language.ipaddresserror);
                    }
                    return value;
                }
                else
                {
                    IPHostEntry host = Dns.GetHostEntry(value);
                    IPAddress[] iPs = host.AddressList;
                    if (iPs.Length > 0) return iPs[0].ToString();
                }
            }
            return "127.0.0.1";
        }
        /// <summary>
        /// 从流中接收指定长度的字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] ReadSpecifiedLengthFromStream(Stream stream, int length)
        {
            byte[] buffer = new byte[length];
            int receive = 0;
            while (receive < length)
            {
                int count = stream.Read(buffer, receive, buffer.Length - receive);
                receive += count;
                if (count == 0) break;
            }
            return buffer;
        }
        /// <summary>
        /// 将字符串的内容写入到流中去
        /// </summary>
        /// <param name="stream">数据流</param>
        /// <param name="value">字符串内容</param>
        public static void WriteStringToStream(Stream stream, string value)
        {
            byte[] buffer = string.IsNullOrEmpty(value) ? new byte[0] : Encoding.UTF8.GetBytes(value);
            WriteBinaryToStream(stream, buffer);
        }
        /// <summary>
        /// 将二进制内容写入到数据流中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteBinaryToStream(Stream stream, byte[] value)
        {
            stream.Write(BitConverter.GetBytes(value.Length), 0, 4);
            stream.Write(value, 0, value.Length);
        }
        /// <summary>
        /// 从流中读取一个字符串内容
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadStringFromStream(Stream stream)
        {
            byte[] buffer = ReadBinaryFromStream(stream);
            return Encoding.UTF8.GetString(buffer);
        }
        /// <summary>
        /// 从流中读取二进制内容
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadBinaryFromStream(Stream stream)
        {
            byte[] lengthBuffer = ReadSpecifiedLengthFromStream(stream, 4);
            int length = BitConverter.ToInt32(lengthBuffer, 0);
            if (length <= 0) return new byte[0];
            return ReadSpecifiedLengthFromStream(stream, length);
        }
        /// <summary>
        /// 从字符串的内容提取UTF8编码的字节
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] GetUTF8Bytes(string message)
        {
            return string.IsNullOrEmpty(message) ? new byte[0] : Encoding.UTF8.GetBytes(message);
        }
        /// <summary>
        /// 路径合并
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }
        /// <summary>
        /// 根据当前的位偏移地址及读取位长度信息，计算出实际的字节索引，字节数，字节位偏移
        /// </summary>
        /// <param name="addressStart"></param>
        /// <param name="length"></param>
        /// <param name="newStart"></param>
        /// <param name="byteLength"></param>
        /// <param name="offset"></param>
        public static void CalculateStartBitIndexAndLength(int addressStart, ushort length, out int newStart, out ushort byteLength, out int offset)
        {
            byteLength = (ushort)((addressStart + length - 1) / 8 - addressStart / 8 + 1);
            offset = addressStart % 8;
            newStart = addressStart - offset;
        }
        /// <summary>
        /// 根据字符串内容，获取当前的位索引地址，例如输入 6,返回6，输入15，返回15，输入B，返回11
        /// </summary>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static int CalculateBitStartIndex(string bit)
        {
            if (bit.Contains("A") || bit.Contains("B") || bit.Contains("C") || bit.Contains("D") || bit.Contains("E") || bit.Contains("F"))
            {
                return Convert.ToInt32(bit, 16);
            }
            else
            {
                return Convert.ToInt32(bit);
            }
        }


        #endregion

        #region 数组剪裁
        /// <summary>
        /// 将一个数组的前后移除指定位数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">数组</param>
        /// <param name="leftLength">前面的位数</param>
        /// <param name="rightLength">后面的位数</param>
        /// <returns></returns>
        public static T[] ArrayRemoveDouble<T>(T[] value, int leftLength, int rightLength)
        {
            if (value == null) return null;
            if (value.Length <= (leftLength + rightLength)) return new T[0];

            T[] buffer = new T[value.Length - leftLength - rightLength];
            Array.Copy(value, leftLength, buffer, 0, buffer.Length);

            return buffer;
        }
        /// <summary>
        /// 将一个数组的前面指定位数移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] ArrayRemoveBegin<T>(T[] value, int length) => ArrayRemoveDouble(value, length, 0);
        /// <summary>
        /// 将一个数组的后面指定位数移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] ArrayRemoveLast<T>(T[] value, int length) => ArrayRemoveDouble(value, 0, length);
        /// <summary>
        /// 获得数组中间指定长度的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">数组</param>
        /// <param name="index">起始索引</param>
        /// <param name="length">数据长度</param>
        /// <returns></returns>
        public static T[] ArraySelectMiddle<T>(T[] value, int index, int length)
        {
            if (value == null) return null;
            T[] buffer = new T[Math.Min(value.Length, length)];
            Array.Copy(value, index, buffer, 0, buffer.Length);
            return buffer;
        }
        /// <summary>
        /// 选择数组前几个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] ArraySelectBegin<T>(T[] value, int length)
        {
            T[] buffer = new T[Math.Min(value.Length, length)];
            if (buffer.Length > 0) Array.Copy(value, 0, buffer, 0, length);
            return buffer;
        }
        /// <summary>
        /// 选择数组后面的几个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] ArraySelectLast<T>(T[] value, int length)
        {
            T[] buffer = new T[Math.Min(value.Length, length)];
            Array.Copy(value, value.Length - length, buffer, 0, buffer.Length);
            return buffer;
        }


        #endregion

        #region 按字反转字节数组
        /// <summary>
        /// 将byte数组按照双字节进行反转
        /// </summary>
        /// <param name="inBytes">输入的字节数组</param>
        /// <returns></returns>
        public static byte[] BytesReverseByWord(byte[] inBytes)
        {
            if (inBytes == null) return null;
            if (inBytes.Length == 0) return new byte[0];
            byte[] buffer = ArrayExpandToLengthEven(inBytes.CopyArray());
            for (int i = 0; i < buffer.Length / 2; i++)
            {
                byte temp = buffer[i * 2 + 0];
                buffer[i * 2 + 0] = buffer[i * 2 + 1];
                buffer[i * 2 + 1] = temp;
            }
            return buffer;
        }


        #endregion

        #region 字节数组和ASCII字节数组转换
        /// <summary>
        /// byte[] -> ascii byte[]
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        public static byte[] Bytes2AsciiBytes(byte[] inBytes) => Encoding.ASCII.GetBytes(Byte2HexString(inBytes));
        /// <summary>
        /// ascii byte[] -> byte[]
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        public static byte[] AsciiBytes2Bytes(byte[] inBytes) => HexString2Bytes(Encoding.ASCII.GetString(inBytes));
        /// <summary>
        /// 从字节构建一个ascii字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiBytes4Byte(byte value) => Encoding.ASCII.GetBytes(value.ToString("X2"));
        /// <summary>
        /// 从int16构建一个ascii字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiBytes4Int16(short value) => Encoding.ASCII.GetBytes(value.ToString("X4"));
        /// <summary>
        /// 从uint16构建一个ascii字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiBytes4UInt16(ushort value) => Encoding.ASCII.GetBytes(value.ToString("X4"));
        /// <summary>
        /// 从int32构建一个ascii字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiBytes4Int32(uint value) => Encoding.ASCII.GetBytes(value.ToString("X8"));
        /// <summary>
        /// 从uint32构建一个ascii字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiBytes4UInt32(uint value) => Encoding.ASCII.GetBytes(value.ToString("X8"));
        /// <summary>
        /// 从byte[]构建一个ascii字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] BuildAsciiBytes4Bytes(byte[] value)
        {
            byte[] buffer = new byte[value.Length * 2];
            for (int i = 0; i < value.Length; i++)
            {
                ToolBasic.BuildAsciiBytes4Byte(value[i]).CopyTo(buffer, 2 * i);
            }
            return buffer;
        }


        #endregion

        #region bool byte转换
        private static byte GetDataByBitIndex(int offset)
        {
            switch (offset)
            {
                case 0: return 0x01;
                case 1: return 0x02;
                case 2: return 0x04;
                case 3: return 0x08;
                case 4: return 0x10;
                case 5: return 0x20;
                case 6: return 0x40;
                case 7: return 0x80;
                default: return 0;
            }
        }
        /// <summary>
        /// 获取byte数据类型的第offset位，是否为True
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static bool BoolOnByteIndex(byte value,int offset)
        {
            byte temp = GetDataByBitIndex(offset);
            return (value & temp) == temp;
        }
        /// <summary>
        /// 设置byte数据的第offset位是否为True
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte SetBoolOnByteIndex(byte data,int offset,bool value)
        {
            byte temp = GetDataByBitIndex(offset);
            if (value) return (byte)(data | temp);
            return (byte)(data & (~temp));
        }
        /// <summary>
        /// 将bool数组转换到byte数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static byte[] BoolArrayToByte(bool[] array)
        {
            if (array == null) return null;
            int length = array.Length % 8 == 0 ? array.Length / 8 : array.Length / 8 + 1;
            byte[] buffer = new byte[length];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    buffer[i / 8] += GetDataByBitIndex(i % 8);
                }
            }
            return buffer;
        }
        /// <summary>
        /// 从字节数组中提取所有的位数组
        /// </summary>
        /// <param name="InBytes"></param>
        /// <returns></returns>
        public static bool[] ByteToBoolArray(byte[] InBytes)
        {
            return InBytes == null ? null : ByteToBoolArray(InBytes, InBytes.Length * 8);
        }
        /// <summary>
        /// 从字节数组中提取位数组
        /// </summary>
        /// <param name="InBytes">原先的字节数组</param>
        /// <param name="length">要转换的长度</param>
        /// <returns>转换后的bool数组</returns>
        public static bool[] ByteToBoolArray(byte[] InBytes,int length)
        {
            if (InBytes == null) return null;
            if(length > InBytes.Length * 8)
            {
                length = InBytes.Length * 8;
            }
            bool[] buffer = new bool[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = BoolOnByteIndex(InBytes[i / 8], i % 8);
            }

            return buffer;
        }


        #endregion

        #region 字节数组转16进制字符串
        /// <summary>
        /// 字节数据转化成16进制表示字符串
        /// </summary>
        /// <param name="InBytes"></param>
        /// <returns></returns>
        public static string Byte2HexString(byte[] InBytes) => Byte2HexString(InBytes, (char)0);
        public static string Byte2HexString(byte[] InBytes, char segment) => Byte2HexString(InBytes, segment, 0);
        /// <summary>
        /// 字节数据转化成16进制表示的字符串
        /// </summary>
        /// <param name="InBytes">字节数组</param>
        /// <param name="segment">分割符</param>
        /// <param name="newLineCount">每隔指定数量的时候进行换行</param>
        /// <returns>返回的字符串</returns>
        public static string Byte2HexString(byte[] InBytes,char segment,int newLineCount)
        {
            if (InBytes == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            long tick = 0;
            foreach (byte InByte in InBytes)
            {
                if(segment == 0)
                {
                    sb.Append(string.Format("{0:X2}", InByte));
                }
                else
                {
                    sb.Append(string.Format("{0:X2}{1}", InByte, segment));
                }
                tick++;
                if(newLineCount > 0 && tick >= newLineCount)
                {
                    sb.Append(Environment.NewLine);
                    tick = 0;
                }
            }
            if(segment != 0 && sb.Length > 1 && sb[sb.Length -1] == segment)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 字符串转16进制表示的字符串
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string Byte2HexString(string inString) => Byte2HexString(Encoding.Unicode.GetBytes(inString));
        private static int GetHexCharIndex(char ch)
        {
            switch (ch)
            {
                case '0': return 0;
                case '1': return 1;
                case '2': return 2;
                case '3': return 3;
                case '4': return 4;
                case '5': return 5;
                case '6': return 6;
                case '7': return 7;
                case '8': return 8;
                case '9': return 9;
                case 'A':
                case 'a': return 10;
                case 'B':
                case 'b': return 11;
                case 'C':
                case 'c': return 12;
                case 'D':
                case 'd': return 13;
                case 'E':
                case 'e': return 14;
                case 'F':
                case 'f': return 15;
                default: return -1;
            }
        }

        /// <summary>
        /// 将16进制字符串转成字节数组
        /// 每两个字符转换
        /// </summary>
        /// <param name="hex">16进制字符串 AA 01 02 BB</param>
        /// <returns>转换后字节数组</returns>
        public static byte[] HexString2Bytes(string hex)
        {
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < hex.Length; i++)
            {
                if ((i + 1) < hex.Length)
                {
                    if (GetHexCharIndex(hex[i]) >= 0 && GetHexCharIndex(hex[i + 1]) >= 0)
                    {
                        // 这是一个合格的字节数据
                        ms.WriteByte((byte)(GetHexCharIndex(hex[i]) * 16 + GetHexCharIndex(hex[i + 1])));
                        i++;
                    }
                }
            }

            byte[] result = ms.ToArray();
            ms.Dispose();
            return result;
        }


        #endregion

        #region 字节数组拼接
        /// <summary>
        /// 拼接任意个泛型数组为一个总的泛型数组对象
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrays"></param>
        /// <returns></returns>
        public static T[] SpliceArray<T>(params T[][] arrays)
        {
            int count = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                if (arrays[i]?.Length > 0)
                {
                    count += arrays[i].Length;
                }
            }
            int index = 0;
            T[] buffer = new T[count];
            for (int i = 0; i < arrays.Length; i++)
            {
                if (arrays[i]?.Length > 0)
                {
                    arrays[i].CopyTo(buffer, index);
                    index += arrays[i].Length;
                }
            }
            return buffer;
        }

        #endregion

        #region 字符串数组拼接
        /// <summary>
        /// 将一个字符串和字符串数组拼接成一个数组
        /// </summary>
        /// <param name="first"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] SpliceStringArray(string first, string[] array)
        {
            List<string> list = new List<string>();
            list.Add(first);
            list.AddRange(array);
            return list.ToArray();
        }
        /// <summary>
        /// 将两个字符串和字符串数组拼接成一个数组
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] SpliceStringArray(string first,string second, string[] array)
        {
            List<string> list = new List<string>();
            list.Add(first);
            list.Add(second);
            list.AddRange(array);
            return list.ToArray();
        }
        /// <summary>
        /// 将三个字符串和字符串数组拼接成一个数组
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] SpliceStringArray(string first,string second,string third, string[] array)
        {
            List<string> list = new List<string>();
            list.Add(first);
            list.Add(second);
            list.Add(third);
            list.AddRange(array);
            return list.ToArray();
        }



        #endregion

        #region 深度复制
        /// <summary>
        /// 使用序列化方式深度复制一个对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object DeepClone(object source)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter()
                {
                    Context = new System.Runtime.Serialization.StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone)
                };
                binaryFormatter.Serialize(stream, source);
                stream.Position = 0;
                return binaryFormatter.Deserialize(stream);
            }
        }


        #endregion

        #region 字节转换
        /// <summary>
        /// 结果转换操作的基础方法
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="result">源</param>
        /// <param name="trans">实际转换的委托</param>
        /// <returns>转换结果</returns>
        public static M_OperateResult<TResult> GetResultFromBytes<TResult>(M_OperateResult<byte[]> result, Func<byte[],TResult> trans)
        {
            try
            {
                if (result.IsSuccess) return M_OperateResult.CreateSuccessResult(trans(result.Content));
                else return M_OperateResult.CreateFailedResult<TResult>(result);
            }
            catch (Exception ex)
            {
                return new M_OperateResult<TResult>()
                {
                    Message = $"数据转换失败 {ToolBasic.Byte2HexString(result.Content)} : Length={result.Content.Length} {ex.Message}"
                };
            }
        }
        /// <summary>
        /// 结果转换操作的基础方法
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="result">源结果</param>
        /// <returns>转换结果</returns>
        public static M_OperateResult<TResult> GetResultFromArray<TResult>(M_OperateResult<TResult[]> result)
            => GetSuccessResultFromOther(result, m => m[0]);
        /// <summary>
        /// 使用指定的转换方法
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <typeparam name="TIn">输入类型</typeparam>
        /// <param name="result">原始的结果对象</param>
        /// <param name="trans">转换方法，从类型TIn转换拿到TResult的泛型委托</param>
        /// <returns>类型为TResult的对象</returns>
        public static M_OperateResult<TResult> GetSuccessResultFromOther<TResult,TIn>(M_OperateResult<TIn> result,Func<TIn,TResult> trans)
        {
            if(!result.IsSuccess) return M_OperateResult.CreateFailedResult<TResult>(result);
            return M_OperateResult.CreateSuccessResult(trans(result.Content));
        }
        /// <summary>
        /// 使用指定的转换方法获得实际的结果对象内容
        /// </summary>
        /// <typeparam name="TIn">输入类型</typeparam>
        /// <param name="result">原始的结果对象</param>
        /// <param name="trans">转换方法</param>
        /// <returns>类型为TResult的对象</returns>
        public static M_OperateResult GetResultFromOther<TIn>(M_OperateResult<TIn> result,Func<TIn,M_OperateResult> trans)
        {
            if (!result.IsSuccess) return result;
            return trans(result.Content);
        }
        /// <summary>
        /// 使用指定的转换方法获取实际的结果对象内容
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <typeparam name="TIn">输入类型</typeparam>
        /// <param name="result">原始的结果对象</param>
        /// <param name="trans">转换方法，从类型TIn转换拿到M_OperateResult的TResult的泛型委托</param>
        /// <returns>类型为TResult的对象</returns>
        public static M_OperateResult<TResult> GetResultFromOther<TResult,TIn>(
            M_OperateResult<TIn> result,
            Func<TIn,M_OperateResult<TResult>> trans)
        {
            if (!result.IsSuccess) return M_OperateResult.CreateFailedResult<TResult>(result);
            return trans(result.Content);
        }
        public static M_OperateResult<TResult> GetResultFromOther<TResult, TIn1, TIn2>(
            M_OperateResult<TIn1> result,
            Func<TIn1, M_OperateResult<TIn2>> trans1,
            Func<TIn2, M_OperateResult<TResult>> trans2)
        {
            if (!result.IsSuccess) return M_OperateResult.CreateFailedResult<TResult>(result);

            M_OperateResult<TIn2> result1 = trans1(result.Content);
            if (!result1.IsSuccess) return M_OperateResult.CreateFailedResult<TResult>(result1);

            return trans2(result1.Content);
        }


        #endregion

        #region 类型转换

        /// <summary>
        /// 按类型转换值
        /// </summary>
        public static Dictionary<string, Type> dataType = new Dictionary<string, Type>
        {
            {"bool",typeof(bool) },
            {"byte",typeof(byte) },
            {"int16",typeof(short) },
            {"uint16",typeof(ushort) },
            {"int32",typeof(int) },
            {"uint32",typeof(uint) },
            {"int64",typeof(long) },
            {"uint64",typeof(ulong) },
            {"single",typeof(float) },
            {"double",typeof(double) },
            {"string",typeof(string) },

            {"bool[]",typeof(bool[]) },
            {"byte[]",typeof(byte[]) },
            {"int16[]",typeof(short[]) },
            {"uint16[]",typeof(ushort[]) },
            {"int32[]",typeof(int[]) },
            {"uint32[]",typeof(uint[]) },
            {"int64[]",typeof(long[]) },
            {"uint64[]",typeof(ulong[]) },
            {"single[]",typeof(float[]) },
            {"double[]",typeof(double[]) }
        };

        public static object GetComparableValue(string value, string type)
        {
            string dataType = (type ?? "string").ToLowerInvariant();
            string text = value ?? string.Empty;

            switch (dataType)
            {
                case "bool":
                    return ParseBoolean(text) ? (byte)1 : (byte)0;
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
                default:
                    return text;
            }
        }

        private static bool ParseBoolean(string value)
        {
            string text = (value ?? string.Empty).Trim();

            if (text == "1") return true;
            if (text == "0") return false;

            bool result;
            if (bool.TryParse(text, out result))
            {
                return result;
            }

            throw new FormatException("布尔值转换失败:" + value);
        }

        #endregion
    }
}
