using System.ComponentModel.DataAnnotations;

using Wyn.Admin.Core.Domain.DictGroup;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.DictGroup.Dto
{
    [ObjectMap(typeof(DictGroupEntity), true)]
    public class DictGroupUpdateDto : DictGroupAddDto
    {
        [Required(ErrorMessage = "请选择要修改的字典分组")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的字典分组")]
        public int Id { get; set; }
    }
}
