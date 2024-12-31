
namespace ScoBro.Foundation;

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

    protected PagingRestrictions(IPagingRestrictions restrictions) {
        CurrentPage = restrictions.CurrentPage;
        PageSize = restrictions.PageSize;
        SortField = restrictions.SortField;
        SortOrder = restrictions.SortOrder;
    }

    public int GetOffset() => CurrentPage > 1 ? (CurrentPage - 1) * PageSize : 0;
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

