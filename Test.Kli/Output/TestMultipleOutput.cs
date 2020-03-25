using System;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using NSubstitute;
using Cabrones.Test;
using Xunit;

namespace Kli.Output
{
    public class TestMultipleOutput
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(MultipleOutput);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(Multiple<IOutput>),
                typeof(IMultipleOutput),
                typeof(IMultiple<IOutput>),
                typeof(IOutput));
            sut.AssertMyOwnImplementations(
                typeof(Multiple<IOutput>),
                typeof(IMultipleOutput));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
        
        [Fact]
        public void o_método_Write_deve_chamar_o_mesmo_método_de_todos_as_instâncias_adicionados()
        {
            // Arrange, Given

            var multipleOutput = new MultipleOutput() as IMultipleOutput;

            var totalDeInstâncias = this.Fixture<int>();
            for (var i = 0; i < totalDeInstâncias; i++) 
                multipleOutput.Add(Substitute.For<IOutput>());

            // Act, When

            var qualquerTexto = this.Fixture<string>();
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

            var totalDeInstâncias = this.Fixture<int>();
            for (var i = 0; i < totalDeInstâncias; i++) 
                multipleOutput.Add(Substitute.For<IOutput>());

            // Act, When

            var qualquerTexto = this.Fixture<string>();
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