using System.Linq;
using Wyn.Data.Abstractions.Descriptors;

namespace Wyn.Data.Core.Extensions
{
    internal static class EntityDescriptorExtensions
    {
        /// <summary>
        /// 获取已删除字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetDeletedColumnName(this IEntityDescriptor descriptor) 
            => descriptor.IsSoftDelete ? GetColumnNameByPropertyName(descriptor, "Deleted") : string.Empty;

        /// <summary>
        /// 获取删除时间属性对应字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetDeletedTimeColumnName(this IEntityDescriptor descriptor) 
            => descriptor.IsSoftDelete ? GetColumnNameByPropertyName(descriptor, "DeletedTime") : string.Empty;

        /// <summary>
        /// 获取删除人属性对应字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetDeletedByColumnName(this IEntityDescriptor descriptor)
            => descriptor.IsSoftDelete ? GetColumnNameByPropertyName(descriptor, "DeletedBy") : string.Empty;

        /// <summary>
        /// 获取删除人名称属性对应字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetDeleterColumnName(this IEntityDescriptor descriptor)
            => descriptor.IsSoftDelete ? GetColumnNameByPropertyName(descriptor, "Deleter") : string.Empty;

        /// <summary>
        /// 获取修改人属性对应字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetModifiedByColumnName(this IEntityDescriptor descriptor) 
            => descriptor.IsEntityBase ? GetColumnNameByPropertyName(descriptor, "ModifiedBy") : string.Empty;

        /// <summary>
        /// 获取修改人属性对应字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetModifierColumnName(this IEntityDescriptor descriptor)
            => descriptor.IsEntityBase ? GetColumnNameByPropertyName(descriptor, "Modifier") : string.Empty;

        /// <summary>
        /// 获取修改时间属性对应字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetModifiedTimeColumnName(this IEntityDescriptor descriptor)
            => descriptor.IsEntityBase ? GetColumnNameByPropertyName(descriptor, "ModifiedTime") : string.Empty;

        private static string GetColumnNameByPropertyName(this IEntityDescriptor descriptor, string propertyName) 
            => descriptor.Columns.First(m => m.PropertyInfo.Name.Equals(propertyName)).Name;
    }
}
