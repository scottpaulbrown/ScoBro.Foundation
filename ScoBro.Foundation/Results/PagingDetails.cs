
namespace ScoBro.Foundation;

/// <summary>
/// Specifies the direction of a sort operation.
/// </summary>
public enum PagingSortOrder {
    /// <summary>Sort results in ascending order (A → Z, lowest → highest).</summary>
    Asc,
    /// <summary>Sort results in descending order (Z → A, highest → lowest).</summary>
    Desc
}

/// <summary>
/// Extends <see cref="IPagingRestrictions"/> with the total number of items across all pages,
/// enabling calculation of page counts and navigation bounds.
/// </summary>
public interface IPagingDetails : IPagingRestrictions {
    /// <summary>Gets the total number of items across all pages.</summary>
    int TotalItems { get; init; }
}

/// <summary>
/// Defines the paging and sorting parameters used to request a specific page of results.
/// </summary>
public interface IPagingRestrictions: ISortParameters {
    /// <summary>Gets the 1-based current page number being requested.</summary>
    int CurrentPage { get; init; }

    /// <summary>Gets the maximum number of items to return per page.</summary>
    int PageSize { get; init; }

    /// <summary>
    /// Calculates the zero-based row offset for use in data queries (e.g. SQL OFFSET).
    /// </summary>
    /// <returns>The number of rows to skip before the current page begins.</returns>
    int GetOffset();
}

/// <summary>
/// Concrete implementation of <see cref="IPagingRestrictions"/> carrying the paging and sorting
/// parameters needed to retrieve a specific page of data.
/// </summary>
public record class PagingRestrictions : IPagingRestrictions {
    /// <inheritdoc/>
    public int CurrentPage { get; init; }

    /// <inheritdoc/>
    public int PageSize { get; init; }

    /// <inheritdoc/>
    public string SortField { get; set; }

    /// <inheritdoc/>
    public string SortOrder { get; set; }

    /// <summary>
    /// Returns the sort direction as a <see cref="PagingSortOrder"/> enum value.
    /// Any value other than "Asc" is treated as <see cref="PagingSortOrder.Desc"/>.
    /// </summary>
    public PagingSortOrder GetSortOrder() => SortOrder == "Asc" ? PagingSortOrder.Asc : PagingSortOrder.Desc;

    /// <summary>
    /// Initialises a new <see cref="PagingRestrictions"/> with empty sort fields and default page values.
    /// </summary>
    public PagingRestrictions() {
        SortField = string.Empty;
        SortOrder = string.Empty;
    }

    /// <summary>
    /// Initialises a new <see cref="PagingRestrictions"/> with explicit paging and sort parameters.
    /// </summary>
    /// <param name="currentPage">The 1-based page number.</param>
    /// <param name="pageSize">The maximum items per page.</param>
    /// <param name="sortField">The field name to sort by.</param>
    /// <param name="sortOrder">The sort direction as a string (e.g. "Asc" or "Desc").</param>
    public PagingRestrictions(int currentPage, int pageSize, string sortField, string sortOrder) {
        CurrentPage = currentPage;
        PageSize = pageSize;
        SortField = sortField;
        SortOrder = sortOrder;
    }

    /// <summary>
    /// Initialises a new <see cref="PagingRestrictions"/> with an enum sort order.
    /// </summary>
    /// <param name="currentPage">The 1-based page number.</param>
    /// <param name="pageSize">The maximum items per page.</param>
    /// <param name="sortField">The field name to sort by.</param>
    /// <param name="sortOrder">The sort direction; defaults to <see cref="PagingSortOrder.Asc"/>.</param>
    public PagingRestrictions(int currentPage, int pageSize, string sortField, PagingSortOrder sortOrder = PagingSortOrder.Asc) {
        CurrentPage = currentPage;
        PageSize = pageSize;
        SortField = sortField;
        SortOrder = sortOrder.ToString();
    }

    /// <summary>
    /// Initialises a new <see cref="PagingRestrictions"/> by copying values from an existing
    /// <see cref="IPagingRestrictions"/> instance.
    /// </summary>
    /// <param name="restrictions">The source restrictions to copy from.</param>
    protected PagingRestrictions(IPagingRestrictions restrictions) {
        CurrentPage = restrictions.CurrentPage;
        PageSize = restrictions.PageSize;
        SortField = restrictions.SortField;
        SortOrder = restrictions.SortOrder;
    }

    /// <inheritdoc/>
    public int GetOffset() => CurrentPage > 1 ? (CurrentPage - 1) * PageSize : 0;

    /// <summary>
    /// Returns a new <see cref="PagingRestrictions"/> that fills in zero or empty fields with
    /// the supplied defaults, leaving already-populated fields unchanged.
    /// </summary>
    /// <param name="currentPage">Default page number, applied only if <see cref="CurrentPage"/> is 0.</param>
    /// <param name="pageSize">Default page size, applied only if <see cref="PageSize"/> is 0.</param>
    /// <param name="sortField">Default sort field, applied only if <see cref="SortField"/> is empty.</param>
    /// <param name="sortOrder">Default sort order, applied only if <see cref="SortOrder"/> is empty.</param>
    /// <returns>A new instance with any missing values filled from the provided defaults.</returns>
    public PagingRestrictions CopyWithDefaults(int? currentPage = null, int? pageSize = null, string? sortField = null, string? sortOrder = null) {
        return new PagingRestrictions(
            currentPage: currentPage != null && CurrentPage == 0 ? currentPage.Value : CurrentPage,
            pageSize: pageSize != null && PageSize == 0 ? pageSize.Value : PageSize,
            sortField: sortField != null && string.IsNullOrEmpty(SortField) ? sortField : SortField,
            sortOrder: sortOrder != null && string.IsNullOrEmpty(SortOrder) ? sortOrder : SortOrder
        );
    }
}

/// <summary>
/// Extends <see cref="PagingRestrictions"/> with the total item count, allowing consumers to
/// compute total page counts and determine whether more pages are available.
/// </summary>
public record class PagingDetails : PagingRestrictions, IPagingDetails {
    /// <inheritdoc/>
    public int TotalItems { get; init; }

    /// <summary>
    /// Initialises a new <see cref="PagingDetails"/> with default paging values and zero total items.
    /// </summary>
    public PagingDetails() { }

    /// <summary>
    /// Initialises a new <see cref="PagingDetails"/> by combining a total item count with an
    /// existing set of paging restrictions.
    /// </summary>
    /// <param name="totalItems">The total number of items across all pages.</param>
    /// <param name="restrictions">The paging and sorting parameters for the current page.</param>
    public PagingDetails(int totalItems, IPagingRestrictions restrictions) : base(restrictions) {
        TotalItems = totalItems;
    }

    /// <summary>
    /// Initialises a new <see cref="PagingDetails"/> with explicit paging, sorting, and total item values.
    /// </summary>
    /// <param name="totalItems">The total number of items across all pages.</param>
    /// <param name="currentPage">The 1-based current page number.</param>
    /// <param name="pageSize">The maximum number of items per page.</param>
    /// <param name="sortField">The field name used for sorting.</param>
    /// <param name="sortOrder">The sort direction as a string (e.g. "Asc" or "Desc").</param>
    public PagingDetails(int totalItems, int currentPage, int pageSize, string sortField, string sortOrder) : base(currentPage, pageSize, sortField, sortOrder) {
        TotalItems = totalItems;
    }
}
