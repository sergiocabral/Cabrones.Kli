using Kli.Core;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.Core
{
    public class EngineTest: Test
    {
        [Fact]
        public void verifica_se_está_configurando_o_console_ao_iniciar_e_redefinindo_as_configurações_padrão_ao_sair()
        {
            // Arrange, Given

            var consoleConfiguration = Substitute.For<IConsoleConfiguration>();
            var engine = new Engine(consoleConfiguration);
            
            // Act, When
            
            engine.Run();
            
            // Assert, Then

            consoleConfiguration.Received(1).SaveCurrentColor();
            consoleConfiguration.Received(1).SetDefaultColor();
            consoleConfiguration.Received(1).RestoreColor();
        }
    }
}