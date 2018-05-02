using System.Threading.Tasks;

namespace UoN.VersionInformation
{
    /// <summary>
    /// Basic Interface for classes which are able to provide version information.
    /// </summary>
    public interface IVersionInformationProvider
    {
        /// <summary>
        /// Gets version information, based on provider's configuration.
        /// </summary>
        /// <returns>An object containing the acquired version information.</returns>
        Task<object> GetVersionInformationAsync();

        /// <summary>
        /// Gets version information from the passed source, based on provider's configuration.
        /// Some providers will not implement this method.
        /// </summary>
        /// <param name="source">The .NET Object to use as the source of the version information.</param>
        /// <returns>An object containing the acquired version information.</returns>
        Task<object> GetVersionInformationAsync(object source);
    }
}
