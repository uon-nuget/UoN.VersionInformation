using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UoN.VersionInformation
{
    /// <summary>
    /// A service for getting version information from .NET Object sources,
    /// through configured providers.
    /// </summary>
    public interface IVersionInformationService
    {
        Dictionary<string, IVersionInformationProvider> KeyHandlers { get; }
        Dictionary<Type, IVersionInformationProvider> TypeHandlers { get; }

        /// <summary>
        /// Get version information the provider configured to handle the provided key,
        /// optionally passing a version information source object to the provider.
        /// </summary>
        /// <param name="key">The key to identify the configured provider to use.</param>
        /// <param name="source">Optional source object to pass to the provider.</param>
        /// <returns></returns>
        object ByKey(string key, object source = null);

        /// <summary>
        /// Get version information the provider configured to handle the provided key,
        /// optionally passing a version information source object to the provider.
        /// </summary>
        /// <param name="key">The key to identify the configured provider to use.</param>
        /// <param name="source">Optional source object to pass to the provider.</param>
        /// <returns></returns>
        Task<object> ByKeyAsync(string key, object source = null);

        /// <summary>
        /// Gets the AssemblyInformationalVersion of the application's Entry Assembly
        /// </summary>
        /// <returns>The AssemblyInformationalVersion of the application's Entry Assembly</returns>
        object EntryAssembly();

        /// <summary>
        /// Gets the AssemblyInformationalVersion of the application's Entry Assembly
        /// </summary>
        /// <returns>The AssemblyInformationalVersion of the application's Entry Assembly</returns>
        Task<object> EntryAssemblyAsync();

        /// <summary>
        /// Handle the provided version information source.
        /// </summary>
        /// <param name="source">.NET Object to get Version Information from, or a Provider to provide Version Information.</param>
        /// <param name="providerKey">Optional key to specify a configured provider to use on the source.</param>
        /// <returns>A .NET Object containing version information</returns>
        object FromSource(object source, string providerKey = null);

        /// <summary>
        /// Handle the provided version information source.
        /// </summary>
        /// <param name="source">.NET Object to get Version Information from, or a Provider to provide Version Information.</param>
        /// <param name="providerKey">Optional key to specify a configured provider to use on the source.</param>
        /// <returns>A .NET Object containing version information</returns>
        Task<object> FromSourceAsync(object source, string providerKey = null);
    }
}