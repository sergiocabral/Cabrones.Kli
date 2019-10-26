using System;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using Kli.i18n;
using Kli.Output;
using Kli.Wrappers;
using Test;
using Xunit;

namespace Kli.Infrastructure
{
    public class TestDependencyResolver: BaseForTest
    {
        [Theory]
        [InlineData(typeof(DependencyResolver), 7)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(DependencyResolver), typeof(IDependencyResolver))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);

        [Theory]
        [InlineData(typeof(IOutputWriter))]
        [InlineData(typeof(IOutputMarkers))]
        [InlineData(typeof(IEngine))]
        [InlineData(typeof(ICache))]
        [InlineData(typeof(ILanguage))]
        [InlineData(typeof(ITranslate))]
        [InlineData(typeof(IConsole))]
        [InlineData(typeof(IEnvironment))]
        [InlineData(typeof(IDefinition))]
        [InlineData(typeof(IDependencyResolver))]
        public void verifica_se_o_serviço_está_sendo_resolvido(Type tipoDoServiço)
        {
            // Arrange, Given

            var dependencyResolver = DependencyResolverFromProgram;
            
            // Act, When
            
            var serviço = dependencyResolver.GetInstance(tipoDoServiço);

            // Assert, Then
            
            serviço.Should().NotBeNull();
        }
        
        [Fact]
        public void deve_ser_capaz_de_adicionar_serviços_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;

            // Act, When
            
            Action adicionar = () => dependencyResolver.Register(typeof(ITeste), typeof(Teste));
                
            // Assert, Then

