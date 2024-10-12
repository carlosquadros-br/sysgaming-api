using System;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories;

public class BetRepository : IBetRepository
{
    readonly DbSet<Bet> _betSet;

    public BetRepository(IAppDbContext appDbContext)
    {
        _betSet = appDbContext.Set<Bet>();
    }

    public async Task<PaginationList<Bet>> GetPaginatedAsync(int page, int pageSize)
    {
        var result = await _betSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        if (result.Count == 0)
        {
            return new PaginationList<Bet>(new List<Bet>(), 0, page, pageSize);
        }
        return new PaginationList<Bet>(result, result.Count, page, pageSize);
    }
}
