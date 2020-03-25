using System;
using Cabrones.Test;
using FluentAssertions;
using Kli.Core;
using Xunit;

namespace Kli.Input
{
    public class TestIMultipleInput
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IMultipleInput);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IMultiple<IInput>), 
                typeof(IInput));
            sut.AssertMyOwnImplementations(
                typeof(IMultiple<IInput>), 
                typeof(IInput));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsInterface.Should().BeTrue();
        }
    }
}