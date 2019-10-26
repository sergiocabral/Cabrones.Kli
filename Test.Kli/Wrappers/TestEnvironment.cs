using System;
using AutoFixture;
using FluentAssertions;
using Test;
using Xunit;

namespace Kli.Wrappers
{
    public class TestEnvironment: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Environment), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);
        
        [Theory]
        [InlineData(typeof(Environment), typeof(IEnvironment))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

        [Fact]
        public void métodos_devem_rodar_sem_erros()
        {
            // Arrange, Given

            var environment = DependencyResolverFromProgram.GetInstance<IEnvironment>();

            // Act, When

            Action executarTodosOsMétodos = () =>
            {
                environment.GetEnvironmentVariable(Fixture.Create<string>());
            };

            // Assert, Then   
            
            executarTodosOsMétodos.Should().NotThrow();
        }

        [Fact]
        public void GetEnvironmentVariable_deve_retornar_null_se_não_existe_o_valor()
        {
            // Arrange, Given

            var environment = DependencyResolverFromProgram.GetInstance<IEnvironment>();

            // Act, When

            var valorNãoExistente = environment.GetEnvironmentVariable(Fixture.Create<string>());

            // Assert, Then   

            valorNãoExistente.Should().BeNull();
        }
    }
}