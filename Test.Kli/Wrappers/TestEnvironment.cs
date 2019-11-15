using System;
using AutoFixture;
using FluentAssertions;
using Cabrones.Test;
using Xunit;

namespace Kli.Wrappers
{
    public class TestEnvironment
    {
        [Theory]
        [InlineData(typeof(Environment), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestMethodsCount(totalDeMétodosEsperado);
        
        [Theory]
        [InlineData(typeof(Environment), typeof(IEnvironment))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            tipoDaClasse.TestImplementations(tiposQueDeveSerImplementado);

        [Fact]
        public void métodos_devem_rodar_sem_erros()
        {
            // Arrange, Given

            var environment = Program.DependencyResolver.GetInstance<IEnvironment>();

            // Act, When

            Action executarTodosOsMétodos = () =>
            {
                environment.GetEnvironmentVariable(this.Fixture().Create<string>());
            };

            // Assert, Then   
            
            executarTodosOsMétodos.Should().NotThrow();
        }

        [Fact]
        public void GetEnvironmentVariable_deve_retornar_null_se_não_existe_o_valor()
        {
            // Arrange, Given

            var environment = Program.DependencyResolver.GetInstance<IEnvironment>();

            // Act, When

            var valorNãoExistente = environment.GetEnvironmentVariable(this.Fixture().Create<string>());

            // Assert, Then   

            valorNãoExistente.Should().BeNull();
        }
    }
}