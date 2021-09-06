using Microsoft.AspNetCore.Mvc;

namespace Wyn.Module.Abstractions
{
    /// <summary>
    /// 模块Mvc配置项配置器接口
    /// 当模块中有需要配置Mvc功能时，可通过实现该接口来配置
    /// </summary>
    public interface IModuleMvcOptionsConfigurator
    {
        /// <summary>
        /// 配置Mvc
        /// </summary>
        /// <param name="mvcOptions"></param>
        void Configure(MvcOptions mvcOptions);
    }
}
