﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Wyn.Cache.Abstractions;
using Wyn.Admin.Core.Domain.Account;
using Wyn.Admin.Core.Domain.RolePermission;
using Wyn.Utils.Extensions;

namespace Wyn.Admin.Core.Infrastructure
{
    /// <summary>
    /// 默认账户权限解析器
    /// </summary>
    public class DefaultAccountPermissionResolver : IAccountPermissionResolver
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly AdminCacheKeys _cacheKeys;
        private readonly ICacheHandler _cacheHandler;

        public DefaultAccountPermissionResolver(IAccountRepository accountRepository, IRolePermissionRepository rolePermissionRepository, AdminCacheKeys cacheKeys, ICacheHandler cacheHandler)
        {
            _accountRepository = accountRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _cacheKeys = cacheKeys;
            _cacheHandler = cacheHandler;
        }

        public async Task<IList<string>> Resolve(Guid accountId, int platform)
        {
            if (accountId.IsEmpty())
                return new List<string>();

            var key = _cacheKeys.AccountPermissions(accountId, platform);
            var list = await _cacheHandler.Get<IList<string>>(key);
            if (list == null)
            {
                var account = await _accountRepository.Get(accountId);
                if (account == null)
                    return new List<string>();

                list = await _rolePermissionRepository
                    .Find(m => m.RoleId == account.RoleId)
                    .Select(m => m.PermissionCode)
                    .ToList<string>();

                await _cacheHandler.Set(key, list);
            }

            return list;
        }
    }
}
