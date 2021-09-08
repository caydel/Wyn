using Wyn.Data.Abstractions.Annotations;
using Wyn.Data.Abstractions.Entities;

namespace Wyn.Admin.Core.Domain.MenuGroup
{
    /// <summary>
    /// 菜单组
    /// </summary>
    public class MenuGroupEntity : EntityBase
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Length(300)]
        [Nullable]
        public string Remarks { get; set; }
    }
}
