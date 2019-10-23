using System;
using AutoFixture;
using FluentAssertions;
using Kli.IO;
using Xunit;
using Console = System.Console;

namespace Tests.UnitTests.Kli.IO
{
    public class TestConsole: Test
    {
        [Theory]
        [InlineData(typeof(Console), typeof(IConsole))]
        public void verifica_se_classe_implementa_tipos(Type tipoDaClasse, Type tipoQueDeveSerImplementado)
        {
            verifica_se_classe_implementa_tipo(tipoDaClasse, tipoQueDeveSerImplementado);
        }

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