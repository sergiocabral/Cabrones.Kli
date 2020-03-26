using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Infrastructure
{
    public class TestIDependencyResolver
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IDependencyResolver);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(1);
            sut.AssertPublicPropertyPresence("IEnumerable<Type> InterfacesForMultipleImplementation { get; }");
            sut.AssertMyOwnPublicMethodsCount(7);
            sut.AssertPublicMethodPresence("Guid CreateScope(Nullable<Guid> = null)");
            sut.AssertPublicMethodPresence("Void DisposeScope(Guid)");
            sut.AssertPublicMethodPresence("Boolean IsActive(Guid)");
            sut.AssertPublicMethodPresence("TService GetInstance<TService>(Nullable<Guid> = null)");
            sut.AssertPublicMethodPresence("Object GetInstance(Type, Nullable<Guid> = null)");
            sut.AssertPublicMethodPresence(
                "Void Register<TService, TImplementation>(DependencyResolverLifeTime = 'PerContainer')");
            sut.AssertPublicMethodPresence("Void Register(Type, Type, DependencyResolverLifeTime = 'PerContainer')");

            sut.IsInterface.Should().BeTrue();
        }
    }
}