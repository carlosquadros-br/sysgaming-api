using System;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Enums;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories;

public class BetRepository : AbstractRepository<Bet>, IBetRepository
{

    public BetRepository(IAppDbContext appDbContext) : base(appDbContext)
    { }
    public async Task<Bet> UpdateAsync(Bet entity)
    {
        var updated = _appDbContext.Set<Bet>().Update(entity);
        await _appDbContext.SaveChangesAsync();
        return updated.Entity;
    }

    public async Task<bool> IsLastFiveBetsIsLose(string userId)
    {
        var query = _appDbContext.Set<Bet>().Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).Take(5);
        var lastFive = query.ToList();
        if (lastFive.Count < 5)
        {
            return false;
        }
        return lastFive.Count >= 5 && lastFive.All(x => x.Result == BetResult.Lose);
    }

    public async Task<decimal> GetAmountLastFiveBets(string userId)
    {
        var query = _appDbContext.Set<Bet>().Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).Take(5);
        var lastFive = query.ToList();
        if (lastFive.Count < 5)
        {
            return 0;
        }
        return lastFive.Sum(x => x.Amount);
    }


}
