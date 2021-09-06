using System;

namespace Wyn.Utils.Annotations
{
    /// <summary>
    /// 忽略属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}