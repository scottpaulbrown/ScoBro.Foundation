
namespace ScoBro.Foundation;

public interface IPagingDetails : IPagingRestrictions {
    int TotalPages { get; init; }   
}

public interface IPagingRestrictions {
    int CurrentPage { get; init; }
    int Offset { get; }
    int PageSize { get; init; }
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

    public int Offset => CurrentPage > 1 ? (CurrentPage - 1) * PageSize : 0;
}

public record class PagingDetails(int TotalPages, IPagingRestrictions Restrictions) : PagingRestrictions(Restrictions), IPagingDetails;

