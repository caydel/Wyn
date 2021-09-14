using Wyn.Data.Abstractions.Annotations;
using Wyn.Data.Abstractions.Entities;

namespace Wyn.Mod.Admin.Core.Domain.Dict
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public partial class DictEntity : EntityBaseSoftDelete
    {
        /// <summary>
        /// 分组编码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Length(100)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Length(100)]
        public string Code { get; set; }
    }
}
