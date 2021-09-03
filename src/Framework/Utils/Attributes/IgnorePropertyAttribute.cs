using System;

namespace Wyn.Utils.Attributes
{
    /// <summary>
    /// 忽略属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}