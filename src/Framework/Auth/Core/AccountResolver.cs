﻿using System;

using Microsoft.AspNetCore.Http;

using Wyn.Auth.Abstractions;
using Wyn.Data.Abstractions;
using Wyn.Utils.Extensions;

namespace Wyn.Auth.Core
{
    internal class AccountResolver : IAccountResolver
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountResolver(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid? TenantId
        {
            get
            {
                var tenantId = _contextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.TENANT_ID);

                if (tenantId != null && tenantId.Value.NotNull())
                {
                    return new Guid(tenantId.Value);
                }

                return null;
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

                if (accountId != null && accountId.Value.NotNull())
                {
                    return new Guid(accountId.Value);
                }

                return Guid.Empty;
            }
        }

        public string AccountName
        {
            get
            {
                var accountName = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.ACCOUNT_NAME);

                if (accountName == null || accountName.Value.IsNull())
                {
                    return "";
                }

                return accountName.Value;
            }
        }

    }
}