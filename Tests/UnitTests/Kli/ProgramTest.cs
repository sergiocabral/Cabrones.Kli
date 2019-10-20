using System;
using System.Reflection;
using FluentAssertions;
using Kli.Core;
using Kli.Infrastructure;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli
{
    public class ProgramTest: Test, IDisposable
    {

        /// <summary>
        /// Backup para o resolvedor de dependências padrão.
        /// </summary>
        private readonly IDependencyResolver _backupForDependencyResolver;
        
        /// <summary>
        /// Construtor. Test SetUp.
        /// </summary>
        public ProgramTest()
        {
            _backupForDependencyResolver = DependencyResolver.Default;
            DependencyResolver.Default = Substitute.For<IDependencyResolver>();
        }

        /// <summary>
        /// Test Cleanup.
        /// </summary>
        public void Dispose()
        {
            DependencyResolver.Default = _backupForDependencyResolver;
        }
        
        [Fact]
        public void sendo_um_programa_console_deve_existir_o_metodo_estático_main()
        {
            // Arrange, Given
            
            var tipoDaClassQueContemOMétodoMain = typeof(global::Kli.Program);

            // Act, When
            
            var métodoMain = tipoDaClassQueContemOMétodoMain.GetMethod("Main", BindingFlags.Static | BindingFlags.Public);

            // Assert, Then
            
            métodoMain.Should().NotBeNull();
        }

        [Fact]
        public void verifica_se_o_programa_chama_a_classe_com_a_lógica_principal()
        {
            // Arrange, Given

            var dependencyResolver = DependencyResolver.Default;

            // Act, When
            
            global::Kli.Program.Main();

            // Assert, Then

            dependencyResolver.Received(1).GetInstance<IEngine>();
        }

    }
}