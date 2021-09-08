using System.ComponentModel.DataAnnotations;

using Wyn.Admin.Core.Domain.DictItem;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.DictItem.Dto
{
    [ObjectMap(typeof(DictItemEntity), true)]
    public class DictItemUpdateDto : DictItemAddDto
    {
        [Required(ErrorMessage = "请选择要修改的字典项")]
        [Range(1, int.MaxValue, ErrorMessage = "请选择要修改的字典项")]
        public int Id { get; set; }
    }
}
