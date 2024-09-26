namespace ScoBro.Foundation.Results;


public record FailureType : EnumBase<FailureType> {
    protected FailureType() { }

    private FailureType(string id) : base(id) { }

    public static FailureType UserError => new("UserError");
    public static FailureType SystemError => new("SystemError");
    public static FailureType NotFound => new("NotFound");
}
