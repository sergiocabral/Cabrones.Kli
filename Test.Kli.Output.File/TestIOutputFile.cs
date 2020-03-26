using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Output.File
{
    public class TestIOutputFile
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IOutputFile);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IOutput));
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(1);
            sut.AssertPublicPropertyPresence("String Path { get; }");
            sut.AssertMyOwnPublicMethodsCount(1);
            sut.AssertPublicMethodPresence("Void WriteToFile(String)");

            sut.IsInterface.Should().BeTrue();
        }
    }
}