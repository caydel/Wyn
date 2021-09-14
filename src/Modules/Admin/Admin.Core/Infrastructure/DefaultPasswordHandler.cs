using System;
using Wyn.Utils.Encrypt;

namespace Wyn.Mod.Admin.Core.Infrastructure
{
    public class DefaultPasswordHandler : IPasswordHandler
    {
        private const string KEY = "mkh_";
        public DefaultPasswordHandler()
        {
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Encrypt(string password)
        {
            return Md5Encrypt.Encrypt(KEY + password);
        }

        public string Decrypt(string encryptPassword)
        {
            throw new NotSupportedException("MD5加密无法解密~");
        }
    }
}
