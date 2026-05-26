namespace Application.Common.Models;

public class PagedResponse<T>
{
    public PagedResponse(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => TotalCount > Page * PageSize;
    public bool HasPrevPage => Page > 1;
}
