using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Test;
using Xunit;

namespace Kli.Core
{
    public class TestDefinition: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Definition), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Definition), typeof(IDefinition))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

        [Fact]
        public void verificar_se_valor_está_correto_para_propriedade_DirectoryOfProgram()
        {
            // Arrange, Given

            var definition = DependencyResolverFromProgram.GetInstance<IDefinition>();

            // Act, When

            var valorEsperado = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory?.FullName;
            var valorAtual = definition.DirectoryOfProgram;

            // Assert, Then

            valorAtual.Should().Be(valorEsperado);
        }
    }
}