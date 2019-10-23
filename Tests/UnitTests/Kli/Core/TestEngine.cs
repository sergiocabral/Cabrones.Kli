using System;
using FluentAssertions;
using Kli.Core;
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
    }
}