using System;
using Kli.IO;
using Xunit;

namespace Tests.UnitTests.Kli.IO
{
    public class TestIOutputMarkers: Test
    {
        [Theory]
        [InlineData(typeof(IOutputMarkers), "String Escape(String)")]
        [InlineData(typeof(IOutputMarkers), "Char get_Error()")]
        [InlineData(typeof(IOutputMarkers), "Char get_Question()")]
        [InlineData(typeof(IOutputMarkers), "Char get_Answer()")]
        [InlineData(typeof(IOutputMarkers), "Char get_Highlight()")]
        [InlineData(typeof(IOutputMarkers), "Char get_Light()")]
        [InlineData(typeof(IOutputMarkers), "Char get_Low()")]
        [InlineData(typeof(IOutputMarkers), "String get_Markers()")]
        [InlineData(typeof(IOutputMarkers), "String get_MarkersEscapedForRegexJoined()")]
        [InlineData(typeof(IOutputMarkers), "String[] get_MarkersEscapedForRegexSeparated()")]
        public void verifica_se_os_métodos_existem_com_base_na_assinatura(Type tipo, string assinaturaEsperada) =>
            verifica_se_o_método_existe_com_base_na_assinatura(tipo, assinaturaEsperada);
    }
}