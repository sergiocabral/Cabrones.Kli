using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Kli.Module;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Core
{
    public class TestMultiple: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Multiple<IModule>), 2)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Multiple<IModule>), typeof(IMultiple<IModule>))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);
        
        [Fact]
        public void deve_ser_capaz_de_adicionar_novos_serviços()
        {
            // Arrange, Given

            var multiple = new MultipleModule(Substitute.For<IInteraction>()) as IMultiple<IModule>;

            // Act, When

            Action adicionarNovosServiços = () =>
            {
                multiple.Add(Substitute.For<IModule>());
                multiple.Add(Substitute.For<IModule>());
                multiple.Add(Substitute.For<IModule>());
            };

            // Assert, Then

            adicionarNovosServiços.Should().NotThrow();
        }
        
        [Fact]
        public void ao_adicionar_um_novo_serviços_deve_atualizar_a_lista()
        {
            // Arrange, Given

            var multiple = new MultipleModule(Substitute.For<IInteraction>()) as IMultiple<IModule>;
            var quantasAdições = Fixture.Create<int>();

            // Act, When

            for (var i = 0; i < quantasAdições; i++) multiple.Add(Substitute.For<IModule>());

            // Assert, Then

            multiple.Instances.Count.Should().Be(quantasAdições);
        }
        
        [Fact]
        public void a_lista_de_serviços_não_deve_ser_a_lista_original()
        {
            // Arrange, Given

            var multiple = new MultipleModule(Substitute.For<IInteraction>()) as IMultiple<IModule>;
            var quantasAdições = Fixture.Create<int>();
            for (var i = 0; i < quantasAdições; i++) multiple.Add(Substitute.For<IModule>());

            // Act, When

            multiple.Instances.Clear();

            // Assert, Then

            multiple.Instances.Count.Should().Be(quantasAdições);
        }
        
        [Fact]
        public void ao_adicionar_um_serviço_duas_vezes_não_deve_duplicar_na_lista()
        {
            // Arrange, Given

            var multiple = new MultipleModule(Substitute.For<IInteraction>()) as IMultiple<IModule>;
            var instância = Substitute.For<IModule>();
            
            // Act, When

            multiple.Add(instância);
            multiple.Add(instância);
            multiple.Add(instância);
            multiple.Add(instância);

            // Assert, Then

            multiple.Instances.Should().HaveCount(1);
            multiple.Instances.Single().Should().BeSameAs(instância);
        }
    }
}