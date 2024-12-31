namespace ScoBro.Foundation {
    public interface ISortParameters {
        string SortField { get; set; }
        string SortOrder { get; set; }
    }

    public record class SortParameters : ISortParameters {
        public SortParameters() {
            SortField = string.Empty;
            SortOrder = string.Empty;
        }

        public SortParameters(string sortField, string sortOrder) {
            SortField = sortField;
            SortOrder = sortOrder;
        }

        public string SortField { get; set; }
        public string SortOrder { get; set; }

        public static SortParameters Ascending(string sortField) => new(sortField, "Ascending");
        public static SortParameters Descending(string sortField) => new(sortField, "Descending");
    }
}
