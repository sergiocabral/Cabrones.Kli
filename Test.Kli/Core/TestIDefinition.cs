using System;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Core
{
    public class TestIDefinition
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IDefinition);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(3);
            sut.AssertPublicPropertyPresence("String DirectoryOfProgram { get; }");
            sut.AssertPublicPropertyPresence("String DirectoryOfUser { get; }");
            sut.AssertPublicPropertyPresence("Boolean CanWriteIntoDirectoryOfUser { get; }");
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsInterface.Should().BeTrue();
        }
    }
}