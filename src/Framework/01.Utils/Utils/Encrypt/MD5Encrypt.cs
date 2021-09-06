using System.IO;
using System.Security.Cryptography;
using System.Text;

using Wyn.Utils.Annotations;
using Wyn.Utils.Extensions;

namespace Wyn.Utils.Encrypt
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public static class Md5Encrypt
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">加密字符串</param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns></returns>
        public static string Encrypt(string source, bool lowerCase = false) => source.IsNull() ? null : Encrypt(Encoding.UTF8.GetBytes(source), lowerCase);

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">加密字节流</param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns></returns>
        public static string Encrypt(byte[] source, bool lowerCase = false)
        {
            if (source == null) return null;

            using var md5Hash = MD5.Create();
            return md5Hash.ComputeHash(source).ToHex(lowerCase);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">流</param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns></returns>
        public static string Encrypt(Stream source, bool lowerCase = false)
        {
            if (source == null) return null;

            using var md5Hash = MD5.Create();
            return md5Hash.ComputeHash(source).ToHex(lowerCase);
        }
    }
}
