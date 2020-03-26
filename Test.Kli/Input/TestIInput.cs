using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Input
{
    public class TestIInput
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IInput);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(4);
            sut.AssertPublicMethodPresence("String ReadLine(Boolean = 'False')");
            sut.AssertPublicMethodPresence("String Read(Boolean = 'False')");
            sut.AssertPublicMethodPresence("String ReadKey()");
            sut.AssertPublicMethodPresence("Boolean HasRead()");

            sut.IsInterface.Should().BeTrue();
        }
    }
}