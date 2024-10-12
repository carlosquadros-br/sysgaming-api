using System;

namespace SysgamingApi.Src.Domain.Common;

public class PaginationList<T>
{
    public IReadOnlyCollection<T>? Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginationList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }
}
