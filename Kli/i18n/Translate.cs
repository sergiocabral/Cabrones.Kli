using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Kli.i18n
{
    /// <summary>
    ///     Manipula traduções de texto.
    /// </summary>
    public class Translate : ITranslate
    {
        /// <summary>
        ///     Lista de traduções.
        /// </summary>
        private readonly IDictionary<string, IDictionary<string, string>> _translates =
            new Dictionary<string, IDictionary<string, string>>();

        /// <summary>
        ///     Construtor.
        /// </summary>
        /// <param name="language">Informações do idioma do usuário.</param>
        public Translate(ILanguage language)
        {
            LanguageDefault = language.Current;
        }

        /// <summary>
        ///     Idioma padrão.
        /// </summary>
        public string LanguageDefault { get; set; }

        /// <summary>
        ///     Retorna a tradução de um texto.
        /// </summary>
        /// <param name="text">Texto.</param>
        /// <param name="language">Idioma.</param>
        /// <returns>Tradução.</returns>
        public string Get(string text, string? language = null)
        {
            language = !string.IsNullOrWhiteSpace(language) ? language : LanguageDefault;
            if (!_translates.ContainsKey(text) || !_translates[text].ContainsKey(language)) return text;
            return _translates[text][language] ?? text;
        }

        /// <summary>
        ///     Lista de traduções atuais.
        /// </summary>
        public IDictionary<string, IDictionary<string, string>> Translates =>
            _translates.ToDictionary(a => a.Key, b => b.Value.ToDictionary(c => c.Key, d => d.Value)
                as IDictionary<string, string>);

        /// <summary>
        ///     Limpa todas as traduções carregadas.
        /// </summary>
        public void Clear()
        {
            _translates.Clear();
        }

        /// <summary>
        ///     Carregar as traduções.
        /// </summary>
        /// <param name="source">Dicionário com traduções.</param>
        /// <returns>Traduções carregadas.</returns>
        public IDictionary<string, IDictionary<string, string>>? LoadFromDictionary(
            IDictionary<string, IDictionary<string, string>> source)
        {
            var inserted = NewEmptyTranslates();

            if (source == null) return inserted;

            foreach (var (text, translates) in source)
            {
                if (translates == null) continue;

                foreach (var (language, translated) in translates)
                {
                    if (!_translates.ContainsKey(text)) _translates[text] = new Dictionary<string, string>();

                    if (!inserted.ContainsKey(text)) inserted[text] = new Dictionary<string, string>();

                    _translates[text][language] = translated;
                    inserted[text][language] = translated;
                }
            }

            return inserted;
        }

        /// <summary>
        ///     Carregar as traduções.
        /// </summary>
        /// <param name="source">Texto com traduções.</param>
        /// <returns>Traduções carregadas.</returns>
        public IDictionary<string, IDictionary<string, string>>? LoadFromText(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return NewEmptyTranslates();

            var serializer = new DataContractJsonSerializer(typeof(IDictionary<string, IDictionary<string, string>>));
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(source));
            try
            {
                var translates = (IDictionary<string, IDictionary<string, string>>) serializer.ReadObject(stream);
                return LoadFromDictionary(translates);
            }
            catch
            {
                return NewEmptyTranslates();
            }
        }

        /// <summary>
        ///     Carregar as traduções.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        /// <param name="resource">Nome do recurso.</param>
        /// <returns>Traduções carregadas.</returns>
        public IDictionary<string, IDictionary<string, string>>? LoadFromResource(Assembly assembly, string resource)
        {
            if (assembly == null) return NewEmptyTranslates();

            var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(a => a == resource);
            if (resourceName == null) return NewEmptyTranslates();

            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException();

            using var streamReader = new StreamReader(stream);
            return LoadFromText(streamReader.ReadToEnd());
        }

        /// <summary>
        ///     Retorna um dicionário vazio para comportar traduções.
        /// </summary>
        /// <returns>Nova instância.</returns>
        private static IDictionary<string, IDictionary<string, string>> NewEmptyTranslates()
        {
            return new Dictionary<string, IDictionary<string, string>>();
        }
    }
}