using System;
using FluentAssertions;
using Kli.Core;
using Xunit;

namespace Tests.UnitTests.Kli.Core
{
    public class EngineTest: Test
    {
        [Fact]
        public void método_principal_Run_deve_rodar_sem_error()
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