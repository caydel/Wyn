using System;

using Microsoft.AspNetCore.Http;

using Wyn.Auth.Abstractions;
using Wyn.Utils.Extensions;

namespace Wyn.Auth.Core
{
    /// <summary>
    /// 账户信息
    /// </summary>
    internal class Account : IAccount
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public Account(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

        /// <summary>
        /// 租户编号
        /// </summary>
        public Guid? TenantId
        {
            get
            {
                var tenantId = _contextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.TENANT_ID);

                return tenantId != null && tenantId.Value.NotNull() ? new Guid(tenantId.Value) : null;
            }
        }

        /// <summary>
        /// 账户编号
        /// </summary>
        public Guid Id
        {
            get
            {
                var     id = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.ACCOUNT_ID);

                return id != null && id.Value.NotNull() ? new Guid(id.Value) : Guid.Empty;
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

        /// <summary>
        /// 请求平台
        /// </summary>
        public int Platform
        {
            get
            {
                var pt = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.PLATFORM);

                return pt != null && pt.Value.NotNull() ? pt.Value.ToInt() : 0;
            }
        }

        /// <summary>
        /// 获取当前用户IP(包含IPv和IPv6)
        /// </summary>
        public string IP => _contextAccessor?.HttpContext?.Connection.RemoteIpAddress == null ? "" : _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        /// <summary>
        /// 获取当前用户IPv4
        /// </summary>
        public string IPv4 => _contextAccessor?.HttpContext?.Connection.RemoteIpAddress == null ? "" : _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

        /// <summary>
        /// 获取当前用户IPv6
        /// </summary>
        public string IPv6 => _contextAccessor?.HttpContext?.Connection.RemoteIpAddress == null ? "" : _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();

        /// <summary>
        /// 登录时间
        /// </summary>
        public long LoginTime
        {
            get
            {
                var ty = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.LOGIN_TIME);

                return ty != null && ty.Value.NotNull() ? ty.Value.ToLong() : 0L;
            }
        }

        /// <summary>
        /// User-Agent
        /// </summary>
        public string UserAgent => _contextAccessor?.HttpContext?.Request == null ? "" : _contextAccessor.HttpContext.Request.Headers["User-Agent"];
    }
}
