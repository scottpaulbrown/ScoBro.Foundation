namespace ScoBro.Foundation.Results;

/// <summary>
/// Represents the category of failure that caused a <see cref="SimpleResult"/> to be unsuccessful.
/// Uses the smart-enum pattern via <see cref="EnumBase{T}"/> to allow exhaustive comparisons and
/// serialisation without raw string coupling.
/// </summary>
public record FailureType : EnumBase<FailureType>
{
    /// <summary>Initialises a new <see cref="FailureType"/> for derived or default construction.</summary>
    protected FailureType() { }

    private FailureType(string id) : base(id) { }

    /// <summary>The operation failed because the caller supplied invalid or inappropriate input.</summary>
    public static FailureType UserError => new("UserError");

    /// <summary>The operation failed because one or more field-level validation rules were violated.</summary>
    public static FailureType ValidationError => new("ValidationError");

    /// <summary>The operation failed due to an unexpected internal error (e.g. an unhandled exception).</summary>
    public static FailureType SystemError => new("SystemError");

    /// <summary>The requested resource could not be found.</summary>
    public static FailureType NotFound => new("NotFound");

    /// <summary>The caller is authenticated but does not have permission to perform this operation.</summary>
    public static FailureType Forbidden => new("Forbidden");

    /// <summary>The caller is not authenticated and must supply valid credentials.</summary>
    public static FailureType Unauthorized => new("Unauthorized");
}
