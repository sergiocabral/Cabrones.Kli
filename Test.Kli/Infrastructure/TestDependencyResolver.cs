using System;
using AutoFixture;
using FluentAssertions;
using Kli.Core;
using Kli.i18n;
using Kli.Input;
using Kli.Module;
using Kli.Output;
using Kli.Wrappers;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Infrastructure
{
    public class TestDependencyResolver: BaseForTest
    {
        [Theory]
        [InlineData(typeof(DependencyResolver), 8)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(DependencyResolver), typeof(IDependencyResolver))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            TestTypeImplementations(tipoDaClasse, tiposQueDeveSerImplementado);

        [Theory]
        [InlineData(typeof(IOutputWriter))]
        [InlineData(typeof(IOutputMarkers))]
        [InlineData(typeof(IEngine))]
        [InlineData(typeof(IInteraction))]
        [InlineData(typeof(IMultipleInput))]
        [InlineData(typeof(IMultiple<IInput>))]
        [InlineData(typeof(Multiple<IInput>))]
        [InlineData(typeof(IMultipleOutput))]
        [InlineData(typeof(IMultiple<IOutput>))]
        [InlineData(typeof(Multiple<IOutput>))]
        [InlineData(typeof(IMultipleModule))]
        [InlineData(typeof(IMultiple<IModule>))]
        [InlineData(typeof(Multiple<IModule>))]
        [InlineData(typeof(ICache))]
        [InlineData(typeof(ILanguage))]
        [InlineData(typeof(ITranslate))]
        [InlineData(typeof(ILoaderAssembly))]
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
            
            Action adicionar = () => dependencyResolver.Register(typeof(SimulationToTestDependencyResolver), typeof(SimulationToTestDependencyResolver));
                
            // Assert, Then

            adicionar.Should().NotThrow();
        }
        
        [Fact]
        public void deve_ser_capaz_de_adicionar_serviços_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;

            // Act, When
            
            Action adicionar = () => dependencyResolver.Register<SimulationToTestDependencyResolver, SimulationToTestDependencyResolver>();
                
            // Assert, Then

            adicionar.Should().NotThrow();
        }
        
        [Fact]
        public void deve_ser_capaz_de_obter_um_serviço_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register(typeof(SimulationToTestDependencyResolver), typeof(SimulationToTestDependencyResolver));
            
            // Act, When

            var instância = dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver));
                
            // Assert, Then

            instância.GetType().Should().Be(typeof(SimulationToTestDependencyResolver));
        }
        
        [Fact]
        public void deve_ser_capaz_de_obter_um_serviço_via_Type_como_Generic()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            dependencyResolver.Register<SimulationToTestDependencyResolver, SimulationToTestDependencyResolver>();
            
            // Act, When

            var instância = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>();
                
            // Assert, Then

            instância.GetType().Should().Be(typeof(SimulationToTestDependencyResolver));
        }
        
        [Fact]
        public void verifica_se_está_sendo_respeitado_o_escopo_do_serviço_tipo_PerContainer_via_Type_como_parâmetro()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            // ReSharper disable once RedundantArgumentDefaultValue
            dependencyResolver.Register(typeof(SimulationToTestDependencyResolver),typeof(SimulationToTestDependencyResolver), DependencyResolverLifeTime.PerContainer);
            
            // Act, When

            var instância1 = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver));
            var instância2 = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver));
                
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
            dependencyResolver.Register<SimulationToTestDependencyResolver, SimulationToTestDependencyResolver>(DependencyResolverLifeTime.PerContainer);
            
            // Act, When

            var instância1 = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>();
            var instância2 = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>();
                
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
            dependencyResolver.Register(typeof(SimulationToTestDependencyResolver), typeof(SimulationToTestDependencyResolver),DependencyResolverLifeTime.PerScope);
            
            // Act, When

            var escopo1 = dependencyResolver.CreateScope();
            var instância1A = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopo1);
            var instância1B = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopo1);
            var escopo2 = dependencyResolver.CreateScope();
            var instância2A = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopo2);
            var instância2B = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopo2);
                
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
            dependencyResolver.Register<SimulationToTestDependencyResolver, SimulationToTestDependencyResolver>(DependencyResolverLifeTime.PerScope);
            
            // Act, When

            var escopo1 = dependencyResolver.CreateScope();
            var instância1A = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopo1);
            var instância1B = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopo1);
            var escopo2 = dependencyResolver.CreateScope();
            var instância2A = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopo2);
            var instância2B = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopo2);
                
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
            dependencyResolver.Register(typeof(SimulationToTestDependencyResolver), typeof(SimulationToTestDependencyResolver), DependencyResolverLifeTime.PerScope);
            
            // Act, When

            Action obterServiçoDeEscopoQueNãoExiste =
                () => dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopoQueNãoExiste);
                
            // Assert, Then

            obterServiçoDeEscopoQueNãoExiste.Should().Throw<ObjectDisposedException>();
        }
        
        [Fact]
        public void dispara_exception_ao_obter_um_serviço_via_Type_como_Generic_de_um_escopo_não_existente()
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var escopoQueNãoExiste = Fixture.Create<Guid>();
            dependencyResolver.Register<SimulationToTestDependencyResolver, SimulationToTestDependencyResolver>(DependencyResolverLifeTime.PerScope);
            
            // Act, When

            Action obterServiçoDeEscopoQueNãoExiste =
                () => dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopoQueNãoExiste);
                
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
            dependencyResolver.Register(typeof(SimulationToTestDependencyResolver), typeof(SimulationToTestDependencyResolver),DependencyResolverLifeTime.PerScope);
            var escopo = dependencyResolver.CreateScope();
            
            var liberadoQuantasVezes = 0;
            var instância = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopo);
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
            dependencyResolver.Register<SimulationToTestDependencyResolver, SimulationToTestDependencyResolver>(DependencyResolverLifeTime.PerScope);
            var escopo = dependencyResolver.CreateScope();
            
            var liberadoQuantasVezes = 0;
            var instância = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopo);
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
            dependencyResolver.Register(typeof(SimulationToTestDependencyResolver), typeof(SimulationToTestDependencyResolver),DependencyResolverLifeTime.PerScope);
            var escopoPai = dependencyResolver.CreateScope();
            var escopoFilho = dependencyResolver.CreateScope(escopoPai);
            var escopoNeto = dependencyResolver.CreateScope(escopoFilho);
            
            var liberadoQuantasVezes = 0;
            
            var instânciaNoPai = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopoPai);
            var instânciaNoFilho = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopoFilho);
            var instânciaNoNeto = (SimulationToTestDependencyResolver) dependencyResolver.GetInstance(typeof(SimulationToTestDependencyResolver), escopoNeto);
            
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
            dependencyResolver.Register<SimulationToTestDependencyResolver, SimulationToTestDependencyResolver>(DependencyResolverLifeTime.PerScope);
            var escopoPai = dependencyResolver.CreateScope();
            var escopoFilho = dependencyResolver.CreateScope(escopoPai);
            var escopoNeto = dependencyResolver.CreateScope(escopoFilho);
            
            var liberadoQuantasVezes = 0;
            
            var instânciaNoPai = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopoPai);
            var instânciaNoFilho = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopoFilho);
            var instânciaNoNeto = dependencyResolver.GetInstance<SimulationToTestDependencyResolver>(escopoNeto);
            
            instânciaNoPai.Disposed += () => liberadoQuantasVezes++;
            instânciaNoFilho.Disposed += () => liberadoQuantasVezes++;
            instânciaNoNeto.Disposed += () => liberadoQuantasVezes++;

            // Act, When
            
            dependencyResolver.DisposeScope(escopoPai);
                
            // Assert, Then

            liberadoQuantasVezes.Should().Be(3);
        }
        
        [Theory]
        [InlineData(typeof(IInput))]
        [InlineData(typeof(IOutput))]
        [InlineData(typeof(IModule))]
        public void não_permite_registrar_serviços_de_interfaces_com_múltiplas_implementações(Type tipoDaInterfaces)
        {
            // Arrange, Given
            
            var dependencyResolver = new DependencyResolver() as IDependencyResolver;
            var exemploDeInstância = Substitute.For(new [] { tipoDaInterfaces }, new object[0]);
            var tipoDoExemploDeInstância = exemploDeInstância.GetType();
            
            // Act, When

            Action registrarInterfaceComoServiço =
                () => dependencyResolver.Register(tipoDaInterfaces, tipoDoExemploDeInstância);
                
            // Assert, Then

            registrarInterfaceComoServiço.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void verificar_se_lista_de_interfaces_com_múltiplas_implementações_está_correta()
        {
            // Arrange, Given
            
            var dependencyResolver = DependencyResolverFromProgram.GetInstance<IDependencyResolver>();
            
            // Act, When

            var interfacesComMúltiplasImplementações = dependencyResolver.InterfacesForMultipleImplementation;
                
            // Assert, Then

            interfacesComMúltiplasImplementações.Should().BeEquivalentTo(typeof(IOutput), typeof(IInput), typeof(IModule));
        }
    }
}