using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Builder
{
    public static class UonVersionInformationEndpointRouteBuilderExtensions
    {
        private const string DefaultDisplayName = "UoN Version Information";
        private const string DefaultPath = "/version";

        public static IEndpointConventionBuilder MapUonVersionInformation(
            this IEndpointRouteBuilder endpoints,
            string pattern)
        {
            return MapUonVersionInformationCore(endpoints, pattern);
        }

        public static IEndpointConventionBuilder MapUonVersionInformation(
            this IEndpointRouteBuilder endpoints)
        {
            return MapUonVersionInformationCore(endpoints, DefaultPath);
        }

        public static IEndpointConventionBuilder MapUonVersionInformation(
            this IEndpointRouteBuilder endpoints,
            object source)
        {
            return MapUonVersionInformationCore(endpoints, DefaultPath, source);
        }

        public static IEndpointConventionBuilder MapUonVersionInformation(
            this IEndpointRouteBuilder endpoints,
            string pattern,
            object source)
        {
            return MapUonVersionInformationCore(endpoints, pattern, source);
        }

        private static IEndpointConventionBuilder MapUonVersionInformationCore(IEndpointRouteBuilder endpoints,
            string pattern, object? source = null)
        {
            // Default to entry assembly
            source ??= Assembly.GetEntryAssembly();

            var pipeline = endpoints.CreateApplicationBuilder()
                .UseUonVersionInformationCore(source)
                .Build();

            return endpoints.Map(pattern, pipeline).WithDisplayName(DefaultDisplayName);
        }
    }
}