using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Test;
using Xunit;

namespace Kli.i18n
{
    public class TestTranslate: BaseForTest
    {
        [Theory]
        [InlineData(typeof(Translate), 8)]
        public void verifica_se_o_total_de_métodos_públicos_declarados_está_correto_neste_tipo(Type tipo, int totalDeMétodosEsperado) =>
            TestTypeMethodsCount(tipo, totalDeMétodosEsperado);

        [Theory]
        [InlineData(typeof(Translate), typeof(ITranslate))]
        public void verifica_se_classe_implementa_os_tipos_necessários(Type tipoDaClasse, params Type[] tiposQueDeveSerImplementado) =>
            TestTypeImplementations(tipoDaClasse, tiposQueDeveSerImplementado);
        
        [Fact]
        public void ao_carregar_traduções_de_dicionário_deve_retornar_os_valores_inseridos_sem_ser_uma_referência_direta()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();
            
            var conteudo = new Dictionary<string, IDictionary<string, string>>
            {
                {
                    Fixture.Create<string>(), new Dictionary<string, string>
                    {
                        { Fixture.Create<string>(), Fixture.Create<string>() }
                    }
                },
                {
                    Fixture.Create<string>(), new Dictionary<string, string>
                    {
                        { Fixture.Create<string>(), Fixture.Create<string>() }
                    }
                },
                {
                    Fixture.Create<string>(), new Dictionary<string, string>
                    {
                        { Fixture.Create<string>(), Fixture.Create<string>() }
                    }
                }
            };

            // Act, When

            var traduções = tradução.LoadFromDictionary(conteudo);

            // Assert, Then

            traduções.Should().HaveCount(conteudo.Count);
            traduções.Should().BeEquivalentTo(conteudo);
            traduções.Should().NotBeSameAs(conteudo);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_dicionário_não_deve_aceitar_duplicações_pois_os_últimos_valores_sobrescrevem_os_primeiros()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();
            
            var conteudoInseridoPrimeiro = new Dictionary<string, IDictionary<string, string>>
            {
                { "valor", new Dictionary<string, string> { { "idioma", "único 1" } } }
            };
            var conteudoInseridoPorÚltimo = new Dictionary<string, IDictionary<string, string>>
            {
                { "valor", new Dictionary<string, string> { { "idioma", "único 2" } } }
            };

            // Act, When

            tradução.LoadFromDictionary(conteudoInseridoPrimeiro);
            tradução.LoadFromDictionary(conteudoInseridoPorÚltimo);
            var traduções = tradução.Translates;

            // Assert, Then

            traduções.Should().HaveCount(1);
            traduções.First().Key.Should().Be(conteudoInseridoPorÚltimo.First().Key);
            traduções.First().Value.First().Key.Should().Be(conteudoInseridoPorÚltimo.First().Value.First().Key);
            traduções.First().Value.First().Value.Should().Be(conteudoInseridoPorÚltimo.First().Value.First().Value);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_dicionário_com_valor_null_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var contagemDeTraduçõesNoInício = tradução.Translates.Count;

            // Act, When

            var traduções = tradução.LoadFromDictionary(null);
            var contagemDeTraduçõesNoFinal = tradução.Translates.Count;

            // Assert, Then

            traduções.Should().HaveCount(0);
            contagemDeTraduçõesNoFinal.Should().Be(contagemDeTraduçõesNoInício);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_dicionário_com_valores_incompletos_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var contagemDeTraduçõesNoInício = tradução.Translates.Count;
            var conteudo = new Dictionary<string, IDictionary<string, string>>
            {
                { Fixture.Create<string>(), new Dictionary<string, string>() },
                { Fixture.Create<string>(), null }
            };

            // Act, When

            var traduções = tradução.LoadFromDictionary(conteudo);
            var contagemDeTraduçõesNoFinal = tradução.Translates.Count;

            // Assert, Then

            traduções.Should().HaveCount(0);
            contagemDeTraduçõesNoFinal.Should().Be(contagemDeTraduçõesNoInício);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_texto_deve_retornar_os_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var valorTexto = Fixture.Create<string>();
            var valorIdioma = Fixture.Create<string>();
            var valorTradução = Fixture.Create<string>();
            var conteudo = $@"
[
  {{
    ""Key"": ""{valorTexto}"",
    ""Value"": [
      {{
        ""Key"": ""{valorIdioma}"",
        ""Value"": ""{valorTradução}""
      }}
    ]
  }}
]";

            // Act, When

            var traduções = tradução.LoadFromText(conteudo);

            // Assert, Then

