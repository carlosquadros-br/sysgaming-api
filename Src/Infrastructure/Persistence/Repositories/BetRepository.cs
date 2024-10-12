using System;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories;

public class BetRepository : IBetRepository
{
    private readonly IAppDbContext _appDbContext;

    public BetRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Bet> CreateAsync(Bet entity)
    {
        entity.Id = Guid.NewGuid().ToString();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        await _appDbContext.Bets.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<PaginationList<Bet>> GetPaginatedAsync(int page, int pageSize)
    {
        var result = await _appDbContext.Bets.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        if (result.Count == 0)
        {
            return new PaginationList<Bet>(new List<Bet>(), 0, page, pageSize);
        }
        return new PaginationList<Bet>(result, result.Count, page, pageSize);
    }
}
