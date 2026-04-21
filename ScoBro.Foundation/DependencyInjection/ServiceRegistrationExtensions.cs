using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ScoBro.Foundation.DependencyInjection;

/// <summary>
/// Extension methods for registering services via convention-based scanning using the
/// <see cref="IScopedService"/>, <see cref="ITransientService"/>, and <see cref="ISingletonService"/> marker interfaces.
/// </summary>
public static class ServiceRegistrationExtensions {
    /// <summary>
    /// Scans the assembly containing <typeparamref name="T"/> and registers all classes that implement
    /// <see cref="IScopedService"/>, <see cref="ITransientService"/>, or <see cref="ISingletonService"/>
    /// with their matching interface and corresponding lifetime.
    /// </summary>
    /// <typeparam name="T">A type whose assembly will be scanned for service registrations.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add registrations to.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection RegisterServicesFromAssemblyContaining<T>(this IServiceCollection services) {
        return services.RegisterServices(typeof(T).Assembly);
    }

    /// <summary>
    /// Scans the provided assemblies and registers all classes that implement
    /// <see cref="IScopedService"/>, <see cref="ITransientService"/>, or <see cref="ISingletonService"/>
    /// with their matching interface and corresponding lifetime.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add registrations to.</param>
    /// <param name="assemblies">One or more assemblies to scan for service registrations.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection RegisterServices(this IServiceCollection services, params Assembly[] assemblies) {
        foreach (var type in GetConcreteClassesAssignableTo<IScopedService>(assemblies))
            RegisterWithMatchingInterface(services, type, ServiceLifetime.Scoped);

        foreach (var type in GetConcreteClassesAssignableTo<ITransientService>(assemblies))
            RegisterWithMatchingInterface(services, type, ServiceLifetime.Transient);

        foreach (var type in GetConcreteClassesAssignableTo<ISingletonService>(assemblies))
            RegisterWithMatchingInterface(services, type, ServiceLifetime.Singleton);

        return services;
    }

    private static IEnumerable<Type> GetConcreteClassesAssignableTo<TMarker>(Assembly[] assemblies) =>
        assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && typeof(TMarker).IsAssignableFrom(t));

    private static void RegisterWithMatchingInterface(IServiceCollection services, Type implementationType, ServiceLifetime lifetime) {
        var matchingInterface = implementationType.GetInterfaces()
            .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

        if (matchingInterface is null)
            return;

        services.Add(new ServiceDescriptor(matchingInterface, implementationType, lifetime));
    }
}
