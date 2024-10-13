using System;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
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
}
