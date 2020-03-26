using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Wrappers
{
    public class TestConsole
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Console);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IConsole));
            sut.AssertMyOwnImplementations(
                typeof(IConsole));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}