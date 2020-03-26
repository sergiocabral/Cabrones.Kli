using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Wrappers
{
    public class TestIConsole
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(IConsole);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(11);
            sut.AssertPublicPropertyPresence("ConsoleColor ForegroundColor { get; set; }");
            sut.AssertPublicPropertyPresence("ConsoleColor BackgroundColor { get; set; }");
            sut.AssertPublicPropertyPresence("Boolean KeyAvailable { get; }");
            sut.AssertPublicPropertyPresence("Int32 CursorTop { get; set; }");
            sut.AssertPublicPropertyPresence("Int32 CursorLeft { get; set; }");
            sut.AssertPublicPropertyPresence("Int32 BufferHeight { get; }");
            sut.AssertPublicPropertyPresence("Int32 BufferWidth { get; }");
            sut.AssertMyOwnPublicMethodsCount(6);
            sut.AssertPublicMethodPresence("Void ResetColor()");
            sut.AssertPublicMethodPresence("Void Write(String)");
            sut.AssertPublicMethodPresence("Void WriteLine(String)");
            sut.AssertPublicMethodPresence("String ReadLine()");
            sut.AssertPublicMethodPresence("ConsoleKeyInfo ReadKey()");
            sut.AssertPublicMethodPresence("Void SetCursorPosition(Int32, Int32)");

            sut.IsInterface.Should().BeTrue();
        }
    }
}