using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wyn.Auth.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using Wyn.Utils.Result;

namespace Wyn.Mod.Admin.Web.Controllers
{
    [SwaggerTag("模块管理")]
    public class ModuleController : BaseController
    {
        private readonly IPermissionResolver _permissionResolver;

        public ModuleController(IPermissionResolver permissionResolver)
        {
            _permissionResolver = permissionResolver;
        }

        /// <summary>
        /// 获取指定模块的权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResultModel Permissions([BindRequired] string moduleCode)
        {
            return ResultModel.Success(_permissionResolver.GetPermissions(moduleCode));
        }
    }
}
