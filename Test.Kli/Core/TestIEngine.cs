using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Core
{
    public class TestIEngine
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IEngine);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(1);
            sut.AssertPublicMethodPresence("Void Initialize()");

            sut.IsInterface.Should().BeTrue();
        }
    }
}