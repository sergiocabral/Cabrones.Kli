using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Output
{
    public class TestIOutputMarkers
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IOutputMarkers);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(9);
            sut.AssertPublicPropertyPresence("Char Error { get; }");
            sut.AssertPublicPropertyPresence("Char Question { get; }");
            sut.AssertPublicPropertyPresence("Char Answer { get; }");
            sut.AssertPublicPropertyPresence("Char Highlight { get; }");
            sut.AssertPublicPropertyPresence("Char Light { get; }");
            sut.AssertPublicPropertyPresence("Char Low { get; }");
            sut.AssertPublicPropertyPresence("String Markers { get; }");
            sut.AssertPublicPropertyPresence("String MarkersEscapedForRegexJoined { get; }");
            sut.AssertPublicPropertyPresence("String[] MarkersEscapedForRegexSeparated { get; }");
            sut.AssertMyOwnPublicMethodsCount(1);

            sut.IsInterface.Should().BeTrue();
        }
    }
}