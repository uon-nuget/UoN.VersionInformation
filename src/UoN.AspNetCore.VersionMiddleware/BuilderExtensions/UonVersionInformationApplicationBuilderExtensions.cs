﻿using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using UoN.AspNetCore.VersionMiddleware;
using UoN.VersionInformation.Providers;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to make using the VersionMiddleware easier.
    /// </summary>
    public static class UonVersionInformationApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a custom endpoint to the pipeline,
        /// responding only to GET requests with the output
        /// from the specified version source.
        /// </summary>
        /// <param name="app">The `IApplicationBuilder` instance from `Startup.Configure()`.</param>
        /// <param name="path">The endpoint path to use.</param>
        /// <param name="source">
        /// An optional source supported by the configured VersionInformationService.
        /// 
        /// Defaults to Entry Assembly Informational Version.
        /// </param>
        /// <returns>The `IApplicationBuilder` instance</returns>
        public static IApplicationBuilder UseUonVersion(
            this IApplicationBuilder app,
            PathString path,
            object? source = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            // Default to entry assembly
            source ??= Assembly.GetEntryAssembly();

            // switch on what we've been passed
            // to choose appropriate shortcut behaviours
            return source switch
            {
                // If we've been passed an assembly, explicitly use an assembly provider
                // This is a good safety in case Assembly Type Handlers have been cleared
                // from the configured service.
                Assembly a => app.Map(path,
                    appBuilder =>
                        appBuilder.UseMiddleware<VersionMiddleware>(new AssemblyInformationalVersionProvider(a))),
                _ => app.Map(path,
                    appBuilder => appBuilder.UseMiddleware<VersionMiddleware>(source ?? Array.Empty<object>()))
            };
        }

        /// <summary>
        /// Adds the endpoint `/version` to the pipeline,
        /// responding only to GET requests with the output
        /// from the specified version source.
        /// </summary>
        /// <param name="app">The `IApplicationBuilder` instance from `Startup.Configure()`.</param>
        /// <param name="source">
        /// An optional source supported by the configured VersionInformationService.
        /// 
        /// Defaults to Entry Assembly Informational Version.
        /// </param>
        /// <returns>The `IApplicationBuilder` instance</returns>
        public static IApplicationBuilder UseUonVersion(
            this IApplicationBuilder app,
            object? source = null)
            => app.UseUonVersion("/version", source)
               ?? throw new ArgumentNullException(nameof(app));
    }
}