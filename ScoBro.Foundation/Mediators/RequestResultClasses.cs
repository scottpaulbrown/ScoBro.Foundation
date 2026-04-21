namespace ScoBro.Foundation.Mediators;

/// <summary>
/// Base marker interface for all mediator requests that do not return a value.
/// </summary>
public interface IRequestBase { }

/// <summary>
/// Base marker interface for all mediator requests that return a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">The type of the value returned by the request.</typeparam>
public interface IRequestBase<TResult> : IRequestBase { }

/// <summary>
/// Marker interface for command requests that do not return a value.
/// Commands represent operations that change state (write operations).
/// </summary>
public interface ICommandRequest : IRequestBase { }

/// <summary>
/// Marker interface for command requests that return a value of type <typeparamref name="TResult"/>.
/// Commands represent operations that change state (write operations).
/// </summary>
/// <typeparam name="TResult">The type of the value returned by the command.</typeparam>
public interface ICommandRequest<TResult> : IRequestBase<TResult>, ICommandRequest { }

/// <summary>
/// Marker interface for query requests that do not return a value.
/// Queries represent read-only operations that do not change state.
/// </summary>
public interface IQueryRequest : IRequestBase { }

/// <summary>
/// Marker interface for query requests that return a value of type <typeparamref name="TResult"/>.
/// Queries represent read-only operations that do not change state.
/// </summary>
/// <typeparam name="TResult">The type of the value returned by the query.</typeparam>
public interface IQueryRequest<TResult> : IRequestBase<TResult>, IQueryRequest { }

/// <summary>
/// Defines a handler for a request that returns a value of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TRequest">The request type, which must implement <see cref="IRequestBase{TResult}"/>.</typeparam>
/// <typeparam name="TResult">The type of the value returned by the handler.</typeparam>
public interface IRequestHandler<TRequest, TResult> where TRequest : IRequestBase<TResult> {
    /// <summary>
    /// Handles the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A token to observe for cancellation.</param>
    /// <returns>A task containing a <see cref="SimpleResult{TResult}"/> with the handler's outcome.</returns>
    Task<SimpleResult<TResult>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for a request that does not return a value.
/// </summary>
/// <typeparam name="TRequest">The request type, which must implement <see cref="IRequestBase"/>.</typeparam>
public interface IRequestHandler<TRequest> where TRequest : IRequestBase {
    /// <summary>
    /// Handles the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A token to observe for cancellation.</param>
    /// <returns>A task containing a <see cref="SimpleResult"/> with the handler's outcome.</returns>
    Task<SimpleResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
