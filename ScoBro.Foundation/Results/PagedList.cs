namespace ScoBro.Foundation;

/// <summary>
/// A page of results of type <typeparamref name="T"/>, together with the paging metadata
/// (current page, page size, total items, sort field, and sort direction) inherited from
/// <see cref="PagingDetails"/>.
/// </summary>
/// <typeparam name="T">The type of the items in this page.</typeparam>
public record class PagedList<T> : PagingDetails {
    /// <summary>
    /// Initialises a new <see cref="PagedList{T}"/> with the specified items and paging details.
    /// </summary>
    /// <param name="items">The items on this page.</param>
    /// <param name="pagingDetails">The paging metadata for this page.</param>
    public PagedList(List<T> items, PagingDetails pagingDetails) : base(pagingDetails) {
        Items = items;
    }

    /// <summary>
    /// Initialises a new empty <see cref="PagedList{T}"/> with default paging values.
    /// </summary>
    public PagedList() { }

    /// <summary>
    /// Initialises a new <see cref="PagedList{T}"/> with the specified items and explicit paging parameters.
    /// </summary>
    /// <param name="items">The items on this page.</param>
    /// <param name="totalPages">The total number of items across all pages.</param>
    /// <param name="currentPage">The current (1-based) page number.</param>
    /// <param name="pageSize">The maximum number of items per page.</param>
    /// <param name="sortField">The field name used for sorting.</param>
    /// <param name="sortDirection">The sort direction (e.g. "Asc" or "Desc").</param>
    public PagedList(List<T> items, int totalPages, int currentPage, int pageSize, string sortField, string sortDirection)
        : base(totalPages, currentPage, pageSize, sortField, sortDirection) {
        Items = items;
    }

    /// <summary>
    /// Gets the items on this page.
    /// </summary>
    public List<T> Items { get; init; } = [];

    /// <summary>
    /// Creates an empty <see cref="PagedList{TEntity}"/> with zero total items, page 1, and a page size of 30.
    /// </summary>
    /// <typeparam name="TEntity">The item type of the returned list.</typeparam>
    /// <returns>An empty <see cref="PagedList{TEntity}"/>.</returns>
    public static PagedList<TEntity> Empty<TEntity>() => new([], new PagingDetails(0, 1, 30, "", ""));
}
