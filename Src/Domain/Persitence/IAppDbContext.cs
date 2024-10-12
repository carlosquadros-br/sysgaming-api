using System;
using Microsoft.EntityFrameworkCore;
namespace SysgamingApi.Src.Domain.Persitence;

public interface IAppDbContext
{

    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
