using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using Wyn.Auth.Abstractions.Options;
using Wyn.Mod.Admin.Core.Application.Account.Dto;
using Wyn.Mod.Admin.Core.Domain.Account;
using Wyn.Mod.Admin.Core.Domain.AccountSkin;
using Wyn.Mod.Admin.Core.Domain.Role;
using Wyn.Mod.Admin.Core.Infrastructure;
using Wyn.Mapper.Abstractions;
using Wyn.Utils.Result;
using Wyn.Utils.Extensions;

namespace Wyn.Mod.Admin.Core.Application.Account
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _repository;
        private readonly IRoleRepository _roleRepository;
        private readonly IOptionsMonitor<AuthOptions> _authOptions;
        private readonly IPasswordHandler _passwordHandler;
        private readonly IAccountSkinRepository _skinRepository;

        public AccountService(IMapper mapper, IAccountRepository repository, IOptionsMonitor<AuthOptions> authOptions, IPasswordHandler passwordHandler, IRoleRepository roleRepository, IAccountSkinRepository skinRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _authOptions = authOptions;
            _passwordHandler = passwordHandler;
            _roleRepository = roleRepository;
            _skinRepository = skinRepository;
        }

        public Task<IResultModel> Query(AccountQueryDto dto)
        {
            var query = _repository.Find()
                .WhereNotNull(dto.Username, m => m.Username == dto.Username)
                .WhereNotNull(dto.Name, m => m.Name.Contains(dto.Name))
                .WhereNotNull(dto.Phone, m => m.Phone.Contains(m.Phone))
                .LeftJoin<RoleEntity>(m => m.T1.RoleId == m.T2.Id)
                .Select(m => new { m.T1, RoleName = m.T2.Name });

            return query.ToPaginationResult(dto.Paging);
        }

        public async Task<IResultModel> Add(AccountAddDto dto)
        {
            if (await _repository.ExistsUsername(dto.Username))
                return ResultModel.Failed("用户名已存在");
            if (dto.Phone.NotNull() && await _repository.ExistsPhone(dto.Phone))
                return ResultModel.Failed("手机号已存在");
            if (dto.Email.NotNull() && await _repository.ExistsUsername(dto.Email))
                return ResultModel.Failed("邮箱已存在");
            if (!await _roleRepository.Exists(dto.RoleId))
                return ResultModel.Failed("绑定角色不存在");

            var account = _mapper.Map<AccountEntity>(dto);
            if (account.Password.IsNull())
            {
                account.Password = _authOptions.CurrentValue.DefaultPassword;
            }

            account.Password = _passwordHandler.Encrypt(account.Password);
            var result = await _repository.Add(account);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> Edit(Guid id)
        {
            var account = await _repository.Get(id);
            if (account == null)
                return ResultModel.NotExists;

            var model = _mapper.Map<AccountUpdateDto>(account);
            model.Password = "";

            return ResultModel.Success(model);
        }

        public async Task<IResultModel> Update(AccountUpdateDto dto)
        {
            var account = await _repository.Get(dto.Id);
            if (account == null)
                return ResultModel.NotExists;
            if (dto.Phone.NotNull() && await _repository.ExistsPhone(dto.Phone, dto.Id))
                return ResultModel.Failed("手机号已存在");
            if (dto.Email.NotNull() && await _repository.ExistsUsername(dto.Email, dto.Id))
                return ResultModel.Failed("邮箱已存在");
            if (!await _roleRepository.Exists(dto.RoleId))
                return ResultModel.Failed("绑定角色不存在");

            var username = account.Username;
            var password = account.Password;
            _mapper.Map(dto, account);

            //用户名和密码不允许修改
            account.Username = username;
            account.Password = password;

            var result = await _repository.Update(account);
            return ResultModel.Result(result);
        }

        public async Task<IResultModel> Delete(Guid id)
        {
            var account = await _repository.Get(id);
            if (account == null)
                return ResultModel.NotExists;

            var result = await _repository.SoftDelete(id);

            return ResultModel.Result(result);
        }

        public async Task<IResultModel> UpdateSkin(AccountSkinUpdateDto dto)
        {
            var config = await _skinRepository.Find(m => m.AccountId == dto.AccountId).ToFirst();

            if (config == null)
            {
                config = new AccountSkinEntity
                {
                    AccountId = dto.AccountId
                };
            }

            config.Code = dto.Code;
            config.Name = dto.Name;
            config.Theme = dto.Theme;
            config.Size = dto.Size;

            if (config.Id < 1)
            {
                return ResultModel.Result(await _skinRepository.Add(config));
            }

            return ResultModel.Result(await _skinRepository.Update(config));
        }
    }
}
