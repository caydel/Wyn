using Wyn.Data.Abstractions.Query;

namespace Wyn.Mod.Admin.Core.Application.MenuGroup.Dto
{
    public class MenuGroupQueryDto : QueryDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
    }
}
