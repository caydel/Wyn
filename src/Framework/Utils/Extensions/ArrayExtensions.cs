using System;
using System.Linq;

namespace Wyn.Utils.Extensions
{
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 随机获取指定数组中的一个元素
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="arr">指定数组</param>
        /// <returns>数组中的元素</returns>
        public static T RandomGet<T>(this T[] arr)
        {
            if (arr == null || !arr.Any()) return default;

            var r = new Random();
            return arr[r.Next(arr.Length)];
        }
    }
}
