using System;
using System.Threading;
using Kli.Core;
using Kli.Infrastructure;
using NSubstitute;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli
{
    public class TestProgram
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Program);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(2);
            sut.AssertPublicPropertyPresence("static IDependencyResolver DependencyResolver { get; set; }");
            sut.AssertMyOwnPublicMethodsCount(1);
            sut.AssertPublicMethodPresence("static Void Main()");

            sut.IsClass.Should().BeTrue();
        }
        
        [Fact]
        public void verifica_se_o_programa_chama_a_classe_com_a_lógica_principal()
        {
            // Arrange, Given
            
            var dependencyResolver = Substitute.For<IDependencyResolver>();

            // Espera um tempo para reduzir a possibilidade deste teste
            // rodar simultaneamente com outros testes.
            // O motivo disso está explicado no próximo comentário.
            Thread.Sleep(1000);
            
            // Esta troca pode ocasionar erros em outros testes.
            // O motivo é que está trocando o resolvedor de dependências
            // principal do programa.
            Program.DependencyResolver = dependencyResolver;

            // Act, When
            
            Program.Main();

            // Assert, Then
            
            // Aqui o resolvedor de dependências é restaurado para o original.
            Program.DependencyResolver = null;

            dependencyResolver.GetInstance<IEngine>().Received(1).Initialize();
        }
    }
}