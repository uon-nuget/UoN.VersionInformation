using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UoN.VersionInformation.Providers;

namespace UoN.VersionInformation
{
    public class VersionInformationService : IVersionInformationService
    {
        public static Dictionary<Type, IVersionInformationProvider> DefaultTypeHandlers
            = new Dictionary<Type, IVersionInformationProvider>
            {
                [typeof(Assembly)] = new AssemblyInformationalVersionProvider()
            };

        public VersionInformationService(VersionInformationOptions options = null)
        {
            TypeHandlers =
            options?.TypeHandlers ?? DefaultTypeHandlers;
            KeyHandlers = options?.KeyHandlers
            ?? new Dictionary<string, IVersionInformationProvider>();
        }

        public Dictionary<Type, IVersionInformationProvider> TypeHandlers { get; }

        public Dictionary<string, IVersionInformationProvider> KeyHandlers { get; }


        public async Task<object> ByKeyAsync(string key, object source = null)
            => await TryExecuteAsync(KeyHandlers[key], source);

        public object ByKey(string key, object source = null)
            => Task.Run(async () => await ByKeyAsync(key, source)).Result;

        public async Task<object> EntryAssemblyAsync()
            => await DefaultTypeHandlers[typeof(Assembly)]
                .GetVersionInformationAsync(Assembly.GetEntryAssembly());

        public object EntryAssembly()
            => Task.Run(async () => await EntryAssemblyAsync()).Result;

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
        }

        public object FromSource(object source, string providerKey = null)
            => Task.Run(async () => await FromSourceAsync(source, providerKey)).Result;

        private static async Task<object> TryExecuteAsync(IVersionInformationProvider provider, object source = null)
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