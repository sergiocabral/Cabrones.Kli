﻿using System;
using System.IO;
using System.Linq;
using Cabrones.Test;
using FluentAssertions;
using Kli.Infrastructure;
using Kli.Input;
using Kli.Output;
using NSubstitute;
using Xunit;

namespace Kli.Core
{
    public class TestLoaderAssembly
    {
        [Theory]
        [InlineData(typeof(IOutput), "Kli.Output.Console.dll")]
        [InlineData(typeof(IInput), "Kli.Input.Console.dll")]
        public void não_deve_registrar_interfaces_conhecidas_por_múltiplas_implementações(Type tipoDaInterface,
            string arquivoDoMóduloQueTemEssaInterface)
        {
            // Arrange, Given

            var dependencyResolver = Program.DependencyResolver.GetInstance<IDependencyResolver>();
            var loaderAssembly = Program.DependencyResolver.GetInstance<ILoaderAssembly>();

            // Act, When

            var interfacesCarregadas = loaderAssembly.RegisterServices(arquivoDoMóduloQueTemEssaInterface);
            Action resolverServiço = () => dependencyResolver.GetInstance(tipoDaInterface);

            // Assert, Then

            interfacesCarregadas.Should().NotContain(tipoDaInterface);
            resolverServiço.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void a_busca_por_arquivos_deve_ser_no_mesmo_diretório_que_o_executável()
        {
            // Arrange, Given

            var definition = Substitute.For<IDefinition>();
            var loaderAssembly = new LoaderAssembly(definition, Substitute.For<IDependencyResolver>());
            var leituraDePropriedade = 0;
            definition.DirectoryOfProgram.Returns(info =>
            {
                leituraDePropriedade++;
                return Environment.GetEnvironmentVariable("TEMP");
            });
            const string fileMaskParaNenhumArquivo = "no.found";

            // Act, When

            loaderAssembly.Load(fileMaskParaNenhumArquivo);

            // Assert, Then

            leituraDePropriedade.Should().Be(1);
        }

        [Fact]
        public void ao_carregar_assemblies_informando_a_máscara_inválida_para_o_arquivo_deve_retorna_lista_vazia()
        {
            // Arrange, Given

            var loaderAssembly = Program.DependencyResolver.GetInstance<ILoaderAssembly>();

            // Act, When

            var carregados = loaderAssembly.Load("máscara inválida");


            // Assert, Then

            carregados.Should().HaveCount(0);
        }

        [Fact]
        public void ao_carregar_assemblies_inválidos_deve_indicar_isso()
        {
            // Arrange, Given

            var loaderAssembly = new LoaderAssembly(
                Program.DependencyResolver.GetInstance<IDefinition>(),
                Substitute.For<IDependencyResolver>());

            // Act, When

            var carregados = loaderAssembly.Load("*.pdb").Count(a => a.Value == null);


            // Assert, Then

            carregados.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ao_obter_instâncias_informando_a_máscara_inválida_para_o_arquivo_deve_retorna_lista_vazia()
        {
            // Arrange, Given

            var loaderAssembly = Program.DependencyResolver.GetInstance<ILoaderAssembly>();

            // Act, When

            var instâncias = loaderAssembly.GetInstances<IOutput>("máscara\\inválida");

            // Assert, Then

            instâncias.Should().HaveCount(0);
        }

        [Fact]
        public void ao_obter_instâncias_informando_arquivo_existente_porém_inválido_deve_retorna_lista_vazia()
        {
            // Arrange, Given

            var definition = Program.DependencyResolver.GetInstance<IDefinition>();
            var loaderAssembly =
                new LoaderAssembly(definition, Program.DependencyResolver.GetInstance<IDependencyResolver>());
            var arquivo = new FileInfo(Path.Combine(definition.DirectoryOfProgram, "Kli.pdb"));

            // Act, When

            var instâncias = loaderAssembly.GetInstances<IOutput>(arquivo.Name);

            // Assert, Then

            arquivo.Exists.Should().BeTrue();
            instâncias.Should().HaveCount(0);
        }

        [Fact]
        public void ao_registrar_serviços_informando_a_máscara_inválida_para_o_arquivo_deve_retorna_lista_vazia()
        {
            // Arrange, Given

            var loaderAssembly = Program.DependencyResolver.GetInstance<ILoaderAssembly>();

            // Act, When

            var registrados = loaderAssembly.RegisterServices("máscara\\inválida");


            // Assert, Then

            registrados.Should().HaveCount(0);
        }

        [Fact]
        public void ao_registrar_serviços_informando_arquivo_existente_porém_inválido_deve_retorna_lista_vazia()
        {
            // Arrange, Given

            var definition = Program.DependencyResolver.GetInstance<IDefinition>();
            var loaderAssembly =
                new LoaderAssembly(definition, Program.DependencyResolver.GetInstance<IDependencyResolver>());
            var arquivo = new FileInfo(Path.Combine(definition.DirectoryOfProgram, "Kli.pdb"));

            // Act, When

            var registrados = loaderAssembly.RegisterServices(arquivo.Name);

            // Assert, Then

            arquivo.Exists.Should().BeTrue();
            registrados.Should().HaveCount(0);
        }

        [Fact]
        public void deve_ser_capaz_de_carregar_assemblies_de_um_ou_mais_arquivos()
        {
            // Arrange, Given

            var loaderAssembly = new LoaderAssembly(Program.DependencyResolver.GetInstance<IDefinition>(),
                Substitute.For<IDependencyResolver>());

            // Act, When

            var carregados = loaderAssembly.Load("*.dll").Count(a => a.Value != null);


            // Assert, Then

            carregados.Should().BeGreaterThan(1);
        }

        [Fact]
        public void deve_ser_capaz_de_crias_as_instâncias_dos_tipos_dos_assemblies_de_um_ou_mais_arquivos()
        {
            // Arrange, Given

            var loaderAssembly = Program.DependencyResolver.GetInstance<ILoaderAssembly>();

            // Act, When

            Action obterInstâncias = () => loaderAssembly.GetInstances<IOutput>("Kli.Output.Console.dll");

            // Assert, Then

            obterInstâncias.Should().NotThrow();
        }

        [Fact]
        public void deve_ser_capaz_de_registrar_os_serviços_carregados_no_resolvedor_de_dependências()
        {
            // Arrange, Given

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            var loaderAssembly =
                new LoaderAssembly(Program.DependencyResolver.GetInstance<IDefinition>(), dependencyResolver);

            // Act, When

            loaderAssembly.RegisterServices("Kli.dll");

            // Assert, Then

            dependencyResolver.ReceivedCalls().Count(a => a.GetMethodInfo().Name == "Register")
                // Este valor muda com frequência. A cada nova classe neste projeto.
                // O número é mantido exato ao invés de BeGreaterThan por conta do MutationTest. 
                .Should().Be(21);
        }

        [Fact]
        public void os_serviços_registrados_no_resolvedor_de_dependências_devem_ter_ciclo_de_vida_por_escopo()
        {
            // Arrange, Given

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            var loaderAssembly =
                new LoaderAssembly(Program.DependencyResolver.GetInstance<IDefinition>(), dependencyResolver);

            // Act, When

            loaderAssembly.RegisterServices("Kli.dll");
            var escopos = dependencyResolver.ReceivedCalls().Where(a => a.GetMethodInfo().Name == "Register")
                .Select(a => (DependencyResolverLifeTime) a.GetArguments()[2]).Distinct().ToList();

            // Assert, Then

            escopos.Should().HaveCount(1);
            escopos.Should().Contain(DependencyResolverLifeTime.PerScope);
        }

        [Fact]
        public void verifica_se_a_máscara_do_arquivo_está_capturando_os_arquivos_corretamente()
        {
            // Arrange, Given

            var loaderAssembly = new LoaderAssembly(
                Program.DependencyResolver.GetInstance<IDefinition>(),
                Substitute.For<IDependencyResolver>());

            // Act, When

            var carregados = loaderAssembly.Load("K*.dll").Where(a => a.Value != null)
                .Select(a => new FileInfo(a.Key).Name).OrderBy(a => a).ToList();


            // Assert, Then

            carregados.Should().HaveCount(1);
            carregados[0].Should().Be("Kli.dll");
        }

        [Fact]
        public void verifica_se_está_registrando_os_serviços()
        {
            // Arrange, Given

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            var loaderAssembly =
                new LoaderAssembly(Program.DependencyResolver.GetInstance<IDefinition>(), dependencyResolver);
            dependencyResolver.InterfacesForMultipleImplementation.Returns(info =>
                Program.DependencyResolver.GetInstance<IDependencyResolver>().InterfacesForMultipleImplementation);

            // Act, When

            loaderAssembly.RegisterServices("Kli.dll");

            // Assert, Then

            dependencyResolver
                .ReceivedCalls()
                .Count(a => a.GetMethodInfo().Name == "Register")
                .Should().BeGreaterThan(1);
        }

        [Fact]
        public void verifica_se_o_total_de_serviços_com_instâncias_criadas_está_de_acordo_com_o_esperado()
        {
            // Arrange, Given

            var loaderAssembly = Program.DependencyResolver.GetInstance<ILoaderAssembly>();

            // Act, When

            var instâncias = loaderAssembly.GetInstances<IEngine>("Kli.dll");

            // Assert, Then

            instâncias.Should().HaveCount(1);
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(LoaderAssembly);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(ILoaderAssembly));
            sut.AssertMyOwnImplementations(
                typeof(ILoaderAssembly));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}