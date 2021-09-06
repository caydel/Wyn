using System;

using Microsoft.AspNetCore.Http;

using Wyn.Auth.Abstractions;
using Wyn.Data.Abstractions;
using Wyn.Utils.Extensions;

namespace Wyn.Auth.Core
{
    internal class AccountResolver : IAccountResolver
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountResolver(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

        /// <summary>
        /// 租户编号
        /// </summary>
        public Guid? TenantId
        {
            get
            {
                var tenantId = _contextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.TENANT_ID);

                return tenantId != null && tenantId.Value.NotNull() ? (Guid?)new Guid(tenantId.Value) : null;
            }
        }

        /// <summary>
        /// 账户编号
        /// </summary>
        public Guid? AccountId
        {
            get
            {
                var accountId = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.ACCOUNT_ID);

                return accountId != null && accountId.Value.NotNull() ? (Guid?)new Guid(accountId.Value) : (Guid?)Guid.Empty;
            }
        }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string AccountName
        {
            get
            {
                var accountName = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.ACCOUNT_NAME);

                return accountName == null || accountName.Value.IsNull() ? "" : accountName.Value;
            }
        }

    }
}
