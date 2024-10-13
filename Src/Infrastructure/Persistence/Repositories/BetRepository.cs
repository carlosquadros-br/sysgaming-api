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

    public async Task<Bet> CreateAsync(Bet entity)
    {
        entity.Id = Guid.NewGuid().ToString();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        await _appDbContext.Set<Bet>().AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }
}
