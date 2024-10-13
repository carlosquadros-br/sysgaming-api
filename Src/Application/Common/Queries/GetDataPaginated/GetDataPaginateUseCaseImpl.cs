using System;
using SysgamingApi.Src.Domain.Common;
using SysgamingApi.Src.Domain.Persitence.Repositories;

namespace SysgamingApi.Src.Application.Common.Queries.GetListPaginated;

public class GetDataPaginateUseCaseImpl<T> : IGetDataPaginateUseCase<T> where T : class
{
    private readonly IBaseRepository<T> _repository;
    public GetDataPaginateUseCaseImpl(IBaseRepository<T> repository)
    {
        _repository = repository;
    }
    public async Task<PaginationList<T>> Handle(int page, int pageSize)
    {
        return await _repository.GetPaginatedAsync(page, pageSize);
    }
}
