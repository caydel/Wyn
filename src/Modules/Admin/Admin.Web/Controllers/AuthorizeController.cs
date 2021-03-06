using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wyn.Auth.Abstractions;
using Wyn.Auth.Abstractions.Annotations;
using Wyn.Mod.Admin.Core.Application.Authorize;
using Wyn.Mod.Admin.Core.Application.Authorize.Dto;
using Wyn.Mod.Admin.Core.Infrastructure;
using Wyn.Utils.Resolver;
using Wyn.Utils.Result;
using Wyn.Utils.Extensions;

namespace Wyn.Mod.Admin.Web.Controllers
{
    /// <summary>
    /// 身份认证
    /// </summary>
    public class AuthorizeController : BaseController
    {
        private readonly IAuthorizeService _service;
        private readonly IPResolver _ipResolver;
        private readonly IVerifyCodeProvider _verifyCodeProvider;
        private readonly IAccount _account;

        public AuthorizeController(IAuthorizeService service, IPResolver ipResolver, IVerifyCodeProvider verifyCodeProvider, IAccount account)
        {
            _service = service;
            _ipResolver = ipResolver;
            _verifyCodeProvider = verifyCodeProvider;
            _account = account;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Task<IResultModel> VerifyCode()
        {
            return ResultModel.SuccessAsync(_verifyCodeProvider.Create());
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public Task<IResultModel> Login(LoginDto dto)
        {
            dto.IP = _ipResolver.IP;
            dto.IPv4 = _ipResolver.IPv4;
            dto.IPv6 = _ipResolver.IPv6;
            dto.UserAgent = _ipResolver.UserAgent;
            dto.LoginTime = DateTime.Now.ToTimestamp();

            return _service.Login(dto);
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public Task<IResultModel> RefreshToken(RefreshTokenDto dto)
        {
            dto.IP = _ipResolver.IP;
            return _service.RefreshToken(dto);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowWhenAuthenticated]
        public Task<IResultModel> Profile()
        {
            return _service.GetProfile(_account.Id, _account.Platform);
        }
    }
}
