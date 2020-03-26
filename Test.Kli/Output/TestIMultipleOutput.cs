using Cabrones.Test;
using FluentAssertions;
using Kli.Core;
using Xunit;

namespace Kli.Output
{
    public class TestIMultipleOutput
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IMultipleOutput);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IMultiple<IOutput>),
                typeof(IOutput));
            sut.AssertMyOwnImplementations(
                typeof(IMultiple<IOutput>),
                typeof(IOutput));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsInterface.Should().BeTrue();
        }
    }
}