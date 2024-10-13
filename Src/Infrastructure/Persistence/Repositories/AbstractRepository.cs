using System;
using Microsoft.EntityFrameworkCore;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Entities;
using SysgamingApi.Src.Domain.Persitence;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Infrastructure.Persistence.Repositories;

public abstract class AbstractRepository<T> : IBaseRepository<T> where T : class, IBase
{
    protected readonly IAppDbContext _appDbContext;
    protected readonly DbSet<T> _dbSet;

    protected AbstractRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = _appDbContext.Set<T>();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await _appDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<PaginationList<T>> GetPaginatedAsync(int page, int pageSize)
    {

        var count = await _dbSet.CountAsync();

        var result = await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        if (result.Count == 0)
        {
            return new PaginationList<T>(new List<T>(), 0, page, pageSize);
        }
        return new PaginationList<T>(result, result.Count, page, pageSize);
    }
}
