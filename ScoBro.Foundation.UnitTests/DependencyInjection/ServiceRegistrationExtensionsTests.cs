using Microsoft.Extensions.DependencyInjection;
using ScoBro.Foundation.DependencyInjection;

namespace ScoBro.Foundation.Tests.DependencyInjection;

// Test doubles
public interface ITestScopedService { }
public class TestScopedService : ITestScopedService, IScopedService { }

public interface ITestTransientService { }
public class TestTransientService : ITestTransientService, ITransientService { }

public interface ITestSingletonService { }
public class TestSingletonService : ITestSingletonService, ISingletonService { }

public class NoInterfaceService : IScopedService { } // no matching "INoInterfaceService"

public class ServiceRegistrationExtensionsTests {
    private static IServiceCollection BuildCollection() =>
        new ServiceCollection();

    [Test]
    public void RegisterServices_ScopedMarker_RegistersWithScopedLifetime() {
        var services = BuildCollection();

        services.RegisterServicesFromAssemblyContaining<TestScopedService>();

        var descriptor = services.Single(d => d.ServiceType == typeof(ITestScopedService));
        Assert.Multiple(() => {
            Assert.That(descriptor.ImplementationType, Is.EqualTo(typeof(TestScopedService)));
            Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
        });
    }

    [Test]
    public void RegisterServices_TransientMarker_RegistersWithTransientLifetime() {
        var services = BuildCollection();

        services.RegisterServicesFromAssemblyContaining<TestTransientService>();

        var descriptor = services.Single(d => d.ServiceType == typeof(ITestTransientService));
        Assert.Multiple(() => {
            Assert.That(descriptor.ImplementationType, Is.EqualTo(typeof(TestTransientService)));
            Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Transient));
        });
    }

    [Test]
    public void RegisterServices_SingletonMarker_RegistersWithSingletonLifetime() {
        var services = BuildCollection();

        services.RegisterServicesFromAssemblyContaining<TestSingletonService>();

        var descriptor = services.Single(d => d.ServiceType == typeof(ITestSingletonService));
        Assert.Multiple(() => {
            Assert.That(descriptor.ImplementationType, Is.EqualTo(typeof(TestSingletonService)));
            Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
        });
    }

    [Test]
    public void RegisterServices_ClassWithNoMatchingInterface_IsSkipped() {
        var services = BuildCollection();

        services.RegisterServicesFromAssemblyContaining<NoInterfaceService>();

        Assert.That(services.Any(d => d.ImplementationType == typeof(NoInterfaceService)), Is.False);
    }

    [Test]
    public void RegisterServicesFromAssemblyContaining_DelegatesToRegisterServices() {
        var services1 = BuildCollection();
        var services2 = BuildCollection();

        services1.RegisterServicesFromAssemblyContaining<TestScopedService>();
        services2.RegisterServices(typeof(TestScopedService).Assembly);

        Assert.That(
            services1.Select(d => (d.ServiceType, d.ImplementationType, d.Lifetime)),
            Is.EquivalentTo(services2.Select(d => (d.ServiceType, d.ImplementationType, d.Lifetime))));
    }
}
