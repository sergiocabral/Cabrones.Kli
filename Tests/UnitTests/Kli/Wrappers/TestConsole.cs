using System;
using AutoFixture;
using FluentAssertions;
using Kli.Wrappers;
using Xunit;
using Console = Kli.Wrappers.Console;

namespace Tests.UnitTests.Kli.Wrappers
{
    public class TestConsole: Test
    {
        [Theory]
        [InlineData(typeof(Console), typeof(IConsole))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, Type tipoQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tipoQueDeveSerImplementado);

        [Fact]
        public void métodos_devem_rodar_sem_erros()
        {
            // Arrange, Given

            var console = DependencyResolverFromProgram.GetInstance<IConsole>();

            // Act, When

            Action executarTodosOsMétodos = () =>
            {
                console.BackgroundColor = console.ForegroundColor;
                console.ForegroundColor = console.BackgroundColor;
                console.ResetColor();
                console.Write(Fixture.Create<string>());
                console.WriteLine(Fixture.Create<string>());
            };

            // Assert, Then   
            
            executarTodosOsMétodos.Should().NotThrow();
        }
    }
}