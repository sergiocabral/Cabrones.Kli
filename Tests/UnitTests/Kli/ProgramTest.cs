using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Tests.UnitTests.Kli
{
    public class ProgramTest: Test
    {
        [Fact]
        public void sendo_um_programa_console_deve_existir_o_metodo_estático_main()
        {
            // Arrange, Given
            
            var tipoDaClassQueContemOMétodoMain = typeof(Program);

            // Act, When
            
            var métodoMain = tipoDaClassQueContemOMétodoMain.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);

            // Assert, Then
            
            métodoMain.Should().NotBeNull();
        }
    }
}