using System;
using System.Threading;
using Kli.Core;
using Kli.Infrastructure;
using NSubstitute;
using Cabrones.Test;
using Xunit;

namespace Kli
{
    public class TestProgram
    {
        [Theory]
        [InlineData(typeof(Program), 3)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            tipo.TestTypeMethodsCount(totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Program), "IDependencyResolver get_DependencyResolver()")]
        [InlineData(typeof(Program), "Void Main()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            tipo.TestTypeMethodSignature(assinaturaEsperada);
        
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