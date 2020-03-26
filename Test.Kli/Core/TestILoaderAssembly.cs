using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Core
{
    public class TestILoaderAssembly
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(ILoaderAssembly);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(3);
            sut.AssertPublicMethodPresence("IDictionary<String, Assembly> Load(String)");
            sut.AssertPublicMethodPresence("IEnumerable<Type> RegisterServices(String)");
            sut.AssertPublicMethodPresence("IEnumerable<TService> GetInstances<TService>(String)");

            sut.IsInterface.Should().BeTrue();
        }
    }
}