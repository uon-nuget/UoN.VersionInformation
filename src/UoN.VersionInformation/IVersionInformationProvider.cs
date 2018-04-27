using System.Threading.Tasks;

namespace UoN.VersionInformation
{
    /// <summary>
    /// Basic Interface for classes which are able to provide version information.
    /// </summary>
    public interface IVersionInformationProvider
    {
        /// <summary>
        /// Gets version information.
        /// </summary>
        /// <returns>An object containing the acquired version information.</returns>
        Task<object> GetVersionInformationAsync();
    }

    /// <summary>
    /// Typed interface for classes which are able to provide
    /// version information from source objects of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVersionInformationProvider<in T>
    {
        /// <summary>
        /// Gets version information from the passed source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>An object containing the acquired version information.</returns>
        Task<object> GetVersionInformationAsync(T source);
    }
}
