using System.ComponentModel.DataAnnotations;

using Wyn.Admin.Core.Domain.Menu;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.Menu.Dto
{
    [ObjectMap(typeof(MenuEntity), true)]
    public class MenuUpdateDto : MenuAddDto
    {
        [Required(ErrorMessage = "请选择要修改的角色")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的角色")]
        public int Id { get; set; }
    }
}
