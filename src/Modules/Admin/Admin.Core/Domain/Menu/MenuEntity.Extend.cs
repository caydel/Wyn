using AutoMapper.Configuration.Annotations;

using Wyn.Utils.Annotations;
using Wyn.Utils.Extensions;

namespace Wyn.Mod.Admin.Core.Domain.Menu
{
    public partial class MenuEntity
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        [Ignore]
        public string TypeName => Type.ToDescription();

        /// <summary>
        /// 打开方式名称
        /// </summary>
        [Ignore]
        public string OpenTargetName => OpenTarget.ToDescription();
    }
}
