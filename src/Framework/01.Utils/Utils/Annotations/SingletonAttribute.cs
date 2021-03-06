using System;

namespace Wyn.Utils.Annotations
{
    /// <summary>
    /// 单例注入(使用该特性的服务系统会自动注入)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
        /// <summary>
        /// 是否使用自身的类型进行注入
        /// </summary>
        public bool IsSelf { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public SingletonAttribute() { }

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="isSelf">是否使用自身的类型进行注入，默认不使用</param>
        public SingletonAttribute(bool isSelf = false) => IsSelf = isSelf;
    }
}
