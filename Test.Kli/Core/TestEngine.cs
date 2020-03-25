using System;
using FluentAssertions;
using Kli.i18n;
using Kli.Input;
using Kli.Module;
using Kli.Output;
using Kli.Wrappers;
using NSubstitute;
using Cabrones.Test;
using Xunit;

namespace Kli.Core
{
    public class TestEngine
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(Engine);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IEngine));
            sut.AssertMyOwnImplementations(
                typeof(IEngine));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
        
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

            var translate = Program.DependencyResolver.GetInstance<ITranslate>();
            var engine = new Engine(
                Substitute.For<IConsole>(),
                translate,
                Substitute.For<ILoaderAssembly>(),
                Substitute.For<IMultipleInput>(),
                Substitute.For<IMultipleOutput>(),
                Substitute.For<IMultipleModule>()
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
        public void método_principal_Initialize_deve_carregar_items_dos_IMultiple()
        {
            // Arrange, Given

            var loaderAssembly = Substitute.For<ILoaderAssembly>();
            var multipleInput = Substitute.For<IMultipleInput>();
            var multipleOutput = Substitute.For<IMultipleOutput>();
            var multipleModule = Substitute.For<IMultipleModule>();
            var engine = new Engine(
                Substitute.For<IConsole>(), 
                Substitute.For<ITranslate>(), 
                loaderAssembly,
                multipleInput,
                multipleOutput,
                multipleModule) as IEngine;

            var listaDeIInput = new[] { Substitute.For<IInput>() };
            loaderAssembly.GetInstances<IInput>(null).ReturnsForAnyArgs(listaDeIInput);
            
            var listaDeIOutput = new[] { Substitute.For<IOutput>() };
            loaderAssembly.GetInstances<IOutput>(null).ReturnsForAnyArgs(listaDeIOutput);
            
            var listaDeIModule = new[] { Substitute.For<IModule>() };
            loaderAssembly.GetInstances<IModule>(null).ReturnsForAnyArgs(listaDeIModule);
                
            // Act, When
            
            engine.Initialize();
                
            // Assert, Then

            foreach (var input in listaDeIInput) multipleInput.Received(1).Add(input);
            foreach (var output in listaDeIOutput) multipleOutput.Received(1).Add(output);
            foreach (var module in listaDeIModule) multipleModule.Received(1).Add(module);
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