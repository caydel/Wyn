using System.ComponentModel.DataAnnotations;

using Wyn.Mod.Admin.Core.Domain.MenuGroup;
using Wyn.Utils.Annotations;

namespace Wyn.Mod.Admin.Core.Application.MenuGroup.Dto
{
    [ObjectMap(typeof(MenuGroupEntity))]
    public class MenuGroupAddDto
    {
        [Required(ErrorMessage = "请填写分组名称")]
        public string Name { get; set; }

        public string Remarks { get; set; }
    }
}
