using System;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Output
{
    public class TestMultipleOutput: BaseForTest
    {
        [Theory]
        [InlineData(typeof(MultipleOutput), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(MultipleOutput), typeof(Multiple<IOutput>), typeof(IMultiple<IOutput>), typeof(IMultipleOutput), typeof(IOutput))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            TestTypeImplementations(tipoDaClasse, tiposQueDeveSerImplementado);
        
        [Fact]
        public void o_método_Write_deve_chamar_o_mesmo_método_de_todos_as_instâncias_adicionados()
        {
            // Arrange, Given

            var multipleOutput = new MultipleOutput() as IMultipleOutput;

            var totalDeInstâncias = Fixture.Create<int>();
            for (var i = 0; i < totalDeInstâncias; i++) 
                multipleOutput.Add(Substitute.For<IOutput>());

            // Act, When

            var qualquerTexto = Fixture.Create<string>();
            multipleOutput.Write(qualquerTexto);
            
            // Assert, Then

            foreach (var instance in multipleOutput.Instances)
                instance.Received(1).Write(qualquerTexto);
        }
        
        [Fact]
        public void o_método_WriteLine_deve_chamar_o_mesmo_método_de_todos_as_instâncias_adicionados()
        {
            // Arrange, Given

            var multipleOutput = new MultipleOutput() as IMultipleOutput;

            var totalDeInstâncias = Fixture.Create<int>();
            for (var i = 0; i < totalDeInstâncias; i++) 
                multipleOutput.Add(Substitute.For<IOutput>());

            // Act, When

            var qualquerTexto = Fixture.Create<string>();
            multipleOutput.WriteLine(qualquerTexto);
            
            // Assert, Then

            foreach (var instance in multipleOutput.Instances)
                instance.Received(1).WriteLine(qualquerTexto);
        }
        
        [Fact]
        public void os_métodos_Write_e_WriteLine_devem_retornar_uma_auto_referência()
        {
            // Arrange, Given

            var multipleOutput = new MultipleOutput() as IMultipleOutput;
            
            // Act, When

            var retornoWrite = multipleOutput.Write(string.Empty);
            var retornoWriteLine = multipleOutput.WriteLine(string.Empty);
            
            // Assert, Then

            retornoWrite.Should().BeSameAs(retornoWriteLine);
            retornoWriteLine.Should().BeSameAs(multipleOutput);
        }
    }
}