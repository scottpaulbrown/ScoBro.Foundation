namespace ScoBro.Foundation.DependencyInjection;

/// <summary>
/// Marker interface for services that should be registered with a scoped lifetime.
/// A new instance is created once per request/scope.
/// </summary>
public interface IScopedService { }

/// <summary>
/// Marker interface for services that should be registered with a transient lifetime.
/// A new instance is created every time the service is requested.
/// </summary>
public interface ITransientService { }

/// <summary>
/// Marker interface for services that should be registered with a singleton lifetime.
/// A single instance is created and shared for the lifetime of the application.
/// </summary>
public interface ISingletonService { }
