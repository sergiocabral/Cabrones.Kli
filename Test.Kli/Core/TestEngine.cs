using System;
using FluentAssertions;
using Kli.i18n;
using Kli.Input;
using Kli.Module;
using Kli.Output;
using Kli.Wrappers;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.Core
{
    public class TestEngine: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Engine), 1)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            verifica_se_o_total_de_métodos_públicos_declarados_está_correto_no_tipo(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Engine), typeof(IEngine))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            verifica_se_classe_implementa_o_tipo(tipoDaClasse, tiposQueDeveSerImplementado);
        
        [Fact]
        public void método_principal_Initialize_deve_fazer_reset_nas_cores_do_console_no_início_e_no_final()
        {
            // Arrange, Given

            var console = Substitute.For<IConsole>();
            var engine = new Engine(
                console, 
                Substitute.For<ITranslate>(), 
                Substitute.For<ILoaderAssembly>(),
                Substitute.For<IMultipleInput>(),
                Substitute.For<IMultipleOutput>(),
                Substitute.For<IMultipleModule>()) as IEngine;
            
            // Act, When
            
            engine.Initialize();

            // Assert, Then
            
            console.Received(2).ResetColor();
        }
        
        [Fact]
        public void método_principal_Initialize_deve_carregar_as_traduções_do_recurso_embutido()
        {
            // Arrange, Given

            var translate = Substitute.For<ITranslate>();
            var engine = new Engine(
                Substitute.For<IConsole>(), 
                translate, 
                Substitute.For<ILoaderAssembly>(),
                Substitute.For<IMultipleInput>(),
                Substitute.For<IMultipleOutput>(),
                Substitute.For<IMultipleModule>()) as IEngine;
            
            // Act, When
            
            engine.Initialize();

            // Assert, Then

            translate.ReceivedWithAnyArgs(1).LoadFromResource(null, null);
        }
        
        [Fact]
        public void após_chamada_do_método_principal_Initialize_as_traduções_devem_estar_funcionando()
        {
            // Arrange, Given

            var translate = DependencyResolverFromProgram.GetInstance<ITranslate>();
            var engine = new Engine(
                DependencyResolverForTest.GetInstance<IConsole>(),
                translate,
                DependencyResolverForTest.GetInstance<ILoaderAssembly>(),
                DependencyResolverForTest.GetInstance<IMultipleInput>(),
                DependencyResolverForTest.GetInstance<IMultipleOutput>(),
                DependencyResolverForTest.GetInstance<IMultipleModule>()
                ) as IEngine;
            engine.Initialize();
            
            // Act, When

            var traduçãoDeYes = translate.Get("Yes", "pt");
            
            // Assert, Then

            traduçãoDeYes.Should().Be("Sim");
        }
        
        [Fact]
        public void método_principal_Initialize_deve_carregar_em_tempo_de_execução_os_arquivos_de_assemblies()
        {
            // Arrange, Given

            var assemblyFileLoader = Substitute.For<ILoaderAssembly>();
            var engine = new Engine(
                Substitute.For<IConsole>(), 
                Substitute.For<ITranslate>(), 
                assemblyFileLoader,
                Substitute.For<IMultipleInput>(),
                Substitute.For<IMultipleOutput>(),
                Substitute.For<IMultipleModule>()) as IEngine;

            // Act, When
            
            engine.Initialize();
                
            // Assert, Then

            assemblyFileLoader.Received(1).GetInstances<IOutput>("Kli.Output.*.dll");
            assemblyFileLoader.Received(1).GetInstances<IInput>("Kli.Input.*.dll");
            assemblyFileLoader.Received(1).GetInstances<IModule>("Kli.Module.*.dll");
        }
        
        [Fact]
        public void método_principal_Initialize_deve_carregar_items_pelo_menos_no_IMultipleInput_e_IMultipleOutput()
        {
            // Arrange, Given

            var multipleInput = Substitute.For<IMultipleInput>();
            var multipleOutput = Substitute.For<IMultipleOutput>();
            var engine = new Engine(
                Substitute.For<IConsole>(), 
                Substitute.For<ITranslate>(), 
                DependencyResolverFromProgram.GetInstance<ILoaderAssembly>(),
                multipleInput,
                multipleOutput,
                Substitute.For<IMultipleModule>()) as IEngine;

            // Act, When
            
            engine.Initialize();
                
            // Assert, Then

            multipleInput.ReceivedWithAnyArgs().Add(null);
            multipleOutput.ReceivedWithAnyArgs().Add(null);
        }
        
        [Fact]
        public void método_principal_Initialize_deve_chamar_a_execução_dos_módulos()
        {
            // Arrange, Given

            var multipleModule = Substitute.For<IMultipleModule>();
            var engine = new Engine(
                Substitute.For<IConsole>(), 
                Substitute.For<ITranslate>(),
                Substitute.For<ILoaderAssembly>(),
                Substitute.For<IMultipleInput>(),
                Substitute.For<IMultipleOutput>(),
                multipleModule) as IEngine;
            
            // Act, When
            
            engine.Initialize();

            // Assert, Then

            multipleModule.Received(1).Run();
        }
    }
}