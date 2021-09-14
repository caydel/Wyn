using System;
using System.ComponentModel.DataAnnotations;

using Wyn.Mod.Admin.Core.Domain.Account;
using Wyn.Utils.Annotations;

namespace Wyn.Mod.Admin.Core.Application.Account.Dto
{
    [ObjectMap(typeof(AccountEntity), true)]
    public class AccountUpdateDto : AccountAddDto
    {
        [Required(ErrorMessage = "请选择账户")]
        [GuidNotEmpty(ErrorMessage = "请选择账户")]
        public Guid Id { get; set; }
    }
}
