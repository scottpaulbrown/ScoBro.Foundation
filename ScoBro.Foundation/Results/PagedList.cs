namespace ScoBro.Foundation;

public record class PagedList<T> : PagingDetails {
    public PagedList(List<T> items, PagingDetails pagingDetails) : base(pagingDetails) {
        Items = items;
    }

    public List<T> Items { get; }
}