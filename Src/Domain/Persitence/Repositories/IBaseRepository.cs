using System;
using SysgamingApi.Src.Domain.Common;

namespace SysgamingApi.Src.Domain.Persitence.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<PaginationList<T>> GetPaginatedAsync(int page, int pageSize);

}
