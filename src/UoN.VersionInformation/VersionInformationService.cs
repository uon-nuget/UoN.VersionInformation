using System;
using System.Threading.Tasks;

namespace UoN.VersionInformation
{
    /// <summary>
    /// A service for getting version information from .NET Object sources,
    /// through configured providers.
    /// </summary>
    public class VersionInformationService
    {
        /// <summary>
        /// Handle the provided version information source.
        /// If the source matches a configured default type handler,
        /// the provider for that type will be used.
        /// If the source is a provider, that provider will be executed.
        /// If the 
        /// </summary>
        /// <param name="source"></param>
        /// <returns>A .NET Object containing version information</returns>
        public static async Task<object> HandleSource(object source)
        {
            throw new NotImplementedException();
        }
    }
}
