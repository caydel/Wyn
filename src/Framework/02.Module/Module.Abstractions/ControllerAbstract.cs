using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Wyn.Module.Abstractions.Annotations;

namespace Wyn.Module.Abstractions
{
    /// <summary>
    /// 控制器抽象
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "WYN")]
    [ValidateResultFormat]
    public abstract class ControllerAbstract : ControllerBase
    {
        
    }
}
