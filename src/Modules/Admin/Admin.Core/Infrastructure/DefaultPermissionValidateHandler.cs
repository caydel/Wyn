using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wyn.Auth.Abstractions;
using Wyn.Auth.Abstractions.Descriptor;
using Wyn.Utils.Extensions;

namespace Wyn.Mod.Admin.Core.Infrastructure
{
    /// <summary>
    /// 默认权限验证处理器
    /// </summary>
    public class DefaultPermissionValidateHandler : IPermissionValidateHandler
    {
        private readonly IAccountPermissionResolver _accountPermissionResolver;
        private readonly IAccount _account;

        public DefaultPermissionValidateHandler(IAccountPermissionResolver accountPermissionResolver, IAccount account)
        {
            _accountPermissionResolver = accountPermissionResolver;
            _account = account;
        }

        public async Task<bool> Validate(IDictionary<string, object> routeValues, HttpMethod httpMethod)
        {
            var permissions = await _accountPermissionResolver.Resolve(_account.Id, _account.Platform);

            var area = routeValues["area"];
            var controller = routeValues["controller"];
            var action = routeValues["action"];
            return permissions.Any(m => m.EqualsIgnoreCase($"{area}_{controller}_{action}_{httpMethod}"));
        }
    }
}
