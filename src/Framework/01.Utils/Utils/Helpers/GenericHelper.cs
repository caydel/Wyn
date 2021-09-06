using System;
using System.Collections.Generic;
using System.Linq;

namespace Wyn.Utils.Helpers
{
    public static class GenericHelper
    {
        /// <summary>
        /// 检测对象是否为空
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="message">消息信息</param>
        public static void NotNull<T>(T obj, string parameterName, string message = null)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName, message ?? $"{parameterName} is null");
        }

        /// <summary>
        /// 检测字符串是否为空
        /// </summary>
        /// <param name="obj">指定的字符串</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="message">消息信息</param>
        public static void NotNull(string obj, string parameterName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(obj))
                throw new ArgumentNullException(parameterName, message ?? $"{parameterName} is null");
        }

        /// <summary>
        /// 检测集合是否不为空且包含内容
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">指定的集合</param>
        /// <param name="parameterName">参数名称</param>
        public static void Collection<T>(IList<T> list, string parameterName)
        {
            NotNull(list, parameterName);

            if (!list.Any())
                throw new ArgumentException("集合不包含任何内容", parameterName);
        }
    }
}
