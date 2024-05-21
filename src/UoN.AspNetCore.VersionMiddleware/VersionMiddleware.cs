using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UoN.VersionInformation;
using UoN.VersionInformation.Providers;

namespace UoN.AspNetCore.VersionMiddleware
{
    /// <summary>
    /// ASP.NET Core middleware for providing a GET endpoint
    /// which returns version information.
    /// </summary>
    public class VersionMiddleware
    {
        private readonly object _versionSource;

        /// <summary>
        /// Construct an instance of the middleware.
        /// </summary>
        /// <param name="next">See ASP.NET Core Docs.</param>
        /// <param name="versionSource">
        /// An object that version information can be gotten from.
        /// 
        /// See UoN.VersionInformation Docs.
        /// 
        /// Defaults to Entry Assembly Informational Version.
        /// </param>
        public VersionMiddleware(RequestDelegate next, object versionSource = null)
        {
            _versionSource = versionSource ??
                             new AssemblyInformationalVersionProvider(Assembly.GetEntryAssembly());
        }

        public async Task Invoke(HttpContext context)
        {
            // make it clear that we're always returning json, even if it's just a string
            context.Response.ContentType = "application/json";

            var service =
                // try and get a version information service by DI
                (VersionInformationService)
                context.RequestServices.GetService(
                    typeof(VersionInformationService))
                // else use a new one with default options
                ?? new VersionInformationService();

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(await service.FromSourceAsync(_versionSource),
                    new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }));
        }
    }
}