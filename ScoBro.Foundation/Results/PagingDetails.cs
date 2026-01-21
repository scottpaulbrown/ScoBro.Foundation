
namespace ScoBro.Foundation;

public enum PagingSortOrder {
    Asc,
    Desc
}

public interface IPagingDetails : IPagingRestrictions {
    int TotalItems { get; init; }   
}

public interface IPagingRestrictions: ISortParameters {
    int CurrentPage { get; init; }    
    int PageSize { get; init; }

    int GetOffset();
}

public record class PagingRestrictions : IPagingRestrictions {
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public string SortField { get; set; }
    public string SortOrder { get; set; }

    public PagingSortOrder GetSortOrder() => SortOrder == "Asc" ? PagingSortOrder.Asc : PagingSortOrder.Desc;

    public PagingRestrictions() {
        SortField = string.Empty;
        SortOrder = string.Empty;
    }

    public PagingRestrictions(int currentPage, int pageSize, string sortField, string sortOrder) {
        CurrentPage = currentPage;
        PageSize = pageSize;
        SortField = sortField;
        SortOrder = sortOrder;
    }

     public PagingRestrictions(int currentPage, int pageSize, string sortField, PagingSortOrder sortOrder = PagingSortOrder.Asc) {
        CurrentPage = currentPage;
        PageSize = pageSize;
        SortField = sortField;
        SortOrder = sortOrder.ToString();
    }

    protected PagingRestrictions(IPagingRestrictions restrictions) {
        CurrentPage = restrictions.CurrentPage;
        PageSize = restrictions.PageSize;
        SortField = restrictions.SortField;
        SortOrder = restrictions.SortOrder;
    }

    public int GetOffset() => CurrentPage > 1 ? (CurrentPage - 1) * PageSize : 0;

    public PagingRestrictions CopyWithDefaults(int? currentPage = null, int? pageSize = null, string? sortField = null, string? sortOrder = null) {
        return new PagingRestrictions(
            currentPage: currentPage != null && CurrentPage == 0 ? currentPage.Value : CurrentPage,
            pageSize: pageSize != null && PageSize == 0 ? pageSize.Value : PageSize,
            sortField: sortField != null && string.IsNullOrEmpty(SortField) ? sortField : SortField,
            sortOrder: sortOrder != null && string.IsNullOrEmpty(SortOrder) ? sortOrder : SortOrder
        );
    }
}

public record class PagingDetails : PagingRestrictions, IPagingDetails {
    public int TotalItems { get; init; }
    
    public PagingDetails() { }

    public PagingDetails(int totalItems, IPagingRestrictions restrictions) : base(restrictions) {
        TotalItems = totalItems;
    }

    public PagingDetails(int totalItems, int currentPage, int pageSize, string sortField, string sortOrder) : base(currentPage, pageSize, sortField, sortOrder) {
        TotalItems = totalItems;
    }
}

