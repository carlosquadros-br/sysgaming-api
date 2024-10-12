using System;
using SysgamingApi.Src.Domain.Common;

namespace SysgamingApi.Src.Domain.Persitence.Repositories;

public interface IBaseRepository<T> where T : class
{

    Task<T> CreateAsync(T entity);
    Task<PaginationList<T>> GetPaginatedAsync(int page, int pageSize);


}
