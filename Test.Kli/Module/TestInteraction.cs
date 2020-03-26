using System;
using Cabrones.Test;
using FluentAssertions;
using Kli.Input;
using Kli.Output;
using NSubstitute;
using Xunit;

namespace Kli.Module
{
    public class TestInteraction
    {
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

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Interaction);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IInteraction));
            sut.AssertMyOwnImplementations(
                typeof(IInteraction));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}