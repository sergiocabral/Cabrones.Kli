using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Input.Console
{
    public class TestIInputConsole
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IInputConsole);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IInput));
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsInterface.Should().BeTrue();
        }
    }
}