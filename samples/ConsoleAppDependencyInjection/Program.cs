using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using UoN.VersionInformation;
using UoN.VersionInformation.Providers;


// Default options
var services = ConfigureServices_NoOptions(new ServiceCollection());

var version = services.GetRequiredService<VersionInformationService>();

Console.WriteLine(await version.EntryAssemblyAsync());

// Options object
services = ConfigureServices_OptionsObject(new ServiceCollection());

version = services.GetRequiredService<VersionInformationService>();

Console.WriteLine(await version.ByKeyAsync("Assembly"));

// Options Delegate
services = ConfigureServices_OptionsDelegate(new ServiceCollection());

version = services.GetRequiredService<VersionInformationService>();

Console.WriteLine(await version.ByKeyAsync("Assembly"));

Console.ReadKey();


static IServiceProvider ConfigureServices_NoOptions(IServiceCollection services)
{
    services.AddVersionInformation();
    return services.BuildServiceProvider();
}

static IServiceProvider ConfigureServices_OptionsObject(IServiceCollection services)
{
    var options = new VersionInformationOptions
    {
        KeyHandlers = new Dictionary<string, IVersionInformationProvider>
        {
            ["Assembly"] = new AssemblyInformationalVersionProvider()
        }
    };

    services.AddVersionInformation(options);
    return services.BuildServiceProvider();
}

static IServiceProvider ConfigureServices_OptionsDelegate(IServiceCollection services)
{
    services.AddVersionInformation(opts =>
    {
        opts.KeyHandlers.Add("Assembly", new AssemblyInformationalVersionProvider());
    });
    return services.BuildServiceProvider();
}