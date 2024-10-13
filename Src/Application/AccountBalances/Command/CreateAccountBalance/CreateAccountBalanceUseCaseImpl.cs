using System;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Application.AccountBalances.Command.CreateAccountBalance
{
    public class CreateAccountBalanceUseCaseImpl : ICreateAccountBalanceUseCase
    {
        private readonly IAccountBalanceRepository _accountBalanceRepository;

        public CreateAccountBalanceUseCaseImpl(IAccountBalanceRepository accountBalanceRepository)
        {
            _accountBalanceRepository = accountBalanceRepository;
        }

        public async Task<bool> Handle(string userId, decimal amount)
        {
            var accountBalance = await _accountBalanceRepository.GetByUserIdAsync(userId);
            if (accountBalance == null)
            {
                await _accountBalanceRepository.CreateAsync(new AccountBalance
                {
                    UserId = userId,
                    Balance = 0
                });
            }
            else
            {
                await _accountBalanceRepository.UpdateBalanceAsync(userId, amount);
            }
            return true;
        }
    }
}
