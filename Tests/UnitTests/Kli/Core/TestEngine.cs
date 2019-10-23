using System;
using FluentAssertions;
using Kli.Core;
using Kli.IO;
using NSubstitute;
using Xunit;

namespace Tests.UnitTests.Kli.Core
{
    public class TestEngine: Test
    {
        [Theory]
        [InlineData(typeof(Engine), typeof(IEngine))]
        public void verifica_se_classe_implementa_tipos(Type tipoDaClasse, Type tipoQueDeveSerImplementado)
        {
            verifica_se_classe_implementa_tipo(tipoDaClasse, tipoQueDeveSerImplementado);
        }
        
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

            var console = Substitute.For<IConsole>();
            var engine = new Engine(console);
            
            // Act, When
            
            engine.Run();

            // Assert, Then
            
            console.Received(2).ResetColor();
        }
    }
}