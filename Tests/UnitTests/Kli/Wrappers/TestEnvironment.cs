using System;
using AutoFixture;
using FluentAssertions;
using Kli.Wrappers;
using Xunit;
using Environment = Kli.Wrappers.Environment;

namespace Tests.UnitTests.Kli.Wrappers
{
    public class TestEnvironment: Test
    {
        [Theory]
        [InlineData(typeof(Environment), typeof(IEnvironment))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, Type tipoQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tipoQueDeveSerImplementado);

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