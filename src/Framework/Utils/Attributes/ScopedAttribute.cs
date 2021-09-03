using System;

namespace Wyn.Utils.Attributes
{
    /// <summary>
    /// 作用域注入(使用该特性的服务系统会自动注入)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ScopedAttribute : Attribute
    {
        /// <summary>
        /// 是否使用自身的类型进行注入
        /// </summary>
        public bool IsSelf { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public ScopedAttribute() {}

        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="isSelf">是否使用自身的类型进行注入，默认不使用</param>
        public ScopedAttribute(bool isSelf = false) => IsSelf = isSelf;
    }
}
