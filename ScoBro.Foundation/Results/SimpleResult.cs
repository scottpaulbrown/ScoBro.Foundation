
namespace ScoBro.Foundation;
public record class SimpleResult {
    public SimpleResult(bool wasSuccessful, IEnumerable<string>? errors = null) {
        WasSuccessful = wasSuccessful;
        Errors = errors ?? [];
    }

    public bool WasSuccessful { get; }
    public IEnumerable<string> Errors { get; }

    public static SimpleResult<T> Ok<T>(T value) => new(value, true);
    public static SimpleResult<T> Fail<T>() => new(default, false);

    public static SimpleResult Ok() => new(true);
    public static SimpleResult Fail(IEnumerable<string>? errors = null) => new(false, errors);
}

public record class SimpleResult<T> : SimpleResult {
    public SimpleResult(T? value, bool wasSuccessful, IEnumerable<string>? errors = null) : base(wasSuccessful, errors) {
        this.Value = value;
    }

    public T? Value { get; }
}
