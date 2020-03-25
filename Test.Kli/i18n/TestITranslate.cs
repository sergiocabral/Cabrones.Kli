using System;
using Cabrones.Test;
using FluentAssertions;
using Xunit;

namespace Kli.i18n
{
    public class TestITranslate
    {
        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(ITranslate);

            // Assert, Then

            sut.AssertMyImplementations();
            sut.AssertMyOwnImplementations();
            sut.AssertMyOwnPublicPropertiesCount(3);
            sut.AssertPublicPropertyPresence("String LanguageDefault { get; set; }");
            sut.AssertPublicPropertyPresence("IDictionary<String, IDictionary<String, String>> Translates { get; }");
            sut.AssertMyOwnPublicMethodsCount(5);
            sut.AssertPublicMethodPresence("String Get(String, String = null)");
            sut.AssertPublicMethodPresence("Void Clear()");
            sut.AssertPublicMethodPresence("IDictionary<String, IDictionary<String, String>> LoadFromDictionary(IDictionary<String, IDictionary<String, String>>)");
            sut.AssertPublicMethodPresence("IDictionary<String, IDictionary<String, String>> LoadFromText(String)");
            sut.AssertPublicMethodPresence("IDictionary<String, IDictionary<String, String>> LoadFromResource(Assembly, String)");

            sut.IsInterface.Should().BeTrue();
        }
    }
}