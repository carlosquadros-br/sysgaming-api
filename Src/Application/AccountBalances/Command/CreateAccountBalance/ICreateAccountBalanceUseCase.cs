using System;

namespace SysgamingApi.Src.Application.AccountBalances.Command.CreateAccountBalance;

public interface ICreateAccountBalanceUseCase
{

    public Task<bool> Handle(string userId, decimal amount);

}
