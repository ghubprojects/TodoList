namespace TodoList.Common.Wrappers;

public class PagedList<T>(List<T> items, int page, int pageSize, int totalCount) {
    public List<T> Items { get; } = items;
    public int Page { get; } = page;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page * PageSize < TotalCount;
}
