
using ScoBro.Foundation.Results;

namespace ScoBro.Foundation;
public record class SimpleResult {
    public SimpleResult(bool wasSuccessful, IEnumerable<string>? errors = null, FailureType? failureType = null) {
        WasSuccessful = wasSuccessful;
        Errors = errors ?? [];
        FailureType = failureType;
    }

    public bool WasSuccessful { get; }
    public IEnumerable<string> Errors { get; }
    public FailureType? FailureType { get; }
        
    public static SimpleResult<T> Ok<T>(T value) => new(value, true);
    public static SimpleResult<T> Fail<T>(string message) => new(default, false, [ message ]);
    public static SimpleResult<T> Fail<T>(IEnumerable<string> errors) => new(default, false, errors);
    public static SimpleResult<T> FailNotFound<T>(string message = "The requested resource was not found") =>new(default, false, [ message ], FailureType.NotFound);
    public static SimpleResult<T> FailWIthUserError<T>(string message) => new(default, false, [message], FailureType.UserError);
    public static SimpleResult<T> FailWIthSystemError<T>(string message) => new(default, false, [message], FailureType.SystemError);

    public static SimpleResult Ok() => new(true);
    public static SimpleResult Fail(string message) => new(false, [message]);
    public static SimpleResult Fail(IEnumerable<string>? errors = null) => new(false, errors);
    public static SimpleResult FailWIthUserError(string message) => new(false, [message], FailureType.UserError);
    public static SimpleResult FailWIthSystemError(string message) => new(false, [message], FailureType.SystemError);
}

public record class SimpleResult<T> : SimpleResult {
    public SimpleResult(T? value, bool wasSuccessful, IEnumerable<string>? errors = null, FailureType? failureType = null) : base(wasSuccessful, errors, failureType) {
        this.Value = value;
    }

    public T? Value { get; }
}
