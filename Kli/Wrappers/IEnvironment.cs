namespace Kli.Wrappers
{
    /// <summary>
    ///     Facade para System.Environment.
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        ///     Retorna um valor de variável de ambiente.
        /// </summary>
        /// <param name="variable">Nome da variável.</param>
        /// <returns>Valor da variável.</returns>
        string? GetEnvironmentVariable(string variable);
    }
}