# UoN.AspNetCore.VersionMiddleware

[![License](https://img.shields.io/badge/licence-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://travis-ci.org/uon-nuget/UoN.AspNetCore.VersionMiddleware.svg?branch=master)](https://travis-ci.org/uon-nuget/UoN.AspNetCore.VersionMiddleware)
[![NuGet](https://img.shields.io/nuget/v/UoN.AspNetCore.VersionMiddleware.svg)](https://www.nuget.org/packages/UoN.AspNetCore.VersionMiddleware/)


## What is it?

This is middleware for ASP.Net Core designed to report on version information related to your project.

We use it at UoN so that we can check the version of a web app wherever it's deployed, without having to display it publicly to users who don't care. This is useful for ensuring testers know which builds they're working with and therefore what fixes to test.

## What are its features?

It exposes the version output of [UoN.VersionInformation](../UoN.VersionInformation/README.md) as JSON data at an http endpoint.

### Middleware Extension Methods

#### Endpoint Routing

It provides four `IEndpointRouteBuilder` Extension methods for you to use in `Startup.Configure()`:

- `app.MapUonVersionInformation()`
  - adds a `/version` endpoint to the ASP.Net Core pipeline.
  - returns `AssemblyInformationalVersion` for the Application's Entry Assembly.
- `app.MapUonVersionInformation(path)`
  - behaves as above but with a custom route path
- `app.MapUonVersionInformation(source)`
  - adds a `/version` endpoint to the ASP.Net Core pipeline.
  - expects a valid source accepted by `VersionInformationService`
- `app.MapUonVersionInformation(path, source)`
  - behaves as above but with a custom route path

#### Terminal Middleware

> [!NOTE]
> Prefer Endpoint routing above unless you know you need Terminal Middleware

It provides two traditional "Terminal Middleware" `IApplicationBuilder` Extension methods for you to use in `Startup.Configure()`:

- `app.UseUonVersionInformation(source)`
    - adds a `/version` route to the ASP.Net Core pipeline.
    - expects a valid source accepted by `VersionInformationService`
    - if `source` is `null` then defaults to using [`UoN.VersionInformation.Providers.AssemblyInformationalVersionProvider`](../UoN.VersionInformation/README.md), which in turn uses `AssemblyInformationalVersion` from the current assembly's metadata.
- `app.UseUonVersionInformation(path, source)`
    - behaves as above but with a custom route path

## Dependencies

The library targets `net5.0` and depends upon the ASP.NET Core app framework package and UoN.VersionInformation.

If you can use ASP.NET Core 5.x or newer, you can use this library.

## Example usage

#### `Startup.cs`

``` csharp
public class Startup
{
  // ...

  public void Configure(IApplicationBuilder app, IHostingEnvironment env)
  {
    // ...
    
    app.UseStaticFiles();

    app.UseRouting();
    
    app.MapUonVersionInformation(); //adds `/version` endpoint
  }
}
```

Refer to the samples for additional usage examples.

## Building from source

We recommend building with the `dotnet` cli, but since the package targets `net5.0` and depends only on ASP.Net Core, you should be able to build it in any tooling that supports those requirements.

- Have the .NET Core SDK 5.0 or newer
- `dotnet build`
- Optionally `dotnet pack`
- Reference the resulting assembly, or NuGet package.

## Contributing

Contributions are unlikely to be needed often as this is a library with a very specific purpose.

If there are issues open, please feel free to make pull requests for them, and they will be reviewed.