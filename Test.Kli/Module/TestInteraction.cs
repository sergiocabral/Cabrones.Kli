using System;
using FluentAssertions;
using Kli.Input;
using Kli.Output;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Module
{
    public class TestInteraction: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Interaction), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Interaction), typeof(IInteraction))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

        [Fact]
        public void o_método_que_inicia_a_interação_com_o_usuário_só_pode_ser_chamado_uma_vez()
        {
            // Arrange, Given

            var utils = new Interaction(Substitute.For<IMultipleOutput>(), Substitute.For<IMultipleInput>());

            // Act, When

            Action chamarMétodoStartInteraction = () => utils.StartInteraction();

            // Assert, Then
            chamarMétodoStartInteraction.Should().NotThrow();
            chamarMétodoStartInteraction.Should().Throw<InvalidOperationException>();
        }
    }
}