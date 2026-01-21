using System.Runtime.CompilerServices;

namespace ScoBro.Foundation;

public static class ResultExtensions
{
    // Task<SimpleResult<T>> => Task<SimpleResult<U>>
    public static async Task<SimpleResult<U>> Then<T, U>(this Task<SimpleResult<T>> resultTask, Func<T, Task<SimpleResult<U>>> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result.Switch<U>();

        return await next(result.Value!);
    }

    public static async Task<SimpleResult<U>> Then<T, U>(this Task<SimpleResult<T>> resultTask, Func<T, SimpleResult<U>> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result.Switch<U>();

        return next(result.Value!);
    }

    public static async Task<SimpleResult<T>> OnError<T>(this Task<SimpleResult<T>> resultTask, Action<SimpleResult<T>> action)
    {
        var result = await resultTask;

        if (!result.WasSuccessful)
        {
            action(result);
        }

        return result;
    }

    public static async Task<SimpleResult> ThenTry<T>(this Task<SimpleResult<T>> resultTask, Func<T, Task> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result;

        try
        {
            await next(result.Value!);
            return SimpleResult.Ok();
        }
        catch (Exception ex)
        {
            return SimpleResult.Fail(ex.Message);
        }
    }

    public static async Task<SimpleResult<U>> ThenTry<T, U>(this Task<SimpleResult<T>> resultTask, Func<T, Task<SimpleResult<U>>> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result.Switch<U>();

        try
        {
            return await next(result.Value!);
        }
        catch (Exception ex)
        {
            return SimpleResult.Fail<U>(ex.Message);
        }
    }

    public static async Task<SimpleResult<U>> ThenTry<T, U>(this Task<SimpleResult<T>> resultTask, Func<T, Task<U>> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result.Switch<U>();

        try
        {
            var nextResult = await next(result.Value!);
            return SimpleResult.Ok(nextResult);
        }
        catch (Exception ex)
        {
            return SimpleResult.Fail<U>(ex.Message);
        }
    }

    public static async Task<SimpleResult<U>> ThenTry<T, U>(this Task<SimpleResult<T>> resultTask, Func<T, ValueTask<U>> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result.Switch<U>();

        try
        {
            var nextResult = await next(result.Value!);
            return SimpleResult.Ok(nextResult);
        }
        catch (Exception ex)
        {
            return SimpleResult.Fail<U>(ex.Message);
        }
    }

    public static async Task<SimpleResult<U>> ThenTry<T, U>(this Task<SimpleResult<T>> resultTask, Func<T, U> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result.Switch<U>();

        try
        {
            var nextResult = next(result.Value!);
            return SimpleResult.Ok(nextResult);
        }
        catch (Exception ex)
        {
            return SimpleResult.Fail<U>(ex.Message);
        }
    }

    public static SimpleResult<U> ThenTry<T, U>(this SimpleResult<T> result, Func<T, U> next)
    {
        if (!result.WasSuccessful)
            return result.Switch<U>();

        try
        {
            var nextResult = next(result.Value!);
            return SimpleResult.Ok(nextResult);
        }
        catch (Exception ex)
        {
            return SimpleResult.Fail<U>(ex.Message);
        }
    }

    public static SimpleResult ThenTry<T>(this SimpleResult<T> result, Action<T> next)
    {
        if (!result.WasSuccessful) return result;

        try
        {
            next(result.Value!);
            return SimpleResult.Ok();
        }
        catch (Exception ex)
        {
            return SimpleResult.Fail(ex.Message);
        }
    }

    // public static async Task<SimpleResult<T>> OnError<T>(this Task<SimpleResult<T>> resultTask, Action<SimpleResult<T>> action)
    // {
    //     var result = await resultTask;

    //     if (!result.WasSuccessful)
    //     {
    //         action(result);
    //     }

    //     return result;
    // }

    public static async Task<SimpleResult<T>> Tap<T>(this Task<SimpleResult<T>> resultTask, Func<T, Task> sideEffect)
    {
        var result = await resultTask;
        if (result.WasSuccessful)
        {
            await sideEffect(result.Value!);
        }

        return result;
    }

    public static async Task<SimpleResult<U>> Map<T, U>(this Task<SimpleResult<T>> resultTask, Func<T, U> mapper)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return SimpleResult.Fail<U>(result.Errors);

        var newValue = mapper(result.Value!);
        return SimpleResult.Ok(newValue);
    }

    // Task<SimpleResult> => Task<SimpleResult<U>>
    public static async Task<SimpleResult<U>> Then<U>(this Task<SimpleResult> resultTask, Func<Task<SimpleResult<U>>> next)
    {
        var result = await resultTask;
        if (!result.WasSuccessful)
            return result.Switch<U>();

        return await next();
    }

    public static async Task<SimpleResult> Tap(this Task<SimpleResult> resultTask, Func<Task> sideEffect)
    {
        var result = await resultTask;
        if (result.WasSuccessful)
        {
            await sideEffect();
        }

        return result;
    }

    public static SimpleResult<U> Then<T, U>(this SimpleResult<T> result, Func<T, SimpleResult<U>> next)
    {
        if (!result.WasSuccessful)
            return SimpleResult.Fail<U>(result.Errors);
        return next(result.Value!);
    }

    public static SimpleResult<T> Tap<T>(this SimpleResult<T> result, Action<T> sideEffect)
    {
        if (result.WasSuccessful)
        {
            sideEffect(result.Value!);
        }
        return result;
    }

    public static SimpleResult<U> Map<T, U>(this SimpleResult<T> result, Func<T, U> mapper)
    {
        if (!result.WasSuccessful)
            return SimpleResult.Fail<U>(result.Errors);

        var newValue = mapper(result.Value!);
        return SimpleResult.Ok(newValue);
    }

    // Synchronous extensions for non-generic SimpleResult
    public static SimpleResult<T> Then<T>(this SimpleResult result, Func<SimpleResult<T>> next)
    {
        if (!result.WasSuccessful)
            return SimpleResult.Fail<T>(result.Errors);

        return next();
    }

    public static SimpleResult Then(this SimpleResult result, Func<SimpleResult> next)
    {
        if (!result.WasSuccessful)
            return SimpleResult.Fail(result.Errors);

        return next();
    }

    public static SimpleResult Tap(this SimpleResult result, Action sideEffect)
    {
        if (result.WasSuccessful)
        {
            sideEffect();
        }

        return result;
    }

    public static SimpleResult<T> Map<T>(this SimpleResult result, Func<T> mapper)
    {
        if (!result.WasSuccessful)
            return SimpleResult.Fail<T>(result.Errors);

        var newValue = mapper();
        return SimpleResult.Ok(newValue);
    }
}