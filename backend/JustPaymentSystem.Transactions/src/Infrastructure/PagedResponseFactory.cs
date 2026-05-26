using Application.Common;
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PagedResponseFactory : IPagedResponseFactory
{

    public PagedResponse<T> Create<T>(List<T> items, int page, int pageSize, int totalCount)
    {
        if (!IsPageValid(page, pageSize))
        {
            throw new Exception("Page and Page size are not valid");
        }
        return new PagedResponse<T>(items, page, pageSize, totalCount);
    }

    public async Task<PagedResponse<T>> Create<T>(IQueryable<T> query, int page, int pageSize)
    {
        if (!IsPageValid(page, pageSize))
        {
            throw new Exception("Page and Page size are not valid");
        }
        List<T> col = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        int totalCount = await query.CountAsync();
        return new PagedResponse<T>(col, page, pageSize, totalCount);
    }

    public bool IsPageValid(int page, int pageSize)
    {
        if (page <= 0 || pageSize <= 0) return false;
        if (pageSize >= 50) return false;
        return true;
    }
}
