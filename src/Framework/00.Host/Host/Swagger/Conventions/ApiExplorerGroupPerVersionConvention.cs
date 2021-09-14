using Microsoft.AspNetCore.Mvc.ApplicationModels;

using Wyn.Utils.Extensions;

namespace Wyn.Host.Web.Swagger.Conventions
{
    /// <summary>
    /// API分组约定
    /// </summary>
    public class ApiExplorerGroupConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.Namespace.IsNull())
                return;

            // 根据约定，控制器的命名空间为Wyn.Mod.XXX.Controllers.XXController,我们需要取第三个元素作为分组名
            string[] array = controller.ControllerType.FullName.Split('.');
            controller.ApiExplorer.GroupName = array[2].ToLower();
        }
    }
}