            traduções.Should().HaveCount(1);
            traduções.First().Key.Should().Be(valorTexto);
            traduções.First().Value.First().Key.Should().Be(valorIdioma);
            traduções.First().Value.First().Value.Should().Be(valorTradução);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_texto_não_deve_aceitar_duplicações_pois_os_últimos_valores_sobrescrevem_os_primeiros()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();
            
            var valorTexto = Fixture.Create<string>();
            var valorIdioma = Fixture.Create<string>();
            var valorTradução1 = Fixture.Create<string>();
            var valorTradução2 = Fixture.Create<string>();
            string CriaConteudo(string valorTradução) => $@"
[
  {{
    ""Key"": ""{valorTexto}"",
    ""Value"": [
      {{
        ""Key"": ""{valorIdioma}"",
        ""Value"": ""{valorTradução}""
      }}
    ]
  }}
]";

            var conteudoInseridoPrimeiro = CriaConteudo(valorTradução1);
            var conteudoInseridoPorÚltimo = CriaConteudo(valorTradução2);

            // Act, When

            tradução.LoadFromText(conteudoInseridoPrimeiro);
            tradução.LoadFromText(conteudoInseridoPorÚltimo);
            var traduções = tradução.Translates;

            // Assert, Then

            traduções.Should().HaveCount(1);
            traduções.First().Key.Should().Be(valorTexto);
            traduções.First().Value.First().Key.Should().Be(valorIdioma);
            traduções.First().Value.First().Value.Should().Be(valorTradução2);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_texto_com_valor_null_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var contagemDeTraduçõesNoInício = tradução.Translates.Count;

            // Act, When

            var traduções = tradução.LoadFromText(null);
            var contagemDeTraduçõesNoFinal = tradução.Translates.Count;

            // Assert, Then

            traduções.Should().HaveCount(0);
            contagemDeTraduçõesNoFinal.Should().Be(contagemDeTraduçõesNoInício);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_texto_com_valores_incompletos_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var contagemDeTraduçõesNoInício = tradução.Translates.Count;
            var conteudo = $@"
[
  {{
    ""Key"": ""{Fixture.Create<string>()}"",
    ""Value"": [ ]
  }},
  {{
    ""Key"": ""{Fixture.Create<string>()}"",
    ""Value"": null
  }}
]";

            // Act, When

            var traduções = tradução.LoadFromText(conteudo);
            var contagemDeTraduçõesNoFinal = tradução.Translates.Count;

            // Assert, Then

            traduções.Should().HaveCount(0);
            contagemDeTraduçõesNoFinal.Should().Be(contagemDeTraduçõesNoInício);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_texto_com_valores_inválidos_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var contagemDeTraduçõesNoInício = tradução.Translates.Count;
            const string conteudo = "Isso não tem formato de IDictionary.";

            // Act, When

            var traduções = tradução.LoadFromText(conteudo);
            var contagemDeTraduçõesNoFinal = tradução.Translates.Count;

            // Assert, Then

            traduções.Should().HaveCount(0);
            contagemDeTraduçõesNoFinal.Should().Be(contagemDeTraduçõesNoInício);
        } 
        
        [Fact]
        public void ao_carregar_traduções_de_recurso_embutido_deve_retornar_os_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var nomeDoRecurso = $"{GetType().FullName}Valido1.json";

            // Act, When

            var traduções = tradução.LoadFromResource(assembly,  nomeDoRecurso);

            // Assert, Then

