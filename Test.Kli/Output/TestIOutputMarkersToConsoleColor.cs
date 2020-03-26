using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Output
{
    public class TestIOutputMarkersToConsoleColor
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IOutputMarkersToConsoleColor);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(2);
            sut.AssertPublicMethodPresence("Tuple<ConsoleColor, ConsoleColor> Convert(Char)");
            sut.AssertPublicMethodPresence("Char Convert(ConsoleColor, ConsoleColor = 'Black')");

            sut.IsInterface.Should().BeTrue();
        }
    }
}