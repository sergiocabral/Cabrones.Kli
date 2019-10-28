using System;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Input
{
    public class TestMultipleInput: BaseForTest
    {
        [Theory]
        [InlineData(typeof(MultipleInput), 4)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(MultipleInput), typeof(IMultiple<IInput>), typeof(Multiple<IInput>), typeof(IMultipleInput), typeof(IInput))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);
        
        [Fact]
        public void o_método_ReadLine_deve_chamar_o_mesmo_método_da_primeira_instância_que_puder_responder()
        {
            // Arrange, Given

            var multipleInput = new MultipleInput();

            var input1 = Substitute.For<IInput>();

            var depoisDeQuantasTentativasHasReadRetornaTrue = Fixture.Create<int>() + 5;
            var input2Resposta = Fixture.Create<string>();
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
        public void o_método_Read_deve_chamar_o_mesmo_método_da_primeira_instância_que_puder_responder()
        {
            // Arrange, Given

            var multipleInput = new MultipleInput();

            var input1 = Substitute.For<IInput>();

            var depoisDeQuantasTentativasHasReadRetornaTrue = Fixture.Create<int>() + 5;
            var input2Resposta = Fixture.Create<string>();
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

            var depoisDeQuantasTentativasHasReadRetornaTrue = Fixture.Create<int>() + 5;
            var input2Resposta = Fixture.Create<string>();
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
        public void o_método_HasRead_deve_chamar_o_mesmo_método_de_todos_as_instâncias_adicionados_caso_todas_sejam_false()
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
        public void o_método_HasRead_deve_ir_chamando_o_mesmo_método_de_todos_as_instâncias_adicionados_até_retornar_true()
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
    }
}