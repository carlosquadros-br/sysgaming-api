using System;
using SysgamingApi.Src.Domain.Entities;

namespace SysgamingApi.Src.Domain.Persitence;

public interface IAccountBalanceRepository
{
    Task<AccountBalance> CreateAsync(AccountBalance entity);
    Task<AccountBalance> GetByIdAsync(string id);

    Task<AccountBalance> GetByUserIdAsync(string userId);

    Task<AccountBalance> UpdateAsync(AccountBalance entity);

    Task<AccountBalance> UpdateBalanceAsync(string userId, decimal amount);

}
