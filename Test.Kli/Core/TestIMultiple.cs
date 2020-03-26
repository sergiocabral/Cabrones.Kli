using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Core
{
    public class TestIMultiple
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IMultiple<>);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(1);
            sut.AssertPublicPropertyPresence("IList<TService> Instances { get; }");
            sut.AssertMyOwnPublicMethodsCount(1);

            sut.IsInterface.Should().BeTrue();
        }
    }
}