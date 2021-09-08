using System.ComponentModel.DataAnnotations;

using Wyn.Admin.Core.Domain.Role;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.Role.Dto
{
    /// <summary>
    /// 角色添加
    /// </summary>
    [ObjectMap(typeof(RoleEntity))]
    public class RoleAddDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入角色名称")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入角色编码")]
        public string Code { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 绑定的菜单分组编号
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请选择绑定的菜单分组")]
        public int MenuGroupId { get; set; }
    }
}