            adicionar.Should().NotThrow();
        }
        
        [Fact]
        public void deve_ser_capaz_de_adicionar_serviços_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;

            // Act, When
            
            Action adicionar = () => dependencyResolver.Register<ITeste, Teste>();
                
            // Assert, Then

            adicionar.Should().NotThrow();
        }
        
        [Fact]
        public void deve_ser_capaz_de_obter_um_serviço_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register(typeof(ITeste), typeof(Teste));
            
            // Act, When

            var instância = (ITeste) dependencyResolver.GetInstance(typeof(ITeste));
                
            // Assert, Then

            instância.GetType().Should().Be(typeof(Teste));
        }
        
        [Fact]
        public void deve_ser_capaz_de_obter_um_serviço_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register<ITeste, Teste>();
            
            // Act, When

            var instância = dependencyResolver.GetInstance<ITeste>();
                
            // Assert, Then

            instância.GetType().Should().Be(typeof(Teste));
        }
        
        [Fact]
        public void verifica_se_está_sendo_respeitado_o_escopo_do_serviço_tipo_PerContainer_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            // ReSharper disable once RedundantArgumentDefaultValue
            dependencyResolver.Register(typeof(ITeste),typeof(Teste), DependencyResolverLifeTime.PerContainer);
            
            // Act, When

            var instância1 = (ITeste) dependencyResolver.GetInstance(typeof(ITeste));
            var instância2 = (ITeste) dependencyResolver.GetInstance(typeof(ITeste));
                
            // Assert, Then

            instância1.Should().BeSameAs(instância2);
            instância1.Identificador.Should().Be(instância2.Identificador);
        }
        
        [Fact]
        public void verifica_se_está_sendo_respeitado_o_escopo_do_serviço_tipo_PerContainer_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            // ReSharper disable once RedundantArgumentDefaultValue
            dependencyResolver.Register<ITeste, Teste>(DependencyResolverLifeTime.PerContainer);
            
            // Act, When

            var instância1 = dependencyResolver.GetInstance<ITeste>();
            var instância2 = dependencyResolver.GetInstance<ITeste>();
                
            // Assert, Then

            instância1.Should().BeSameAs(instância2);
            instância1.Identificador.Should().Be(instância2.Identificador);
        }
        
        [Fact]
        public void deve_ser_possível_criar_um_escopo()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            
            // Act, When

            Action criarEscopo = () => dependencyResolver.CreateScope();
                
            // Assert, Then

            criarEscopo.Should().NotThrow();
        }
        
        [Fact]
        public void deve_ser_possível_apagar_um_escopo_criado()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var escopo = dependencyResolver.CreateScope();
            
            // Act, When

            Action removerEscopo = () => dependencyResolver.DisposeScope(escopo);
                
            // Assert, Then

            removerEscopo.Should().NotThrow();
        }
        
        [Fact]
        public void dispara_exception_ao_apagar_um_escopo_não_existente()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var escopoQueNãoExiste = Fixture.Create<Guid>();
            
            // Act, When

            Action removerEscopoQueNãoExiste = () => dependencyResolver.DisposeScope(escopoQueNãoExiste);
                
            // Assert, Then

            removerEscopoQueNãoExiste.Should().Throw<ObjectDisposedException>();
        }
        
        [Fact]
        public void verifica_se_está_sendo_respeitado_o_escopo_do_serviço_tipo_PerScope_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register(typeof(ITeste), typeof(Teste),DependencyResolverLifeTime.PerScope);
            
            // Act, When

            var escopo1 = dependencyResolver.CreateScope();
            var instância1A = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopo1);
            var instância1B = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopo1);
            var escopo2 = dependencyResolver.CreateScope();
            var instância2A = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopo2);
            var instância2B = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopo2);
                
            // Assert, Then

            instância1A.Should().BeSameAs(instância1B);
            instância1A.Identificador.Should().Be(instância1B.Identificador);
            
            instância2A.Should().BeSameAs(instância2B);
            instância2A.Identificador.Should().Be(instância2B.Identificador);
            
            instância1A.Should().NotBeSameAs(instância2A);
            instância1A.Identificador.Should().NotBe(instância2A.Identificador);
        }
        
        [Fact]
        public void verifica_se_está_sendo_respeitado_o_escopo_do_serviço_tipo_PerScope_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register<ITeste, Teste>(DependencyResolverLifeTime.PerScope);
            
            // Act, When

            var escopo1 = dependencyResolver.CreateScope();
            var instância1A = dependencyResolver.GetInstance<ITeste>(escopo1);
            var instância1B = dependencyResolver.GetInstance<ITeste>(escopo1);
            var escopo2 = dependencyResolver.CreateScope();
            var instância2A = dependencyResolver.GetInstance<ITeste>(escopo2);
            var instância2B = dependencyResolver.GetInstance<ITeste>(escopo2);
                
            // Assert, Then

            instância1A.Should().BeSameAs(instância1B);
            instância1A.Identificador.Should().Be(instância1B.Identificador);
            
            instância2A.Should().BeSameAs(instância2B);
            instância2A.Identificador.Should().Be(instância2B.Identificador);
            
            instância1A.Should().NotBeSameAs(instância2A);
            instância1A.Identificador.Should().NotBe(instância2A.Identificador);
        }
        
        [Fact]
        public void dispara_exception_ao_obter_um_serviço_via_Type_como_parâmetro_de_um_escopo_não_existente()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var escopoQueNãoExiste = Fixture.Create<Guid>();
            dependencyResolver.Register(typeof(ITeste), typeof(Teste), DependencyResolverLifeTime.PerScope);
            
            // Act, When

            Action obterServiçoDeEscopoQueNãoExiste =
                () => dependencyResolver.GetInstance(typeof(ITeste), escopoQueNãoExiste);
                
            // Assert, Then

            obterServiçoDeEscopoQueNãoExiste.Should().Throw<ObjectDisposedException>();
        }
        
        [Fact]
        public void dispara_exception_ao_obter_um_serviço_via_Type_como_Generic_de_um_escopo_não_existente()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var escopoQueNãoExiste = Fixture.Create<Guid>();
            dependencyResolver.Register<ITeste, Teste>(DependencyResolverLifeTime.PerScope);
            
            // Act, When

            Action obterServiçoDeEscopoQueNãoExiste =
                () => dependencyResolver.GetInstance<ITeste>(escopoQueNãoExiste);
                
            // Assert, Then

            obterServiçoDeEscopoQueNãoExiste.Should().Throw<ObjectDisposedException>();
        }
        
        [Fact]
        public void permite_criar_escopo_dentro_de_outro_escopo()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            
            // Act, When

            Action criarEscoposAninhados = () => 
                dependencyResolver.CreateScope(
                    dependencyResolver.CreateScope(
                        dependencyResolver.CreateScope()));
                
            // Assert, Then

            criarEscoposAninhados.Should().NotThrow();
        }
        
        [Fact]
        public void deve_ser_capaz_de_verificar_se_um_escopo_está_ativo_ou_já_foi_liberado()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var escopoInvalido = Guid.NewGuid();
            var escopoLiberado = dependencyResolver.CreateScope();
            dependencyResolver.DisposeScope(escopoLiberado);
            var escopoAtivo = dependencyResolver.CreateScope();
            
            // Act, When

            var escopoInvalidoEstáAtivo = dependencyResolver.IsActive(escopoInvalido);
            var escopoLiberadoEstáAtivo = dependencyResolver.IsActive(escopoLiberado);
            var escopoAtivoEstáAtivo = dependencyResolver.IsActive(escopoAtivo);
                
            // Assert, Then

            escopoInvalidoEstáAtivo.Should().BeFalse();
            escopoLiberadoEstáAtivo.Should().BeFalse();
            escopoAtivoEstáAtivo.Should().BeTrue();
        }
        
        [Fact]
        public void liberar_um_escopo_pai_deve_liberar_todos_os_escopos_filhos()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var escopoPai = dependencyResolver.CreateScope();
            var escopoFilho = dependencyResolver.CreateScope(escopoPai);
            var escopoNeto = dependencyResolver.CreateScope(escopoFilho);
            
            // Act, When

            dependencyResolver.DisposeScope(escopoPai);
            var escopoPaiFoiLiberado = !dependencyResolver.IsActive(escopoPai);
            var escopoFilhoFoiLiberado = !dependencyResolver.IsActive(escopoFilho);
            var escopoNetoFoiLiberado = !dependencyResolver.IsActive(escopoNeto);

            // Assert, Then

            escopoPaiFoiLiberado.Should().BeTrue();
            escopoFilhoFoiLiberado.Should().BeTrue();
            escopoNetoFoiLiberado.Should().BeTrue();
        }
        
        [Fact]
        public void verifica_se_a_liberação_do_escopo_libera_as_instâncias_associadas_quando_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register(typeof(ITeste), typeof(Teste),DependencyResolverLifeTime.PerScope);
            var escopo = dependencyResolver.CreateScope();
            
            var liberadoQuantasVezes = 0;
            var instância = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopo);
            instância.Disposed += () => liberadoQuantasVezes++;

            // Act, When
            
            dependencyResolver.DisposeScope(escopo);
                
            // Assert, Then

            liberadoQuantasVezes.Should().Be(1);
        }
        
        [Fact]
        public void verifica_se_a_liberação_do_escopo_libera_as_instâncias_associadas_quando_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register<ITeste, Teste>(DependencyResolverLifeTime.PerScope);
            var escopo = dependencyResolver.CreateScope();
            
            var liberadoQuantasVezes = 0;
            var instância = dependencyResolver.GetInstance<ITeste>(escopo);
            instância.Disposed += () => liberadoQuantasVezes++;

            // Act, When
            
            dependencyResolver.DisposeScope(escopo);
                
            // Assert, Then

            liberadoQuantasVezes.Should().Be(1);
        }
        
        [Fact]
        public void verifica_se_a_liberação_do_escopo_libera_as_instâncias_associadas_em_escopos_filhos_quando_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register(typeof(ITeste), typeof(Teste),DependencyResolverLifeTime.PerScope);
            var escopoPai = dependencyResolver.CreateScope();
            var escopoFilho = dependencyResolver.CreateScope(escopoPai);
            var escopoNeto = dependencyResolver.CreateScope(escopoFilho);
            
            var liberadoQuantasVezes = 0;
            
            var instânciaNoPai = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopoPai);
            var instânciaNoFilho = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopoFilho);
            var instânciaNoNeto = (ITeste) dependencyResolver.GetInstance(typeof(ITeste), escopoNeto);
            
            instânciaNoPai.Disposed += () => liberadoQuantasVezes++;
            instânciaNoFilho.Disposed += () => liberadoQuantasVezes++;
            instânciaNoNeto.Disposed += () => liberadoQuantasVezes++;

            // Act, When
            
            dependencyResolver.DisposeScope(escopoPai);
                
            // Assert, Then

            liberadoQuantasVezes.Should().Be(3);
        }
        
        [Fact]
        public void verifica_se_a_liberação_do_escopo_libera_as_instâncias_associadas_em_escopos_filhos_quando_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register<ITeste, Teste>(DependencyResolverLifeTime.PerScope);
            var escopoPai = dependencyResolver.CreateScope();
            var escopoFilho = dependencyResolver.CreateScope(escopoPai);
            var escopoNeto = dependencyResolver.CreateScope(escopoFilho);
            
            var liberadoQuantasVezes = 0;
            
            var instânciaNoPai = dependencyResolver.GetInstance<ITeste>(escopoPai);
            var instânciaNoFilho = dependencyResolver.GetInstance<ITeste>(escopoFilho);
            var instânciaNoNeto = dependencyResolver.GetInstance<ITeste>(escopoNeto);
            
            instânciaNoPai.Disposed += () => liberadoQuantasVezes++;
            instânciaNoFilho.Disposed += () => liberadoQuantasVezes++;
            instânciaNoNeto.Disposed += () => liberadoQuantasVezes++;

            // Act, When
            
            dependencyResolver.DisposeScope(escopoPai);
                
            // Assert, Then

            liberadoQuantasVezes.Should().Be(3);
        }
    }

    /// <summary>
    /// Interface usada como insumo para testar o DependencyResolver.
    /// </summary>
    internal interface ITeste: IDisposable
    {
        /// <summary>
        /// Identificador da instância.
        /// </summary>
        string Identificador { get; }

        /// <summary>
        /// Evento disparado quando Dispose é chamado.
        /// </summary>
        event Action Disposed;
    }

    /// <summary>
    /// Classe usada como insumo para testar o DependencyResolver.
    /// </summary>
    internal class Teste: BaseForTest, ITeste
    {
        /// <summary>
        /// Construtor. Define o identificador.
        /// </summary>
        public Teste() => Identificador = Fixture.Create<string>();
        
        /// <summary>
        /// Identificador da instância.
        /// </summary>
        public string Identificador { get; }

        /// <summary>
        /// Evento disparado quando Dispose é chamado.
        /// </summary>
        public event Action Disposed;

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() => Disposed?.Invoke();
    }
}