using System;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.i18n
{
    public class TestILanguage
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(ILanguage);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(2);
            sut.AssertPublicPropertyPresence("IEnumerable<String> EnvironmentVariables { get; }");
            sut.AssertPublicPropertyPresence("String Current { get; }");
            sut.AssertMyOwnPublicMethodsCount(2);

            sut.IsInterface.Should().BeTrue();
        }
    }
}