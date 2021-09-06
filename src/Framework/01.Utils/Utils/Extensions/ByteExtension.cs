using System;
using System.Text;

namespace Wyn.Utils.Extensions
{
    /// <summary>
    /// Byte类的扩展方法
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        /// 转换为16进制字符串
        /// </summary>
        /// <param name="bytes">指定的字节数组</param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns>转换后的字符串</returns>
        public static string ToHex(this byte[] bytes, bool lowerCase = true)
        {
            if (bytes == null)
                return null;

            var result = new StringBuilder();
            var format = lowerCase ? "x2" : "X2";
            foreach (var b in bytes)
            {
                result.Append(b.ToString(format));
            }

            return result.ToString();
        }

        /// <summary>
        /// 转换为Base64
        /// </summary>
        /// <param name="bytes">指定的字节数组</param>
        /// <returns>转换后的字符串</returns>
        public static string ToBase64(this byte[] bytes) => bytes == null ? null : Convert.ToBase64String(bytes);
    }
}
