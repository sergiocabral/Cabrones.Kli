using Cabrones.Test;
using FluentAssertions;
using Kli.Core;
using Xunit;

namespace Kli.Module
{
    public class TestIMultipleModule
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IMultipleModule);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IMultiple<IModule>),
                typeof(IModule));
            sut.AssertMyOwnImplementations(
                typeof(IMultiple<IModule>),
                typeof(IModule));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsInterface.Should().BeTrue();
        }
    }
}