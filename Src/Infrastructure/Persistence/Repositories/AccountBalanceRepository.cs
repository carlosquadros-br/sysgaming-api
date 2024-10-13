using System;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories;

public class AccountBalanceRepository : AbstractRepository<AccountBalance>, IAccountBalanceRepository
{
    public AccountBalanceRepository(IAppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<AccountBalance> CreateAsync(AccountBalance entity)
    {
        entity.Id = Guid.NewGuid().ToString();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        await _appDbContext.Set<AccountBalance>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<AccountBalance> GetByUserIdAsync(string userId)
    {
        var result = await _dbSet.FirstOrDefaultAsync(entity => entity.UserId == userId);
        if (result == null)
        {
            return null;
        }
        return result;
    }

    public Task<AccountBalance> UpdateAsync(AccountBalance entity)
    {
        throw new NotImplementedException();
    }

    public async Task<AccountBalance> UpdateBalanceAsync(string userId, decimal amount)
    {
        var updated = await _dbSet.FirstOrDefaultAsync(entity => entity.UserId == userId);

        if (updated == null)
            throw new Exception("Account not found");

        updated.Balance += amount;

        await _appDbContext.SaveChangesAsync();
        return updated;
    }
}

