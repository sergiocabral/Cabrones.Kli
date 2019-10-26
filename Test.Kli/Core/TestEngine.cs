﻿using System;
using FluentAssertions;
using Kli.i18n;
using Kli.Input;
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
        public void método_principal_Run_deve_rodar_sem_erros()
        {
            // Arrange, Given

            var engine = DependencyResolverFromProgram.GetInstance<IEngine>();
            
            // Act, When
            
            Action run = () => engine.Run();

            // Assert, Then
            
            run.Should().NotThrow();
        }
        
        [Fact]
        public void método_principal_Run_deve_fazer_reset_nas_cores_do_console_no_início_e_no_final()
        {
            // Arrange, Given

            var console = DependencyResolverForTest.GetInstance<IConsole>();
            var engine = new Engine(console, DependencyResolverForTest.GetInstance<ITranslate>(), DependencyResolverForTest.GetInstance<ILoaderAssembly>()) as IEngine;
            
            // Act, When
            
            engine.Run();

            // Assert, Then
            
            console.Received(2).ResetColor();
        }
        
        [Fact]
        public void método_principal_Run_deve_carregar_as_traduções_do_recurso_embutido()
        {
            // Arrange, Given

            var translate = DependencyResolverForTest.GetInstance<ITranslate>();
            var engine = new Engine(DependencyResolverForTest.GetInstance<IConsole>(), translate, DependencyResolverForTest.GetInstance<ILoaderAssembly>()) as IEngine;
            
            // Act, When
            
            engine.Run();

            // Assert, Then

            translate.ReceivedWithAnyArgs().LoadFromResource(null, null);
        }
        
        [Fact]
        public void após_chamada_do_método_principal_Run_as_traduções_devem_estar_funcionando()
        {
            // Arrange, Given

            var engine = DependencyResolverFromProgram.GetInstance<IEngine>();
            engine.Run();
            
            // Act, When

            var traduçãoDeYes = "Yes".Translate("pt");
            
            // Assert, Then

            traduçãoDeYes.Should().Be("Sim");
        }
        
        [Fact]
        public void método_principal_Run_deve_carregar_em_tempo_de_execução_os_arquivos_de_assemblies()
        {
            // Arrange, Given

            var assemblyFileLoader = DependencyResolverForTest.GetInstance<ILoaderAssembly>();
            var engine = new Engine(DependencyResolverForTest.GetInstance<IConsole>(), DependencyResolverForTest.GetInstance<ITranslate>(), assemblyFileLoader) as IEngine;

            // Act, When
            
            engine.Run();
                
            // Assert, Then

            assemblyFileLoader.Received(1).GetInstances<IOutput>("Kli.Output.*.dll");
            assemblyFileLoader.Received(1).GetInstances<IInput>("Kli.Input.*.dll");
        }
    }
}