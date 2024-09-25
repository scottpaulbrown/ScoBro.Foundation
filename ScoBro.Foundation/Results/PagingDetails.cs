
namespace ScoBro.Foundation;

public interface IPagingDetails : IPagingRestrictions {
    int TotalPages { get; init; }   
}

public interface IPagingRestrictions {
    int CurrentPage { get; init; }    
    int PageSize { get; init; }

    int GetOffset();
}

public record class PagingRestrictions : IPagingRestrictions {
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }

    public PagingRestrictions(int currentPage, int pageSize) {
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

    protected PagingRestrictions(IPagingRestrictions restrictions) {
        CurrentPage = restrictions.CurrentPage;
        PageSize = restrictions.PageSize;
    }

    public int GetOffset() => CurrentPage > 1 ? (CurrentPage - 1) * PageSize : 0;
}

public record class PagingDetails : PagingRestrictions, IPagingDetails {
    public int TotalPages { get; init; }

    public PagingDetails(int totalPages, IPagingRestrictions restrictions) : base(restrictions) {
        TotalPages = totalPages;
    }

    public PagingDetails(int totalPages, int currentPage, int pageSize) : base(currentPage, pageSize) {
        TotalPages = totalPages;
    }
}

