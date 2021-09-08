using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Wyn.Admin.Core.Infrastructure
{
    public class DefaultCredentialClaimExtender : ICredentialClaimExtender
    {
        public Task Extend(List<Claim> claims, Guid accountId)
        {
            return Task.CompletedTask;
        }
    }
}
