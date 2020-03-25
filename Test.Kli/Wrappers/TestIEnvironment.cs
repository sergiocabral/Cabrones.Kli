using System;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Wrappers
{
    public class TestIEnvironment
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IEnvironment);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(1);
            sut.AssertPublicMethodPresence("String GetEnvironmentVariable(String)");

            sut.IsInterface.Should().BeTrue();
        }
    }
}