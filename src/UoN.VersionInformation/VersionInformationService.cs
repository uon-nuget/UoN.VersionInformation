using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace UoN.VersionInformation
{
    /// <summary>
    /// A service for getting version information from .NET Object sources,
    /// through configured providers.
    /// </summary>
    public class VersionInformationService
    {
        public static Dictionary<Type, IVersionInformationProvider> DefaultTypeHandlers
        => new Dictionary<Type, IVersionInformationProvider>
        {
            [typeof(Assembly)] = null // TODO adopt default Assembly provider
        };
        public Dictionary<Type, IVersionInformationProvider> TypeHandlers { get; }
        public Dictionary<string, IVersionInformationProvider> KeyHandlers { get; }

        public VersionInformationService(VersionInformationOptions options = null)
        {
            TypeHandlers = options?.TypeHandlers ?? DefaultTypeHandlers;
            KeyHandlers = options?.KeyHandlers
                           ?? new Dictionary<string, IVersionInformationProvider>();
        }

        /// <summary>
        /// Get version information the provider configured to handle the provided key,
        /// optionally passing a version information source object to the provider.
        /// </summary>
        /// <param name="key">The key to identify the configured provider to use.</param>
        /// <param name="source">Optional source object to pass to the provider.</param>
        /// <returns></returns>
        public Task<object> ByKey(string key, object source = null)
        {
            try
            {
                return source == null
                    ? KeyHandlers[key].GetVersionInformationAsync()
                    : KeyHandlers[key].GetVersionInformationAsync(source);
            }
            // in case we accidentally pass a source to a provider that doesn't take one
            catch (NotImplementedException)
            {
                return KeyHandlers[key].GetVersionInformationAsync();
            }
        }

        /// <summary>
        /// Handle the provided version information source.
        /// </summary>
        /// <param name="source">.NET Object to get Version Information from, or a Provider to provide Version Information.</param>
        /// <param name="providerKey">Optional key to specify a configured provider to use on the source.</param>
        /// <returns>A .NET Object containing version information</returns>
        public async Task<object> FromSource(object source, string providerKey = null)
        {
            // if a key is provided, use the keyed provider
            if (!string.IsNullOrWhiteSpace(providerKey))
                return ByKey(providerKey, source);

            // If we've been passed an instance of a provider,
            // just execute it as configured
            if (source is IVersionInformationProvider p)
                return await p.GetVersionInformationAsync();

            // Try and get a typed provider
            return TypeHandlers.TryGetValue(source.GetType(), out var provider)
                ? provider.GetVersionInformationAsync(source) // if we get one, execute it
                : source; // else just pass through the object as is
        }
    }
}
