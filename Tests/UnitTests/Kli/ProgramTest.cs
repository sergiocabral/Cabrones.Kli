using System.Reflection;
using FluentAssertions;
using Kli;
using Kli.Core;
using NSubstitute;
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

        [Fact]
        public void verifica_se_o_programa_chama_a_classe_com_a_lógica_principal()
        {
            // Arrange, Given
            
            var dependencyResolver = Program.DependencyResolver = DependencyResolverForTest;

            // Act, When
            
            Program.Main();

            // Assert, Then

            dependencyResolver.GetInstance<IEngine>().Received(1).Run();
        }

    }
}