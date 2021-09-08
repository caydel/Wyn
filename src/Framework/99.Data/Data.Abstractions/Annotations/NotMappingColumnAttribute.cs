using System;

namespace Wyn.Data.Abstractions.Annotations
{
    /// <summary>
    /// 指定属性不映射列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMappingColumnAttribute : Attribute
    {
    }
}
