using System.Threading.Tasks;
using Wyn.Admin.Core.Application.Authorize.Vo;
using Wyn.Admin.Core.Domain.Account;

namespace Wyn.Admin.Core.Infrastructure
{
    /// <summary>
    /// 账户资料解析器
    /// </summary>
    public interface IAccountProfileResolver
    {
        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="account">账户信息</param>
        /// <param name="platform">登录平台</param>
        /// <returns></returns>
        Task<ProfileVo> Resolve(AccountEntity account, int platform);
    }
}
