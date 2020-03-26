using System;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.Wrappers
{
    public class TestEnvironment
    {
        [Fact]
        public void GetEnvironmentVariable_deve_retornar_null_se_não_existe_o_valor()
        {
            // Arrange, Given

            var environment = Program.DependencyResolver.GetInstance<IEnvironment>();

            // Act, When

            var valorNãoExistente = environment.GetEnvironmentVariable(this.Fixture<string>());

            // Assert, Then   

            valorNãoExistente.Should().BeNull();
        }

        [Fact]
        public void métodos_devem_rodar_sem_erros()
        {
            // Arrange, Given

            var environment = Program.DependencyResolver.GetInstance<IEnvironment>();

            // Act, When

            Action executarTodosOsMétodos = () => { environment.GetEnvironmentVariable(this.Fixture<string>()); };

            // Assert, Then   

            executarTodosOsMétodos.Should().NotThrow();
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Environment);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IEnvironment));
            sut.AssertMyOwnImplementations(
                typeof(IEnvironment));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}