using System.ComponentModel.DataAnnotations;
using Wyn.Admin.Core.Domain.Dict;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.Dict.Dto
{
    [ObjectMap(typeof(DictEntity), true)]
    public class DictUpdateDto : DictAddDto
    {
        [Required(ErrorMessage = "请选择要修改的字典")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的字典")]
        public int Id { get; set; }
    }
}
