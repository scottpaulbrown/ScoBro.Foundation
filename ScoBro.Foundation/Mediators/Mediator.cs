using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ScoBro.Foundation.Mediators;

/// <summary>
/// Dispatches requests to their registered handlers, decoupling request senders from request handlers.
/// </summary>
public interface IMediator {
    /// <summary>
    /// Sends a request that returns a value and awaits the handler's response.
    /// </summary>
    /// <typeparam name="TRequest">The request type, which must implement <see cref="IRequestBase{TResult}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by the handler.</typeparam>
    /// <param name="request">The request to dispatch.</param>
    /// <param name="cancellationToken">A token to observe for cancellation.</param>
    /// <returns>A task containing a <see cref="SimpleResult{TResult}"/> with the handler's outcome.</returns>
    Task<SimpleResult<TResult>> Send<TRequest, TResult>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequestBase<TResult>;

    /// <summary>
    /// Sends a request that does not return a value and awaits the handler's response.
    /// </summary>
    /// <typeparam name="TRequest">The request type, which must implement <see cref="IRequestBase"/>.</typeparam>
    /// <param name="request">The request to dispatch.</param>
    /// <param name="cancellationToken">A token to observe for cancellation.</param>
    /// <returns>A task containing a <see cref="SimpleResult"/> with the handler's outcome.</returns>
    Task<SimpleResult> Send<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequestBase;
}

/// <summary>
/// Default implementation of <see cref="IMediator"/> that resolves request handlers from the
/// dependency injection container and delegates request execution to them.
/// </summary>
public class Mediator(IServiceProvider serviceProvider, ILogger<Mediator> logger) : IMediator {
    /// <inheritdoc/>
    public async Task<SimpleResult<TResult>> Send<TRequest, TResult>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequestBase<TResult> {
        ArgumentNullException.ThrowIfNull(request);
        var handler = serviceProvider.GetService<IRequestHandler<TRequest, TResult>>();
        if (handler is null) {
            logger.LogError("No handler found for request type {RequestType}", typeof(TRequest).Name);
            return SimpleResult.FailWithSystemError<TResult>("No handler found for request");
        }

        try {
            return await handler.HandleAsync(request, cancellationToken);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception in handler for request type {RequestType}", typeof(TRequest).Name);
            return SimpleResult.FailWithSystemError<TResult>("An unexpected error occurred while handling the request");
        }
    }

    /// <inheritdoc/>
    public async Task<SimpleResult> Send<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequestBase {
        ArgumentNullException.ThrowIfNull(request);
        var handler = serviceProvider.GetService<IRequestHandler<TRequest>>();
        if (handler is null) {
            logger.LogError("No handler found for request type {RequestType}", typeof(TRequest).Name);
            return SimpleResult.FailWithSystemError("No handler found for request");
        }

        try {
            return await handler.HandleAsync(request, cancellationToken);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception in handler for request type {RequestType}", typeof(TRequest).Name);
            return SimpleResult.FailWithSystemError("An unexpected error occurred while handling the request");
        }
    }
}
