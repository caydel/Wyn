using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Wyn.Utils.Extensions
{
    /// <summary>
    /// String类的扩展方法
    /// </summary>
    public static class StringExtensions
    {

        #region 类型转换

        /// <summary>
        /// 对指定字符串进行指定的类型转换
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="type">数据类型</param>
        /// <returns>转换后的变量</returns>
        public static object ParseTo(this string str, string type)
        {
            return type switch
            {
                "System.Boolean" => ToBoolean(str),
                "System.SByte" => ToSByte(str),
                "System.Byte" => ToByte(str),
                "System.UInt16" => ToUInt16(str),
                "System.Int16" => ToInt16(str),
                "System.uInt32" => ToUInt32(str),
                "System.Int32" => ToInt32(str),
                "System.UInt64" => ToUInt64(str),
                "System.Int64" => ToInt64(str),
                "System.Single" => ToSingle(str),
                "System.Double" => ToDouble(str),
                "System.Decimal" => ToDecimal(str),
                "System.DateTime" => ToDateTime(str),
                "System.Guid" => ToGuid(str),
                _ => throw new NotSupportedException($"The string of \"{str}\" can not be parsed to {type}"),
            };
        }

        /// <summary>
        /// ToByte
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Byte类型变量</returns>
        public static byte? ToByte(this string str)
        {
            if (byte.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToSByte
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的SByte类型变量</returns>
        public static sbyte? ToSByte(this string str)
        {
            if (sbyte.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToInt16
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Int16类型变量</returns>
        public static short? ToInt16(this string str)
        {
            if (short.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToUInt16
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的UInt16类型变量</returns>
        public static ushort? ToUInt16(this string str)
        {
            if (ushort.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToInt32
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Int32类型变量</returns>
        public static int? ToInt32(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            if (int.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToUInt32
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的UInt32类型变量</returns>
        public static uint? ToUInt32(this string str)
        {
            if (uint.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToInt64
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Int64类型变量</returns>
        public static long? ToInt64(this string str)
        {
            if (long.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToUInt64
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的UInt64类型变量</returns>
        public static ulong? ToUInt64(this string str)
        {
            if (ulong.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToSingle
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Single类型变量</returns>
        public static float? ToSingle(this string str)
        {
            if (float.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToDouble
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Double类型变量</returns>
        public static double? ToDouble(this string str)
        {
            if (double.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToDecimal
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Decimal类型变量</returns>
        public static decimal? ToDecimal(this string str)
        {
            if (decimal.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToBoolean
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的bool类型变量</returns>
        public static bool? ToBoolean(this string str)
        {
            if (bool.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToEnum
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Enum类型变量</returns>
        public static T? ToEnum<T>(this string str) where T : struct
        {
            if (Enum.TryParse(str, true, out T outResult) && Enum.IsDefined(typeof(T), outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToGuid
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的Guid类型变量</returns>
        public static Guid? ToGuid(this string str)
        {
            if (Guid.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        /// <summary>
        /// ToDateTime
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>可为null的DateTime类型变量</returns>
        public static DateTime? ToDateTime(this string str)
        {
            if (DateTime.TryParse(str, out var outResult))
                return outResult;

            return null;
        }

        #endregion

        #region 验证方法

        /// <summary>
        /// 判断指定字符串不为空和空格键
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool NotNull(this string str) => !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);


        /// <summary>
        /// 判断指定字符串为空或者空格键
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsNull(this string str) => string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// 返回一个值，该值指示指定的String对象是否出现在指定字符串中
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="value">要搜寻的字符串</param>
        /// <param name="comparisonType">指定搜索规则的枚举值之一</param>
        /// <returns>如果value参数出现在此字符串中则为true；否则为false</returns>
        public static bool Contains(this string str, string value, 
            StringComparison comparisonType = StringComparison.OrdinalIgnoreCase) =>
            str.Contains(value, comparisonType);

        /// <summary>
        /// 判断指定字符串是否为数字
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsInt(this string str) => !string.IsNullOrEmpty(str) && Regex.IsMatch(str, "^-?\\d+$");

        /// <summary>
        /// 判断指定字符串是否为安全SQL语句
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsSafeSql(this string str) => 
            !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");

        /// <summary>
        /// 判断指定字符串是否为合法IP地址
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsIp(this string str) => 
            Regex.IsMatch(str, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        /// <summary>
        /// 判断指定字符串是否为Url地址
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsUrl(this string str) => 
            Regex.IsMatch(str, "(http[s]{0,1}|ftp)://[a-zA-Z0-9\\.\\-]+\\.([a-zA-Z]{2,4})(:\\d+)?(/[a-zA-Z0-9\\.\\-~!@#$%^&*+?:_/=<>]*)?");

        /// <summary>
        /// 判断指定字符串是否合法的日期格式
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsDateTime(this string str) => DateTime.TryParse(str, out _);

        /// <summary>
        /// 判断指定字符串是否为合法Email
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsEmail(this string str) =>
            Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        /// <summary>
        /// 判断指定字符串是否为合法的手机号码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsMobile(this string str) =>
            Regex.IsMatch(str, @"^0{0,1}(1[3-8])[0-9]{9}$");

        /// <summary>
        /// 判断指定字符串是否为合法的身份证号码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>真或假</returns>
        public static bool IsChinaIdCardNumber(string str)
        {
            var idNumber = str;
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            var birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            var arrVerifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            var wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            var ai = idNumber.Remove(17).ToCharArray();
            var sum = 0;

            // 长度验证
            if (idNumber.Length != 18)
                return false;

            // 数字验证
            if (long.TryParse(idNumber.Remove(17), out var number) == false
                || number < Math.Pow(10, 16) 
                || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out _) == false)
                return false;

            // 省份验证
            if (address.IndexOf(idNumber.Remove(2), StringComparison.Ordinal) == -1)
                return false;

            // 生日验证
            if (DateTime.TryParse(birth, out _) == false)
                return false;

            for (var i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }

            Math.DivRem(sum, 11, out var y);
            return arrVerifyCode[y] == idNumber.Substring(17, 1).ToLower();
        }

        #endregion

        #region 编码与解码

        /// <summary>
        /// 对指定字符串进行Base64位编码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>编码后的Base64字符串</returns>
        public static string EncodeBase64(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 对指定字符串进行Base64位解码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string DecodeBase64(this string str)
        {
            var bytes = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 对指定字符串进行HTML编码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>HTML编码的字符串</returns>
        public static string HtmlEncode(this string str) => HttpUtility.HtmlEncode(str);

        /// <summary>
        /// 对指定字符串进行HTML解码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string HtmlDecode(this string str) => HttpUtility.HtmlDecode(str);

        /// <summary>
        /// 对指定字符串进行URL编码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="e">编码</param>
        /// <returns>URL编码的字符串</returns>
        public static string UrlEncode(this string str, Encoding e = null)
        {
            e ??= Encoding.UTF8;
            return HttpUtility.UrlEncode(str, e);
        }

        /// <summary>
        /// 对指定字符串进行URL解码
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="e">编码</param>
        /// <returns>解码后的字符串</returns>
        public static string UrlDecode(this string str, Encoding e = null)
        {
            e ??= Encoding.UTF8;
            return HttpUtility.UrlDecode(str, e);
        }

        #endregion

        #region 截位方法

        /// <summary>
        /// 截取指定字符串
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(this string str, int length) =>
            CutString(str, 0, length);

        /// <summary>
        /// 截取指定字符串
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(this string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex -= length;
                }
                if (startIndex > str.Length)
                    return string.Empty;
            }
            else
            {
                if (length < 0)
                    return string.Empty;
                if (length + startIndex > 0)
                {
                    length += startIndex;
                    startIndex = 0;
                }
                else
                    return string.Empty;
            }
            if (str.Length - startIndex <= length)
            {
                length = str.Length - startIndex;
            }
            try
            {
                return str.Substring(startIndex, length);
            }
            catch
            {
                return str;
            }
        }

        #endregion

        #region 拓展方法

        /// <summary>
        /// 替换空格字符
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="replacement">替换为该字符</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceWhiteSpace(this string str, string replacement = "") =>
            string.IsNullOrEmpty(str) ? null : Regex.Replace(str, "\\s", replacement, RegexOptions.Compiled);


        /// <summary>
        /// 返回指定字符串的真实长度, 1个汉字长度为2
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>指定字符串的真实长度</returns>
        public static int Length(this string str) => string.IsNullOrEmpty(str) ? 0 : Encoding.UTF8.GetBytes(str).Length;


        /// <summary>
        /// 获取默认非空字符串
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="args">依次非空字符串可选项</param>
        /// <returns>默认非空字符串。若无可选项则返回string.Empty</returns>
        public static string DefaultStringIfEmpty(this string str, params string[] args)
        {
            if (!string.IsNullOrEmpty(str)) return str;

            foreach (var item in args)
            {
                if (!string.IsNullOrEmpty(item) && !string.IsNullOrEmpty(item.Trim()))
                {
                    return item;
                }
            }
            return str ?? string.Empty;
        }

        /// <summary>
        /// 与字符串进行比较，忽略大小写
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="value">对比的字符串</param>
        /// <returns>真或假</returns>
        public static bool EqualsIgnoreCase(this string str, string value) => str.Equals(value, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string FirstCharToLower(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return str.First().ToString().ToLower() + str[1..];
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string FirstCharToUpper(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return str.First().ToString().ToUpper() + str[1..];
        }

        /// <summary>
        /// 字符串转换为文件名
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>文件名。</returns>
        public static string ToFileName(this string str) =>
            Regex.Replace(str.ToString(string.Empty), @"[\\/:*?<>|]", "_")
                .Replace("\t", " ").Replace("\r\n", " ").Replace("\"", " ");

        /// <summary>
        /// 隐藏身份证号后六位
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>替换后的字符串</returns>
        public static string HideChinaIdCardNumber(this string str)
        {
            if (str.IsNull()) return "";

            return str.Length != 18 ? str : $"{str.Substring(0, 11)}******";
        }

        /// <summary>
        /// 移除指定字符串的HTML标记
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>移除后的字符串</returns>
        public static string TrimHtml(this string str)
        {
            if (str.IsNull()) return string.Empty;

            //删除脚本和嵌入式CSS   
            str = Regex.Replace(str, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<style[^>]*?>.*?</style>", "", RegexOptions.IgnoreCase);

            //删除HTML   
            var regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            str = regex.Replace(str, "");
            str = Regex.Replace(str, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"-->", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"<!--.*", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            return str.Replace("<", "").Replace(">", "").Replace("\r\n", "");
        }

        /// <summary>
        /// 移除指定字符串的HTML标记，并返回指定长度的文本。(连续空行或空格会被替换为一个)
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <param name="maxLength">返回的文本长度（为0返回所有文本）</param>
        /// <returns>移除后的返回的字符串</returns>
        public static string StripHtml(this string str, int maxLength = 0)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            str = str.Trim();

            str = Regex.Replace(str, "[\\r\\n]{2,}", "<&rn>"); //替换回车和换行为<&rn>，防止下一行代码替换空格的时候被替换掉
            str = Regex.Replace(str, "[\\s]{2,}", " "); //替换 2 个以上的空格为 1 个
            str = Regex.Replace(str, "(<&rn>)+", "\n"); //还原 <&rn> 为 \n
            str = Regex.Replace(str, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;
            str = Regex.Replace(str, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            str = Regex.Replace(str, "<(.|\n)+?>", " ", RegexOptions.IgnoreCase); //any other tags

            if (maxLength > 0 && str.Length > maxLength)
                str = str.Substring(0, maxLength);

            return str;
        }

        /// <summary>
        /// 16进制转字节数组
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>转换后的16进制字节数组</returns>
        public static byte[] HexToBytes(this string str)
        {
            if (str.IsNull()) return null;
            var bytes = new byte[str.Length / 2];

            for (var x = 0; x < str.Length / 2; x++)
            {
                var i = Convert.ToInt32(str.Substring(x * 2, 2), 16);
                bytes[x] = (byte)i;
            }

            return bytes;
        }

        /// <summary>
        /// 16进制数组字符串转换为正常字符串
        /// </summary>
        /// <param name="str">指定字符串</param>
        /// <returns>转换后的正常字符串</returns>
        public static string HexToString(this string str)
        {
            if (str.IsNull()) return null;
            var bytes = str.HexToBytes();

            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

    }
}
