using System;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Common.Queries.GetListPaginated;

public interface IGetDataPaginateUseCase<T> where T : class
{
    Task<PaginationList<T>> Handle(int page, int pageSize);

}
