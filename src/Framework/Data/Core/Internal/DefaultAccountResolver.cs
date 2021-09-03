using System;
using Wyn.Data.Abstractions;

namespace Wyn.Data.Core.Internal
{
    /// <summary>
    /// 默认账户解析器
    /// </summary>
    internal class DefaultAccountResolver : IAccountResolver
    {
        public Guid? TenantId => null;

        public Guid? AccountId => null;

        public string AccountName => string.Empty;
    }
}
