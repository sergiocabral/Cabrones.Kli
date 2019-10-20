using System;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using Xunit;

namespace Tests.UnitTests.Kli.Core
{
    public class ConsoleConfigurationTest: Test
    {
        [Fact]
        public void deve_ser_capaz_de_gravar_as_cores_atuais_do_console()
        {
            // Arrange, Given
            
            var consoleConfiguration = DependencyResolverFromProgram.GetInstance<IConsoleConfiguration>();

            Console.BackgroundColor++;
            var backupParaBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor++;
            var backupParaForegroundColor = Console.ForegroundColor;
            
            // Act, When

            consoleConfiguration.SaveCurrentColor();

            // Assert, Then

            consoleConfiguration.BackgroundColorBackup.Should().Be(backupParaBackgroundColor);
            consoleConfiguration.ForegroundColorBackup.Should().Be(backupParaForegroundColor);
        }
        
        [Fact]
        public void deve_ser_capaz_de_restaurar_as_cores_do_console_anteriormente_gravadas()
        {
            // Arrange, Given
            
            var consoleConfiguration = DependencyResolverFromProgram.GetInstance<IConsoleConfiguration>();
            
            var backupParaBackgroundColor = consoleConfiguration.BackgroundColorBackup;
            Console.BackgroundColor++;

            var backupParaForegroundColor = consoleConfiguration.ForegroundColorBackup;
            Console.ForegroundColor++;
            
            // Act, When

            consoleConfiguration.RestoreColor();

            // Assert, Then

            consoleConfiguration.BackgroundColorBackup.Should().Be(backupParaBackgroundColor);
            consoleConfiguration.ForegroundColorBackup.Should().Be(backupParaForegroundColor);
        }
        
        [Fact]
        public void deve_ser_capaz_de_gravar_e_restaurar_as_cores_atuais_do_console()
        {
            // Arrange, Given
            
            var consoleConfiguration = DependencyResolverFromProgram.GetInstance<IConsoleConfiguration>();
            
            Console.BackgroundColor = Fixture.Create<ConsoleColor>();
            var backupParaBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = Fixture.Create<ConsoleColor>();
            var backupParaForegroundColor = Console.ForegroundColor;

            consoleConfiguration.SaveCurrentColor();

            // Act, When

            Console.BackgroundColor++;
            Console.ForegroundColor++;
            
            consoleConfiguration.RestoreColor();

            // Assert, Then

            Console.ForegroundColor.Should().Be(backupParaForegroundColor);
            Console.BackgroundColor.Should().Be(backupParaBackgroundColor);
        }
        
        [Fact]
        public void verifica_se_as_cores_padrão_estao_sendo_definidas_quando_solicitado()
        {
            // Arrange, Given
            
            var consoleConfiguration = DependencyResolverFromProgram.GetInstance<IConsoleConfiguration>();
            
            // Act, When

            var novoValorParaForegroundColor = Console.ForegroundColor = Fixture.Create<ConsoleColor>();
            var novoValorParaBackgroundColor = Console.BackgroundColor = Fixture.Create<ConsoleColor>();
            
            consoleConfiguration.SetDefaultColor();

            // Assert, Then

            Console.ForegroundColor.Should().Be(ConsoleConfiguration.ForegroundColorDefault).And.NotBe(novoValorParaForegroundColor);
            Console.BackgroundColor.Should().Be(ConsoleConfiguration.BackgroundColorDefault).And.NotBe(novoValorParaBackgroundColor);
        }
    }
}