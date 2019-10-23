namespace Kli.Infrastructure
{
    /// <summary>
    /// Cache simples para valores
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Grava um valor no cache.
        /// </summary>
        /// <param name="key">Identificador.</param>
        /// <param name="value">Valor.</param>
        /// <typeparam name="T">Tipo de valor.</typeparam>
        /// <returns>O mesmo valor é retornado.</returns>
        T Save<T>(string key, T value);

        /// <summary>
        /// LÊ um valor do cache.
        /// </summary>
        /// <param name="key">Identificador.</param>
        /// <typeparam name="T">Tipo de valor.</typeparam>
        /// <returns>Valor.</returns>
        T Read<T>(string key);
    }
}