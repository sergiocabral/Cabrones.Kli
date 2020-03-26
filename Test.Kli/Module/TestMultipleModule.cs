using Cabrones.Test;
using FluentAssertions;
using Kli.Core;
using NSubstitute;
using Xunit;

namespace Kli.Module
{
    public class TestMultipleModule
    {
        [Fact]
        public void o_método_Run_deve_chamar_o_serviço_IUtils_para_exibir_as_opções_inicias()
        {
            // Arrange, Given

            var utils = Substitute.For<IInteraction>();
            var multipleModule = new MultipleModule(utils);

            // Act, When

            multipleModule.Run();

            // Assert, Then

            utils.Received(1).StartInteraction();
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(MultipleModule);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(Multiple<IModule>),
                typeof(IMultipleModule),
                typeof(IMultiple<IModule>),
                typeof(IModule));
            sut.AssertMyOwnImplementations(
                typeof(Multiple<IModule>),
                typeof(IMultipleModule));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}