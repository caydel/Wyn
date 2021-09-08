using System.ComponentModel.DataAnnotations;

using Wyn.Admin.Core.Domain.MenuGroup;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.MenuGroup.Dto
{
    [ObjectMap(typeof(MenuGroupEntity), true)]
    public class MenuGroupUpdateDto : MenuGroupAddDto
    {
        [Required(ErrorMessage = "请选择要修改的菜单分组")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的菜单分组")]
        public int Id { get; set; }
    }
}
