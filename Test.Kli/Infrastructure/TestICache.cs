using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Infrastructure
{
    public class TestICache
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(ICache);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(3);
            sut.AssertPublicMethodPresence("T Set<T>(String, T)");
            sut.AssertPublicMethodPresence("T Get<T>(String)");
            sut.AssertPublicMethodPresence("Void Clear()");

            sut.IsInterface.Should().BeTrue();
        }
    }
}