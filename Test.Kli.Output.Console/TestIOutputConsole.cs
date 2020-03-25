using System;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Output.Console
{
    public class TestIOutputConsole
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IOutputConsole);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IOutput));
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(2);
            sut.AssertPublicMethodPresence("Void WriteToConsole(String, Char = '')");
            sut.AssertPublicMethodPresence("Char CurrentMarker()");

            sut.IsInterface.Should().BeTrue();
        }
    }
}