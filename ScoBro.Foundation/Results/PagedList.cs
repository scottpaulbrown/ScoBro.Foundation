namespace ScoBro.Foundation;

public record class PagedList<T> : PagingDetails {
    public PagedList(List<T> items, PagingDetails pagingDetails) : base(pagingDetails) {
        Items = items;
    }

    public PagedList() { }

    public PagedList(List<T> items, int totalPages, int currentPage, int pageSize) : base(totalPages, currentPage, pageSize) {
        Items = items;
    }

    public List<T> Items { get; protected set; } = [];

    public static PagedList<TEntity> Empty<TEntity>() => new([], new PagingDetails(0, 1, 30));
}