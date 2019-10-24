namespace Kli.Wrappers
{
    /// <summary>
    /// Facade para System.Environment.
    /// </summary>
    public class Environment : IEnvironment
    {
        /// <summary>
        /// Retorna um valor de variável de ambiente.
        /// </summary>
        /// <param name="variable">Nome da variável.</param>
        /// <returns>Valor da variável.</returns>
        public string? GetEnvironmentVariable(string variable) => System.Environment.GetEnvironmentVariable(variable);
    }
}