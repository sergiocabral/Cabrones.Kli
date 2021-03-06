﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cabrones.Test;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Kli.Output
{
    public class TestOutputWriter
    {
        [Theory]
        [InlineData("Texto normal.", "Texto normal.")]
        [InlineData("Texto com *marcador* tipo **highlight** aqui.", "Texto com marcador tipo *highlight* aqui.")]
        [InlineData("Texto com _marcador_ tipo __light__ aqui.", "Texto com marcador tipo _light_ aqui.")]
        [InlineData("Texto com #marcador# tipo ##low## aqui.", "Texto com marcador tipo #low# aqui.")]
        [InlineData("Texto com !marcador! tipo !!error!! aqui.", "Texto com marcador tipo !error! aqui.")]
        [InlineData("Texto com ?marcador? tipo ??question?? aqui.", "Texto com marcador tipo ?question? aqui.")]
        [InlineData("Texto com @marcador@ tipo @@answer@@ aqui.", "Texto com marcador tipo @answer@ aqui.")]
        [InlineData("Texto com\rquebra de linha CR.", "Texto com\r\nquebra de linha CR.")]
        [InlineData("Texto com\nquebra de linha LF.", "Texto com\r\nquebra de linha LF.")]
        [InlineData("Texto com\r\nquebra de linha CR LF.", "Texto com\r\nquebra de linha CR LF.")]
        [InlineData("Texto com\n\rquebra de linha LF CR.", "Texto com\r\nquebra de linha LF CR.")]
        [InlineData("*Marcador1 iniciado, _Marcador2 iniciado, #Marcador3 iniciado...",
            "Marcador1 iniciado, Marcador2 iniciado, Marcador3 iniciado...")]
        [InlineData("**Marcador1 não iniciado, __Marcador2 não iniciado, ##Marcador3 não iniciado...",
            "*Marcador1 não iniciado, _Marcador2 não iniciado, #Marcador3 não iniciado...")]
        [InlineData("Marcador escapado nome____sobrenome por exemplo.",
            "Marcador escapado nome__sobrenome por exemplo.")]
        [InlineData("Marcador ___múltiplo___ simples.", "Marcador _múltiplo_ simples.")]
        [InlineData("Marcador múltiplo ____ lado-a-lado.", "Marcador múltiplo __ lado-a-lado.")]
        [InlineData("Marcador múltiplo _____ ímpar.", "Marcador múltiplo __ ímpar.")]
        [InlineData("Marcador no final___", "Marcador no final_")]
        [InlineData("Marcador no final__", "Marcador no final_")]
        [InlineData("Marcador no final_", "Marcador no final")]
        [InlineData("!!!a!!b!!c!", "!a!b!c")]
        [InlineData(" ", " ")]
        [InlineData("", "")]
        public void
            o_parse_que_desconsidera_marcadores_deve_enviar_o_texto_para_ser_escrito_corretamente_para_o_usuario(
                string textoEnviado, string textoEscritoEsperado)
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();

            // Act, When

            var textoEscrito = string.Empty;
            outputWriter.Parse(textoEnviado, textoRecebido => textoEscrito += textoRecebido);

            // Assert, Then

            textoEscrito.Should().Be(textoEscritoEsperado);
        }

        [Theory]
        [InlineData("Texto normal.", "Texto normal.")]
        [InlineData("Texto com *marcador* tipo **highlight** aqui.", "Texto com marcador tipo *highlight* aqui.")]
        [InlineData("Texto com _marcador_ tipo __light__ aqui.", "Texto com marcador tipo _light_ aqui.")]
        [InlineData("Texto com #marcador# tipo ##low## aqui.", "Texto com marcador tipo #low# aqui.")]
        [InlineData("Texto com !marcador! tipo !!error!! aqui.", "Texto com marcador tipo !error! aqui.")]
        [InlineData("Texto com ?marcador? tipo ??question?? aqui.", "Texto com marcador tipo ?question? aqui.")]
        [InlineData("Texto com @marcador@ tipo @@answer@@ aqui.", "Texto com marcador tipo @answer@ aqui.")]
        [InlineData("Texto com\rquebra de linha CR.", "Texto com\r\nquebra de linha CR.")]
        [InlineData("Texto com\nquebra de linha LF.", "Texto com\r\nquebra de linha LF.")]
        [InlineData("Texto com\r\nquebra de linha CR LF.", "Texto com\r\nquebra de linha CR LF.")]
        [InlineData("Texto com\n\rquebra de linha LF CR.", "Texto com\r\nquebra de linha LF CR.")]
        [InlineData("*Marcador1 iniciado, _Marcador2 iniciado, #Marcador3 iniciado...",
            "Marcador1 iniciado, Marcador2 iniciado, Marcador3 iniciado...")]
        [InlineData("**Marcador1 não iniciado, __Marcador2 não iniciado, ##Marcador3 não iniciado...",
            "*Marcador1 não iniciado, _Marcador2 não iniciado, #Marcador3 não iniciado...")]
        [InlineData("Marcador escapado nome____sobrenome por exemplo.",
            "Marcador escapado nome__sobrenome por exemplo.")]
        [InlineData("Marcador ___múltiplo___ simples.", "Marcador _múltiplo_ simples.")]
        [InlineData("Marcador múltiplo ____ lado-a-lado.", "Marcador múltiplo __ lado-a-lado.")]
        [InlineData("Marcador múltiplo _____ ímpar.", "Marcador múltiplo __ ímpar.")]
        [InlineData("Marcador no final___", "Marcador no final_")]
        [InlineData("Marcador no final__", "Marcador no final_")]
        [InlineData("Marcador no final_", "Marcador no final")]
        [InlineData(" ", " ")]
        [InlineData("", "")]
        public void
            o_parse_que_envia_trechos_para_cada_marcador_deve_enviar_o_texto_para_ser_escrito_corretamente_para_o_usuario(
                string textoEnviado, string textoEscritoEsperado)
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();

            // Act, When

            var textoEscrito = string.Empty;
            outputWriter.Parse(textoEnviado, (textoRecebido, marcador) => textoEscrito += textoRecebido);

            // Assert, Then

            textoEscrito.Should().Be(textoEscritoEsperado);
        }

        [Theory]
        [InlineData("Texto normal.")]
        [InlineData("Texto *marcador1* e _marcador2_ e #marcador3#.")]
        [InlineData("Texto *marcador1 iniciado e _marcador2 iniciado e #marcador3 iniciado.")]
        [InlineData("Texto com\rquebra de linha CR.")]
        [InlineData("Texto com\nquebra de linha LF.")]
        [InlineData("Texto com\r\nquebra de linha CR LF.")]
        [InlineData("Texto com\n\rquebra de linha LF CR.")]
        [InlineData(" ")]
        public void o_parse_que_desconsidera_marcadores_deve_enviar_o_texto_para_ser_escrito_em_uma_única_vez(
            string textoEnviado)
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();

            // Act, When

            var escritas = 0;
            outputWriter.Parse(textoEnviado, textoRecebido => escritas++);

            // Assert, Then

            escritas.Should().Be(1);
        }

        [Theory]
        [InlineData("Texto normal.", 1)]
        [InlineData("Texto *marcador1* e _marcador2_ e #marcador3#.", 7)]
        [InlineData("Texto **sem marcador1** e __sem marcador2__ e ##sem marcador3##.", 1)]
        [InlineData("Texto *marcador1 iniciado e _marcador2 iniciado e #marcador3 iniciado.", 4)]
        [InlineData("Texto **sem marcador1 iniciado e __sem marcador2 iniciado e ##sem marcador3 iniciado.", 1)]
        [InlineData("Texto com\rquebra de linha CR.", 1)]
        [InlineData("Texto com\nquebra de linha LF.", 1)]
        [InlineData("Texto com\r\nquebra de linha CR LF.", 1)]
        [InlineData("Texto com\n\rquebra de linha LF CR.", 1)]
        [InlineData(" ", 1)]
        [InlineData("", 0)]
        public void o_parse_que_envia_trechos_para_cada_marcador_deve_enviar_N_vezes_trechos_para_ser_escrito(
            string textoEnviado, int escritasEsperadas)
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();

            // Act, When

            var escritas = 0;
            outputWriter.Parse(textoEnviado, (textoRecebido, marcador) => escritas++);

            // Assert, Then

            escritas.Should().Be(escritasEsperadas);
        }

        [Theory]
        [InlineData("Texto normal.", " |Texto normal.")]
        [InlineData("Texto *marcador1* e _marcador2_ e #marcador3#.", " |Texto ", "*|marcador1", " | e ", "_|marcador2",
            " | e ", "#|marcador3", " |.")]
        [InlineData("Texto **sem marcador1** e __sem marcador2__ e ##sem marcador3##.",
            " |Texto *sem marcador1* e _sem marcador2_ e #sem marcador3#.")]
        [InlineData("!Texto **sem marcador1** e __sem marcador2__ e ##sem marcador3##.",
            "!|Texto *sem marcador1* e _sem marcador2_ e #sem marcador3#.")]
        [InlineData("Texto *marcador1 iniciado e _marcador2 iniciado e #marcador3 iniciado.", " |Texto ",
            "*|marcador1 iniciado e ", "_|marcador2 iniciado e ", "#|marcador3 iniciado.")]
        [InlineData("Texto **sem marcador1 iniciado e __sem marcador2 iniciado e ##sem marcador3 iniciado.",
            " |Texto *sem marcador1 iniciado e _sem marcador2 iniciado e #sem marcador3 iniciado.")]
        [InlineData("Texto com\rquebra de linha CR.", " |Texto com\r\nquebra de linha CR.")]
        [InlineData("Texto com\nquebra de linha LF.", " |Texto com\r\nquebra de linha LF.")]
        [InlineData("Texto com\r\nquebra de linha CR LF.", " |Texto com\r\nquebra de linha CR LF.")]
        [InlineData("Texto com\n\rquebra de linha LF CR.", " |Texto com\r\nquebra de linha LF CR.")]
        [InlineData(" ", " | ")]
        [InlineData("")]
        public void
            o_parse_que_envia_trechos_para_cada_marcador_deve_enviar_separadamente_cada_trecho_com_seu_respectivo_marcador(
                string textoEnviado, params string[] escritasEsperadasComMarcadorETexto)
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();
            var escritasEsperadas = escritasEsperadasComMarcadorETexto
                .Select(a => new Tuple<string, char>(a.Substring(2), a[0] != ' ' ? a[0] : (char) 0)).ToList();

            // Act, When

            var escritasRealizadas = new List<Tuple<string, char>>();
            outputWriter.Parse(textoEnviado,
                (textoRecebido, marcador) => escritasRealizadas.Add(new Tuple<string, char>(textoRecebido, marcador)));

            // Assert, Then

            escritasRealizadas.Should().BeEquivalentTo(escritasEsperadas);
        }

        [Fact]
        public void o_parse_deve_enviar_o_texto_para_ser_escrito()
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();
            var textoEnviado = this.Fixture<string>();

            // Act, When

            var textoEscritoIgnorandoMarcadores = string.Empty;
            outputWriter.Parse(textoEnviado, textoRecebido => textoEscritoIgnorandoMarcadores += textoRecebido);

            var textoEscritoConsiderandoMarcadores = string.Empty;
            outputWriter.Parse(textoEnviado,
                (textoRecebido, marcador) => textoEscritoConsiderandoMarcadores += textoRecebido);

            // Assert, Then

            textoEscritoIgnorandoMarcadores.Should().NotBeEmpty();
            textoEscritoConsiderandoMarcadores.Should().NotBeEmpty();
        }

        [Fact]
        public void o_parse_não_precisa_enviar_texto_vazio_para_ser_escrito()
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();
            const string marcadorQualquer = "!";

            // Act, When

            Action textoEscritoIgnorandoMarcadores = () => outputWriter.Parse(marcadorQualquer, (Action<string>) null);
            Action textoEscritoConsiderandoMarcadores =
                () => outputWriter.Parse(marcadorQualquer, (Action<string, char>) null);

            // Assert, Then

            textoEscritoIgnorandoMarcadores.Should().NotThrow();
            textoEscritoConsiderandoMarcadores.Should().NotThrow();
        }

        [Fact]
        public void o_parse_não_precisa_fazer_análise_de_texto_vazio_ou_nulo()
        {
            // Arrange, Given

            var outputWriter = Program.DependencyResolver.GetInstance<IOutputWriter>();
            var escritorIgnorandoMarcadores = Substitute.For<Action<string>>();
            var escritorConsiderandoMarcadores = Substitute.For<Action<string, char>>();

            // Act, When

            outputWriter.Parse(string.Empty, escritorIgnorandoMarcadores);
            outputWriter.Parse(null, escritorIgnorandoMarcadores);
            outputWriter.Parse(string.Empty, escritorConsiderandoMarcadores);
            outputWriter.Parse(null, escritorConsiderandoMarcadores);

            // Assert, Then

            escritorIgnorandoMarcadores.ReceivedWithAnyArgs(0).Invoke(default);
            escritorConsiderandoMarcadores.ReceivedWithAnyArgs(0).Invoke(default, default);
        }

        [Fact]
        public void verificações_declarativas()
        {
            // Arrange, Given
            // Act, When

            var sut = typeof(OutputWriter);

            // Assert, Then

            sut.AssertMyImplementations(
                typeof(IOutputWriter));
            sut.AssertMyOwnImplementations(
                typeof(IOutputWriter));
            sut.AssertMyOwnPublicPropertiesCount(0);
            sut.AssertMyOwnPublicMethodsCount(0);

            sut.IsClass.Should().BeTrue();
        }
    }
}