using System;
using Cabrones.Test;
using FluentAssertions;
using Kli.Core;
using NSubstitute;
using Xunit;

namespace Kli.Input
{
    public class TestMultipleInput
    {
        [Fact]
        public void
            o_método_HasRead_deve_chamar_o_mesmo_método_de_todos_as_instâncias_adicionados_caso_todas_sejam_false()
        {
            // Arrange, Given

            var multipleInput = new MultipleInput();

            var input1 = Substitute.For<IInput>();
            var input2 = Substitute.For<IInput>();
            var input3 = Substitute.For<IInput>();

            multipleInput.Add(input1);
            multipleInput.Add(input2);
            multipleInput.Add(input3);

            // Act, When

            multipleInput.HasRead();

            // Assert, Then

            input1.Received(1).HasRead();
            input2.Received(1).HasRead();
            input3.Received(1).HasRead();
        }

        [Fact]
        public void
            o_método_HasRead_deve_ir_chamando_o_mesmo_método_de_todos_as_instâncias_adicionados_até_retornar_true()
        {
            // Arrange, Given

            var multipleInput = new MultipleInput();

            var input1 = Substitute.For<IInput>();
            var input2 = Substitute.For<IInput>();
            input2.HasRead().Returns(true);
            var input3 = Substitute.For<IInput>();

            multipleInput.Add(input1);
            multipleInput.Add(input2);
            multipleInput.Add(input3);

            // Act, When

            multipleInput.HasRead();

            // Assert, Then

            input1.Received(1).HasRead();
            input2.Received(1).HasRead();
            input3.Received(0).HasRead();
        }

        [Fact]
        public void o_método_Read_deve_chamar_o_mesmo_método_da_primeira_instância_que_puder_responder()
        {
            // Arrange, Given

            var multipleInput = new MultipleInput();

            var input1 = Substitute.For<IInput>();

            var depoisDeQuantasTentativasHasReadRetornaTrue = new Random(this.Fixture<int>()).Next(3, 6);
            var input2Resposta = this.Fixture<string>();
            var input2 = Substitute.For<IInput>();
            input2.Read().Returns(input2Resposta);
            input2.HasRead().Returns(info =>
            {
                depoisDeQuantasTentativasHasReadRetornaTrue--;
                return depoisDeQuantasTentativasHasReadRetornaTrue < 0;
            });

            var input3 = Substitute.For<IInput>();

            multipleInput.Add(input1);
            multipleInput.Add(input2);
            multipleInput.Add(input3);

            // Act, When

            var resposta = multipleInput.Read();

            // Assert, Then

            resposta.Should().Be(input2Resposta);
        }

        [Fact]
        public void o_método_ReadKey_deve_chamar_o_mesmo_método_da_primeira_instância_que_puder_responder()
        {
            // Arrange, Given

            var multipleInput = new MultipleInput();

            var input1 = Substitute.For<IInput>();

            var depoisDeQuantasTentativasHasReadRetornaTrue = new Random(this.Fixture<int>()).Next(3, 6);
            var input2Resposta = this.Fixture<string>();
            var input2 = Substitute.For<IInput>();
            input2.ReadKey().Returns(input2Resposta);
            input2.HasRead().Returns(info =>
            {
                depoisDeQuantasTentativasHasReadRetornaTrue--;
                return depoisDeQuantasTentativasHasReadRetornaTrue < 0;
            });

            var input3 = Substitute.For<IInput>();

            multipleInput.Add(input1);
            multipleInput.Add(input2);
            multipleInput.Add(input3);

            // Act, When

            var resposta = multipleInput.ReadKey();

            // Assert, Then

            resposta.Should().Be(input2Resposta);
        }

        [Fact]
        public void o_método_ReadLine_deve_chamar_o_mesmo_método_da_primeira_instância_que_puder_responder()
        {
            // Arrange, Given

            var multipleInput = new MultipleInput();

            var input1 = Substitute.For<IInput>();

            var depoisDeQuantasTentativasHasReadRetornaTrue = new Random(this.Fixture<int>()).Next(3, 6);
            var input2Resposta = this.Fixture<string>();
            var input2 = Substitute.For<IInput>();
            input2.ReadLine().Returns(input2Resposta);
            input2.HasRead().Returns(info =>
            {
                depoisDeQuantasTentativasHasReadRetornaTrue--;
                return depoisDeQuantasTentativasHasReadRetornaTrue < 0;
            });

            var input3 = Substitute.For<IInput>();

            multipleInput.Add(input1);
            multipleInput.Add(input2);
            multipleInput.Add(input3);

            // Act, When

            var resposta = multipleInput.ReadLine();

            // Assert, Then

            resposta.Should().Be(input2Resposta);
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(MultipleInput);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(Multiple<IInput>),
                typeof(IMultipleInput),
                typeof(IMultiple<IInput>),
                typeof(IInput));
            sut.AssertMyOwnImplementations(
                typeof(Multiple<IInput>),
                typeof(IMultipleInput));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}