using System;
using SysgamingApi.Src.Application.Utils;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;

namespace SysgamingApi.Src.Application.AccountBalances.Queries;

public interface IGetAccountBalanceByUserId
{
    Task<AccountBalance> HandleAsync();
}

public class GetAccountBalanceByIdUserImpl : IGetAccountBalanceByUserId
{
    private readonly IAccountBalanceRepository _accountBalanceRepository;
    private readonly ILoggedInUserService _loggedInUser;

    public GetAccountBalanceByIdUserImpl(
        IAccountBalanceRepository accountBalanceRepository,
        ILoggedInUserService loggedInUserService
        )
    {
        _accountBalanceRepository = accountBalanceRepository;
        _loggedInUser = loggedInUserService;
    }

    public async Task<AccountBalance> HandleAsync()
    {
        var userId = _loggedInUser.UserId;
        return await _accountBalanceRepository.GetByUserIdAsync(userId);
    }
}
