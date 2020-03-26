using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Output
{
    public class TestIOutput
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IOutput);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(2);
            sut.AssertPublicMethodPresence("IOutput Write(String)");
            sut.AssertPublicMethodPresence("IOutput WriteLine(String)");

            sut.IsInterface.Should().BeTrue();
        }
    }
}