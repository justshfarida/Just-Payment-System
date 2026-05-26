using Application.Common.Models;

namespace Application.Common;

public interface IPagedResponseFactory
{
    PagedResponse<T> Create<T>(List<T> items, int page, int pageSize, int totalCount);
    Task<PagedResponse<T>> Create<T>(IQueryable<T> query, int page, int pageSize);
}
