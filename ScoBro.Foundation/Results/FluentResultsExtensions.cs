using FluentResults;

namespace ScoBro.Foundation;
public static class FluentResultsExtensions {
    public static SimpleResult<T> ToSimpleResult<T>(this Result<T> result) =>
        new(
            value: result.Value,
            wasSuccessful: result.IsSuccess,
            errors: result.Errors.Select(e => e.Message)
        );

    public static SimpleResult ToSimpleResult(this Result result) =>
        new(
            wasSuccessful: result.IsSuccess,
            errors: result.Errors.Select(e => e.Message)
        );
}