            traduções.Should().HaveCount(2);
            traduções.First().Key.Should().Be("word 1");
            traduções.First().Value.First().Key.Should().Be("pt");
            traduções.First().Value.First().Value.Should().Be("palavra 1");
            traduções.Last().Key.Should().Be("word 2");
            traduções.Last().Value.First().Key.Should().Be("pt");
            traduções.Last().Value.First().Value.Should().Be("palavra 2");
        }
        
        [Fact]
        public void ao_carregar_traduções_de_recurso_embutido_não_deve_aceitar_duplicações_pois_os_últimos_valores_sobrescrevem_os_primeiros()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var nomeDoRecurso1 = $"{GetType().FullName}Valido1.json";
            var nomeDoRecurso2 = $"{GetType().FullName}Valido2.json";

            // Act, When

            tradução.LoadFromResource(assembly, nomeDoRecurso1);
            tradução.LoadFromResource(assembly, nomeDoRecurso2);
            var traduções = tradução.Translates;

            // Assert, Then

            traduções.Should().HaveCount(2);
            traduções.First().Key.Should().Be("word 1");
            traduções.First().Value.First().Key.Should().Be("pt");
            traduções.First().Value.First().Value.Should().Be("palavra 1b");
            traduções.Last().Key.Should().Be("word 2");
            traduções.Last().Value.First().Key.Should().Be("pt");
            traduções.Last().Value.First().Value.Should().Be("palavra 2b");
        }
        
        [Fact]
        public void ao_carregar_traduções_de_recurso_embutido_com_valor_null_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var contagemDeTraduçõesNoInício = tradução.Translates.Count;

            // Act, When

            var traduçõesNaChamadaComAssemblyNull = tradução.LoadFromResource(null, null);
            var contagemDeTraduçõesNoFinalNaChamadaComAssemblyNull = tradução.Translates.Count;
            
            var traduçõesNaChamadaComResourceNull = tradução.LoadFromResource(assembly, null);
            var contagemDeTraduçõesNoFinalNaChamadaComResourceNull = tradução.Translates.Count;

            // Assert, Then

            traduçõesNaChamadaComAssemblyNull.Should().HaveCount(0);
            contagemDeTraduçõesNoFinalNaChamadaComAssemblyNull.Should().Be(contagemDeTraduçõesNoInício);
            
            traduçõesNaChamadaComResourceNull.Should().HaveCount(0);
            contagemDeTraduçõesNoFinalNaChamadaComResourceNull.Should().Be(contagemDeTraduçõesNoInício);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_recurso_embutido_com_valores_incompletos_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var contagemDeTraduçõesNoInício = tradução.Translates.Count;
            var nomeDoRecurso = $"{GetType().FullName}Incompleto.json";

            // Act, When

            var traduções = tradução.LoadFromResource(assembly, nomeDoRecurso);
            var contagemDeTraduçõesNoFinal = tradução.Translates.Count;

            // Assert, Then

            traduções.Should().HaveCount(0);
            contagemDeTraduçõesNoFinal.Should().Be(contagemDeTraduçõesNoInício);
        }
        
        [Fact]
        public void ao_carregar_traduções_de_recurso_embutido_com_valores_inválidos_deve_retornar_zero_valores_inseridos()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var contagemDeTraduçõesNoInício = tradução.Translates.Count;
            var nomeDoRecurso = $"{GetType().FullName}Invalido.json";

            // Act, When

            var traduções = tradução.LoadFromResource(assembly, nomeDoRecurso);
            var contagemDeTraduçõesNoFinal = tradução.Translates.Count;

            // Assert, Then

            traduções.Should().HaveCount(0);
            contagemDeTraduçõesNoFinal.Should().Be(contagemDeTraduçõesNoInício);
        }
        
        [Fact]
        public void deve_ser_capaz_de_retornar_uma_tradução()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();

            var valorTexto = Fixture.Create<string>();
            var valorIdioma = Fixture.Create<string>();
            var valorTradução = Fixture.Create<string>();
            tradução.LoadFromText($@"
[
  {{
    ""Key"": ""{valorTexto}"",
    ""Value"": [
      {{
        ""Key"": ""{valorIdioma}"",
        ""Value"": ""{valorTradução}""
      }}
    ]
  }}
]");
            
            // Act, When

            var valorTraduçãoRetornado = tradução.Get(valorTexto, valorIdioma);

            // Assert, Then

            valorTraduçãoRetornado.Should().Be(valorTradução).And.NotBe(valorTexto);
        }
        
        [Fact]
        public void deve_retornar_o_texto_de_entrada_se_não_existir_tradução()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var valorTexto = Fixture.Create<string>();
            var valorIdioma = Fixture.Create<string>();
            
            // Act, When

            var valorTraduçãoRetornadoInformandoOIdioma = tradução.Get(valorTexto, valorIdioma);
            var valorTraduçãoRetornadoSemInformarOIdioma = tradução.Get(valorTexto);

            // Assert, Then

            valorTraduçãoRetornadoInformandoOIdioma.Should().Be(valorTexto);
            valorTraduçãoRetornadoSemInformarOIdioma.Should().Be(valorTexto);
        }
            
        [Fact]
        public void deve_retornar_a_tradução_no_idioma_padrão_se_não_for_especificado()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            tradução.Clear();

            var valorTexto = Fixture.Create<string>();
            var valorIdioma = Fixture.Create<string>();
            var valorTradução = Fixture.Create<string>();
            tradução.LoadFromText($@"
[
  {{
    ""Key"": ""{valorTexto}"",
    ""Value"": [
      {{
        ""Key"": ""{valorIdioma}"",
        ""Value"": ""{valorTradução}""
      }}
    ]
  }}
]");
            
            // Act, When

            tradução.LanguageDefault = valorIdioma;
            var valorTraduçãoRetornado = tradução.Get(valorTexto);

            // Assert, Then

            valorTraduçãoRetornado.Should().Be(valorTradução).And.NotBe(valorTexto);
        }
        
        [Fact]
        public void deve_retornar_a_tradução_no_idioma_especificado_ao_invés_do_idioma_padrão()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            
            var valorTexto = Fixture.Create<string>();
            var valorIdiomaPadrao = Fixture.Create<string>();
            var valorIdiomaDoTexto = Fixture.Create<string>();
            var valorTradução = Fixture.Create<string>();
            tradução.LoadFromText($@"
[
  {{
    ""Key"": ""{valorTexto}"",
    ""Value"": [
      {{
        ""Key"": ""{valorIdiomaDoTexto}"",
        ""Value"": ""{valorTradução}""
      }}
    ]
  }}
]");
            
            // Act, When

            tradução.LanguageDefault = valorIdiomaPadrao;
            var valorTraduçãoRetornado = tradução.Get(valorTexto, valorIdiomaDoTexto);

            // Assert, Then

            valorTraduçãoRetornado.Should().Be(valorTradução).And.NotBe(valorTexto);
        }
        
        [Fact]
        public void deve_ser_capaz_de_limpar_traduções_previamente_carregadas()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            
            var valorTexto = Fixture.Create<string>();
            var valorIdioma = Fixture.Create<string>();
            var valorTradução = Fixture.Create<string>();
            tradução.LoadFromText($@"
[
  {{
    ""Key"": ""{valorTexto}"",
    ""Value"": [
      {{
        ""Key"": ""{valorIdioma}"",
        ""Value"": ""{valorTradução}""
      }}
    ]
  }}
]");
            
            // Act, When

            tradução.Clear();
            var valorTraduçãoRetornado = tradução.Get(valorTexto, valorIdioma);

            // Assert, Then

            valorTraduçãoRetornado.Should().Be(valorTexto).And.NotBe(valorTradução);
        }
        
        [Fact]
        public void o_retorno_da_lista_de_traduções_não_pode_ser_uma_referência_para_os_dados_reais()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            
            var valorTexto = Fixture.Create<string>();
            var valorIdioma = Fixture.Create<string>();
            var valorTradução = Fixture.Create<string>();
            tradução.LoadFromText($@"
[
  {{
    ""Key"": ""{valorTexto}"",
    ""Value"": [
      {{
        ""Key"": ""{valorIdioma}"",
        ""Value"": ""{valorTradução}""
      }}
    ]
  }}
]");
            
            // Act, When

            tradução.Translates.Clear();
            var valorTraduçãoRetornado = tradução.Get(valorTexto, valorIdioma);

            // Assert, Then

            valorTraduçãoRetornado.Should().Be(valorTradução).And.NotBe(valorTexto);
        }
        
        [Fact]
        public void deve_ser_possível_definir_um_idioma_padrão()
        {
            // Arrange, Given

            var tradução = DependencyResolverFromProgram.GetInstance<ITranslate>();
            var valorIdioma = Fixture.Create<string>();
            
            // Act, When

            var valorIdiomaInicial = tradução.LanguageDefault;
            tradução.LanguageDefault = valorIdioma;
            var valorIdiomaConsultado = tradução.LanguageDefault;

            // Assert, Then

            valorIdiomaConsultado.Should().Be(valorIdioma).And.NotBe(valorIdiomaInicial);
        }
        
        [Fact]
        public void o_serviço_ITranslate_deve_consultar_o_serviço_ILanguage_para_determinar_o_idioma_padrão()
        {
            // Arrange, Given

            var idioma = Substitute.For<ILanguage>();
            var idiomaAtualConsultado = 0;
            idioma.Current.ReturnsForAnyArgs(idiomaAtualConsultado++.ToString());
            
            // Act, When

            var _ = new Translate(idioma) as ITranslate;
            
            // Assert, Then

            idiomaAtualConsultado.Should().Be(1);
        }
        
        [Fact]
        public void o_idioma_padrão_deve_ser_definido_com_base_no_serviço_ILanguage_propriedade_Current()
        {
            // Arrange, Given

            var idioma = DependencyResolverFromProgram.GetInstance<ILanguage>();
            var tradução = new Translate(idioma) as ITranslate;
            
            // Act, When

            var valorIdiomaDoSistema = idioma.Current;
            var valorIdiomaDaTradução = tradução.LanguageDefault;

            // Assert, Then

            valorIdiomaDaTradução.Should().Be(valorIdiomaDoSistema);
        }
    }
}