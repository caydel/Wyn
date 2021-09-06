using System;

namespace Wyn.Utils.Extensions
{
    /// <summary>
    /// Guid类的扩展方法
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// 判断Guid是否为空
        /// </summary>
        /// <param name="id">指定Guid</param>
        /// <returns>真或假</returns>
        public static bool IsEmpty(this Guid id) => id == Guid.Empty;

        /// <summary>
        /// 判断Guid是否不为空
        /// </summary>
        /// <param name="id">指定Guid</param>
        /// <returns>真或假</returns>
        public static bool NotEmpty(this Guid id) => id != Guid.Empty;

    }
}
