using System.Reflection;
using System.Threading.Tasks;

namespace UoN.VersionInformation.Providers
{
    /// <summary>
    /// Gets version information from a .NET Assembly's `AssemblyInformationalVersion` metadata.
    /// </summary>
    public class AssemblyInformationalVersionProvider : IVersionInformationProvider
    {
        public Assembly VersionAssembly { get; set; }

        /// <summary>
        /// Create a version provider for a given .NET Assembly.
        /// </summary>
        /// <param name="versionAssembly">
        /// Optional .NET Assembly to get `AssemblyInformationalVersion` from.
        /// Defaults to Entry Assembly.
        /// </param>
        public AssemblyInformationalVersionProvider(Assembly versionAssembly = null)
        {
            VersionAssembly = versionAssembly ?? Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// Gets version information from a .NET Assembly's `AssemblyInformationalVersion` metadata.
        /// </summary>
        /// <returns>`AssemblyInformationalVersion` as a string.</returns>
        public Task<object> GetVersionInformationAsync()
            => GetVersionInformationAsync(VersionAssembly);

        /// <summary>
        /// Gets version information from a .NET Assembly's `AssemblyInformationalVersion` metadata.
        /// </summary>
        /// <param name="source">The .NET Assembly to get version information from.</param>
        /// <returns>`AssemblyInformationalVersion` as a string.</returns>
        public Task<object> GetVersionInformationAsync(object source)
            => Task.FromResult<object>(
                ((Assembly) source)
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion);
    }
}