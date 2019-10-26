using System;
using AutoFixture;
using FluentAssertions;
using Test;
using Xunit;

namespace Kli.Wrappers
{
    public class TestConsole: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Console), 7)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Console), typeof(IConsole))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

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