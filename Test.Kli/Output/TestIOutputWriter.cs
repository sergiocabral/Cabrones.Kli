using System;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Output
{
    public class TestIOutputWriter
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IOutputWriter);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(2);
            sut.AssertPublicMethodPresence("Void Parse(String, Action<String, Char>)");
            sut.AssertPublicMethodPresence("Void Parse(String, Action<String>)");

            sut.IsInterface.Should().BeTrue();
        }
    }
}