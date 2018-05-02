using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UoN.VersionInformation.Providers;

namespace UoN.VersionInformation
{
    /// <summary>
    /// A service for getting version information from .NET Object sources,
    /// through configured providers.
    /// </summary>
    public class VersionInformationService
    {

        public static Dictionary<Type, IVersionInformationProvider> DefaultTypeHandlers
            = new Dictionary<Type, IVersionInformationProvider>
            {
                [typeof(Assembly)] = new AssemblyInformationalVersionProvider()
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
        public async Task<object> ByKeyAsync(string key, object source = null)
            => await TryExecuteAsync(KeyHandlers[key], source);

        /// <summary>
        /// Gets the AssemblyInformationalVersion of the application's Entry Assembly
        /// </summary>
        /// <returns>The AssemblyInformationalVersion of the application's Entry Assembly</returns>
        public async Task<object> EntryAssemblyAsync()
            => await DefaultTypeHandlers[typeof(Assembly)]
                .GetVersionInformationAsync(Assembly.GetEntryAssembly());

        /// <summary>
        /// Handle the provided version information source.
        /// </summary>
        /// <param name="source">.NET Object to get Version Information from, or a Provider to provide Version Information.</param>
        /// <param name="providerKey">Optional key to specify a configured provider to use on the source.</param>
        /// <returns>A .NET Object containing version information</returns>
        public async Task<object> FromSourceAsync(object source, string providerKey = null)
        {
            // if a key is provided, use the keyed provider
            if (!string.IsNullOrWhiteSpace(providerKey))
                return await ByKeyAsync(providerKey, source);

            // If we've been passed an instance of a provider,
            // just execute it as configured
            if (source is IVersionInformationProvider p)
                return await p.GetVersionInformationAsync();

            // Try and get a typed provider
            foreach (var kv in TypeHandlers)
                if (kv.Key.GetTypeInfo().IsAssignableFrom(source.GetType()))
                    return await TryExecuteAsync(kv.Value, source);

            // else just pass through the object as is
            return source;

            //return TypeHandlers.TryGetValue(source.GetType().GetTypeInfo()., out var provider)
            //    ? await TryExecuteAsync(provider, source) // if we get one, execute it
            //    : source; // else just pass through the object as is
        }

        private async Task<object> TryExecuteAsync(IVersionInformationProvider provider, object source = null)
        {
            try
            {
                return source == null
                    ? await provider.GetVersionInformationAsync()
                    : await provider.GetVersionInformationAsync(source);
            }
            // in case we accidentally pass a source to a provider that doesn't take one
            catch (NotImplementedException)
            {
                return await provider.GetVersionInformationAsync();
            }
        }
    }
}
