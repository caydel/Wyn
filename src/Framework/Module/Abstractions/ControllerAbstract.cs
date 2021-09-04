using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Wyn.Module.Attributes;

namespace Wyn.Module.Abstractions
{
    /// <summary>
    /// 控制器抽象
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "Wyn")]
    [ValidateResultFormat]
    public abstract class ControllerAbstract : ControllerBase
    {
        
    }
}
