using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Module
{
    public class TestIInteraction
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IInteraction);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(1);
            sut.AssertPublicMethodPresence("Void StartInteraction()");

            sut.IsInterface.Should().BeTrue();
        }
    }
}