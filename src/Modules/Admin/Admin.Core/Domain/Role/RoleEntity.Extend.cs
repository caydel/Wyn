using Wyn.Data.Abstractions.Annotations;

namespace Wyn.Admin.Core.Domain.Role
{
    public partial class RoleEntity
    {
        /// <summary>
        /// 菜单组名称
        /// </summary>
        [NotMappingColumn]
        public string MenuGroupName { get; set; }
    }
}
