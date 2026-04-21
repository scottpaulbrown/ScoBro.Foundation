using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ScoBro.Foundation.Mediators;

/// <summary>
/// Provides a fluent builder for configuring which assemblies the mediator will scan
/// for <see cref="IRequestHandler{TRequest}"/> and <see cref="IRequestHandler{TRequest,TResult}"/> implementations.
/// </summary>
public class MediatorOptionsBuilder {
    /// <summary>
    /// Gets the list of assemblies that will be scanned for request handler implementations.
    /// </summary>
    public List<Assembly> AssembliesToScan { get; private set; } = [];

    /// <summary>
    /// Adds the assembly that contains type <typeparamref name="T"/> to the scan list.
    /// Duplicate assemblies are ignored at scan time.
    /// </summary>
    /// <typeparam name="T">Any type whose containing assembly should be scanned.</typeparam>
    /// <returns>This builder instance, for method chaining.</returns>
    public MediatorOptionsBuilder ScanAssemblyFrom<T>() {
        AssembliesToScan.Add(typeof(T).Assembly);
        return this;
    }
}

/// <summary>
/// Extension methods for registering the mediator and its request handlers with
/// the <see cref="IServiceCollection"/> dependency injection container.
/// </summary>
public static class MediatorExtensions {
    /// <summary>
    /// Registers <see cref="IMediator"/> and all <see cref="IRequestHandler{TRequest}"/> /
    /// <see cref="IRequestHandler{TRequest,TResult}"/> implementations found in the assemblies
    /// configured via <paramref name="optionsBuilder"/> as scoped services.
    /// </summary>
    /// <param name="services">The service collection to register into.</param>
    /// <param name="optionsBuilder">
    /// A delegate that configures which assemblies are scanned for handler implementations.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> for further chaining.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services, Action<MediatorOptionsBuilder> optionsBuilder) {
        var options = new MediatorOptionsBuilder();
        optionsBuilder(options);

        services.AddScoped<IMediator, Mediator>();

        foreach (var assembly in options.AssembliesToScan.Distinct()) {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in types) {
                var handlerInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                               (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

                foreach (var handlerInterface in handlerInterfaces) {
                    services.AddScoped(handlerInterface, type);
                }
            }
        }

        return services;
    }
}
