using System;
using System.ComponentModel.DataAnnotations;

using Wyn.Admin.Core.Domain.Account;
using Wyn.Utils.Annotations;

namespace Wyn.Admin.Core.Application.Account.Dto
{
    [ObjectMap(typeof(AccountEntity), true)]
    public class AccountUpdateDto : AccountAddDto
    {
        [Required(ErrorMessage = "请选择账户")]
        [GuidNotEmpty(ErrorMessage = "请选择账户")]
        public Guid Id { get; set; }
    }
}
