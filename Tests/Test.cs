using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using Kli;
using Kli.Infrastructure;
using NSubstitute;

namespace Tests
{
    /// <summary>
    /// Classe base para os testes
    /// </summary>
    public abstract class Test
    {
        /// <summary>
        /// Construtor. SetUp dos testes.
        /// </summary>
        protected Test()
        {
            Program.DependencyResolver = DependencyResolverFromProgram;
            DependencyResolverForTest.Reset();
        }
        
        /// <summary>
        /// Fixture.
        /// </summary>
        protected Fixture Fixture { get; } = new Fixture();

        /// <summary>
        /// Resolvedor de dependências usado originalmente pelo programa.
        /// </summary>
        protected static IDependencyResolver DependencyResolverFromProgram { get; } = Program.DependencyResolver;

        /// <summary>
        /// Resolvedor de dependências ajustado para atender os testes com Substitute.
        /// </summary>
        protected static DependencyResolverForTest DependencyResolverForTest { get; } = new DependencyResolverForTest();
        
        /// <summary>
        /// Método para testar se um tipo implementa determinada classe ou interface.
        /// </summary>
        /// <param name="tipoDaClasse">Tipo da classe.</param>
        /// <param name="tipoQueDeveSerImplementado">Tipo que deve ser implementado.</param>
        protected static void verifica_se_classe_implementa_o_tipo(Type tipoDaClasse, Type tipoQueDeveSerImplementado)
        {
            // Arrange, Given
            
            // Act, When

            var implementa = tipoQueDeveSerImplementado.IsAssignableFrom(tipoDaClasse);

            // Assert, Then

            implementa.Should().BeTrue();
        }
        
        /// <summary>
        /// Método para testar se um método existe com base na sua assinatura.
        /// </summary>
        /// <param name="tipo">Tipo a ser consultado.</param>
        /// <param name="assinaturaEsperada">Assinatura esperada.</param>
        protected static void verifica_se_o_método_existe_com_base_na_assinatura(Type tipo, string assinaturaEsperada)
        {
            // Arrange, Given

            string NomeDoTipo(Type type)
            {
                var nome = type.ToString();
                if (Regex.IsMatch(nome, @"`\d+\["))
                {
                    nome = nome
                        .Replace("[", "<")
                        .Replace("]", ">");
                }
                nome = nome
                    .Replace(",", ", ");
                nome = Regex.Replace(nome, @"(\w+\.|`\d+)", string.Empty);
                return nome;
            }
            
            string AssinaturaDoMétodo(MethodInfo método)
            {
                var parâmetrosParaGeneric = método.GetGenericArguments().ToList();
                var parâmetrosDoMétodo = método.GetParameters().ToList();
             
                var assinatura = new StringBuilder();
                
                assinatura.Append($"{NomeDoTipo(método.ReturnType)} {método.Name}");
                
                if (parâmetrosParaGeneric.Count > 0)
                {
                    assinatura.Append($"<{parâmetrosParaGeneric.Select(NomeDoTipo).Aggregate((acumulador, nomeDoTipo) => $"{(string.IsNullOrWhiteSpace(acumulador) ? "" : $"{acumulador}, ")}{nomeDoTipo}")}>");
                }

                assinatura.Append(parâmetrosDoMétodo.Count > 0
                    ? $"({parâmetrosDoMétodo.Select(a => $"{NomeDoTipo(a.ParameterType)}{(a.HasDefaultValue ? $" = {(a.ParameterType == typeof(char) && ((char)0).Equals(a.DefaultValue) ? "0" : a.DefaultValue)}": "")}").Aggregate((acumulador, nomeDoTipo) => $"{(string.IsNullOrWhiteSpace(acumulador) ? "" : $"{acumulador}, ")}{nomeDoTipo}")})"
                    : "()");

                return assinatura.ToString();
            }

            IEnumerable<MethodInfo> MétodosEncontrados()
            {
                var nomeDoMétodo = Regex.Match(assinaturaEsperada, @"\w+(?=(|<[^>]+>)\()").Value;
                return tipo.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).Where(a => a.Name == nomeDoMétodo);
            }

            IEnumerable<string> AssinaturasEncontradas() => MétodosEncontrados().Select(AssinaturaDoMétodo).ToList();

            // Act, When

            var assinaturasEncontrada = AssinaturasEncontradas();

            // Assert, Then

            assinaturasEncontrada.Should().Contain(assinaturaEsperada);
        }
        
        /// <summary>
        /// Verifica se o cache está sendo usado ao consulta uma propriedade.
        /// A evidência é o tempo menor na segunda consulta.
        /// </summary>
        /// <param name="tipo">Tipo.</param>
        /// <param name="nomeDePropriedade">Nome da propriedade.</param>
        protected static void verifica_se_o_cache_está_sendo_usado_na_consulta(Type tipo, string nomeDePropriedade)
        {
            // Arrange, Given
            
            var métodoGetInstance =
                DependencyResolverFromProgram.GetType()
                    .GetMethod("GetInstance", 
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static) ??
                Substitute.For<MethodInfo>();
            var métodoGetInstanceGeneric = métodoGetInstance.MakeGenericMethod(tipo);
            var instânciaDoTipo = métodoGetInstanceGeneric.Invoke(DependencyResolverFromProgram, new object[0]);
            var propriedade = tipo.GetProperty(nomeDePropriedade,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
           
            // Act, When

            var (tempo1, valor1, tempo2, valor2) = cronometrar_consulta_dupla(() => propriedade?.GetValue(instânciaDoTipo));

            // Assert, Then
            
            valor2.Should().BeEquivalentTo(valor1);
            tempo2.Should().BeLessThan(tempo1);
        }

        /// <summary>
        /// Faz uma consulta qualquer e cronometra o tempo.
        /// </summary>
        /// <param name="consulta">Função de consulta.</param>
        /// <typeparam name="T">Tipo de retorno.</typeparam>
        /// <returns>Tempo e valores.</returns>
        protected static Tuple<long, T> cronometrar_consulta<T>(Func<T> consulta)
        {
            var cronômetro = new Stopwatch();
                
            cronômetro.Start();
            var valores = consulta();
            cronômetro.Stop();
            var tempo = cronômetro.ElapsedTicks;
            
            return new Tuple<long, T>(tempo, valores);
        }
        
        /// <summary>
        /// Faz uma consulta qualquer e cronometra o tempo. Executa duas vezes.
        /// </summary>
        /// <param name="consulta">Função de consulta.</param>
        /// <typeparam name="T">Tipo de retorno.</typeparam>
        /// <returns>Tempo e valores duas vezes.</returns>
        private static Tuple<long, T, long, T> cronometrar_consulta_dupla<T>(Func<T> consulta)
        {
            var (tempo1, valores1) = cronometrar_consulta(consulta);
            var (tempo2, valores2) = cronometrar_consulta(consulta);
            
            return new Tuple<long, T, long, T>(tempo1, valores1, tempo2, valores2);
        }
    }
}