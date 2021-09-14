using Wyn.Data.Abstractions;

namespace Wyn.Mod.Admin.Core.Domain.JwtAuthInfo
{
    /// <summary>
    /// JWT认证信息仓储
    /// </summary>
    public interface IJwtAuthInfoRepository : IRepository<JwtAuthInfoEntity>
    {
    }
}
