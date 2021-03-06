using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Wyn.Auth.Abstractions;
using Wyn.Auth.Abstractions.Annotations;
using Wyn.Auth.Abstractions.Options;
using Wyn.Mod.Admin.Core.Application.Account;
using Wyn.Mod.Admin.Core.Application.Account.Dto;
using Swashbuckle.AspNetCore.Annotations;
using Wyn.Utils.Result;

namespace Wyn.Mod.Admin.Web.Controllers
{
    [SwaggerTag("账户管理")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _service;
        private readonly IOptionsMonitor<AuthOptions> _authOptions;
        private readonly IAccount _account;

        public AccountController(IAccountService service, IOptionsMonitor<AuthOptions> authOptions, IAccount account)
        {
            _service = service;
            _authOptions = authOptions;
            _account = account;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <remarks>查询角色列表</remarks>
        [HttpGet]
        public Task<IResultModel> Query([FromQuery] AccountQueryDto dto)
        {
            return _service.Query(dto);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <remarks></remarks>
        [HttpPost]
        public Task<IResultModel> Add(AccountAddDto dto)
        {
            return _service.Add(dto);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<IResultModel> Edit(Guid id)
        {
            return _service.Edit(id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <remarks></remarks>
        [HttpPost]
        public Task<IResultModel> Update(AccountUpdateDto dto)
        {
            return _service.Update(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public Task<IResultModel> Delete([BindRequired] Guid id)
        {
            return _service.Delete(id);
        }

        /// <summary>
        /// 获取账户默认密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResultModel DefaultPassword()
        {
            return ResultModel.Success(_authOptions.CurrentValue.DefaultPassword);
        }

        /// <summary>
        /// 更新账户皮肤配置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowWhenAuthenticated]
        [HttpPost]
        public Task<IResultModel> UpdateSkin(AccountSkinUpdateDto dto)
        {
            dto.AccountId = _account.Id;

            return _service.UpdateSkin(dto);
        }
    }
}
