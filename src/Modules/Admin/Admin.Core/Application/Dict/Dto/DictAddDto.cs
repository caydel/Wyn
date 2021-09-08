using System.ComponentModel.DataAnnotations;
using Wyn.Admin.Core.Domain.Dict;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.Dict.Dto
{
    [ObjectMap(typeof(DictEntity))]
    public class DictAddDto
    {
        /// <summary>
        /// 分组编码
        /// </summary>
        [Required(ErrorMessage = "请选择分组")]
        public string GroupCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入名称")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "请输入编码")]
        public string Code { get; set; }
    }
}
