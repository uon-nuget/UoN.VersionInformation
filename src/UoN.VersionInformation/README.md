# UoN.VersionInformation

[![Build Status]][Build Workflow]
[![NuGet Badge]][NuGet Package]
[![MIT License Badge]][MIT License]

[Build Workflow]: https://github.com/uon-nuget/UoN.VersionInformation/actions/workflows/build.versioninformation.yml
[Build Status]: https://github.com/uon-nuget/UoN.VersionInformation/actions/workflows/build.versioninformation.yml/badge.svg
[NuGet Package]: https://www.nuget.org/packages/UoN.VersionInformation/
[NuGet Badge]: https://img.shields.io/nuget/v/UoN.VersionInformation.svg

[MIT License]: https://opensource.org/licenses/MIT
[MIT License Badge]: https://img.shields.io/badge/licence-MIT-blue.svg

# What is it?

A pluggable service for retrieving version information from .NET Object types.

Really it just provides a simple system for
  - optionally taking an input object
  - executing *something* that returns a simple data object, possibly based on the input object
  - returning the result.
  
We just happen to use it for version information.

It includes some basic providers which don't add dependencies beyond the `NetStandard.Library`.

## Dependency Injection

For Dependency Injection in a .NET Core `IServicesCollection` compatible environment,
see [UoN.VersionInformation.DependencyInjection](https://github.com/uon-nuget/UoN.VersionInformation/blob/main/src/UoN.VersionInformation.DependencyInjection/README.md)

# What are its features?

## Version Information Service

This is the core service.

It provides two main methods for use, as well as a specific helper for the most common use case.
It also accepts some configuration.

- The Service can always give you the `AssemblyInformationalVersion` of the current application's Entry Assembly, using `EntryAssemblyAsync()`.
- It can execute a pre-configured Provider (optionally with an input object) identified by a string key using `ByKeyAsync()`.
- It can execute arbitrary Providers passed to it, or pre-configured Providers assigned to handle specific .NET Types using `FromSourceAsync()`.

## Version Information Providers

The package provides three basic implementations of `IVersionInformationProvider`.

Further implementations are encouraged, based on where you want to get version information from, and the output object structure you would like it to have.

The implementations in this package only depend on `NetStandard.Library`.

### AssemblyInformationalVersionProvider

This provides the behaviour of `EntryAssemblyAsync()`. It is also by default configured as the Type Handler for .NET `Assembly` objects.

It simply gets the `AssemblyInformationalVersion` of a given .NET Assembly and returns it as a string.

You're unlikely to use this directly because you can leave it configured as a Type Handler, and simply pass assemblies directly to `FromSourceAsync()`.

```csharp
// this service usage:
service.EntryAssemblyAsync();
service.FromSourceAsync(MyAssembly);

// is equivalent to
service.FromSourceAsync(new AssemblyInformationalVersionProvider());
service.FromSourceAsync(new AssemblyInformationalVersionProvider(MyAssembly));
```

### FileContentProvider

This is a very basic Provider which reads all the content from a file, and returns it as a string. It's suitable for use as a base class by other file-based Providers.

If you wish to use it, it can be useful to configure it either as the default Type Handler for `string`s, or to configure it as a Key Handler.

```csharp
// this is fine
service.FromSourceAsync(new FileContentProvider("path/to/file.txt"));
service.FromSourceAsync("my string"); // this still passes through the string value

// this is simpler if you want to use it on multiple files later
service.TypeHandlers.Add(typeof(string), new FileContentProvider());
service.FromSourceAsync("path/to/file.txt"); // GOOD - now sent to the FileContentProvider
service.FromSourceAsync("my string"); // BAD - also sent to the provider and assumed to be a file path...

// this is safer than above
service.KeyHandlers.Add("file", new FileContentProvider()); // maybe use enums for keys in your code ;)
service.ByKeyAsync("file", "path/to/file.txt");
service.FromSourceAsync("my string"); //absolutely fine
```

### KeyValueFileProvider

This is a very naive provider for reading Key Value pairs from text files.

It is recommended to use a Provider for a real file format such as JSON, YAML, INI etc...

This implementation is here for convenience and requires no dependencies outside of `NetStandard.Library`.

Each line of the file is considered a new Key Value pair, The first `=` encountered splits the key from the value. That's it.

Keys cannot contain delimiters (obviously) but values can.

The delimiter is configurable, but defaults to `=`.

```csharp
// File:
// -----
// Key 1:=Value
// Key2=Value:With=

var provider = new KeyValueFileProvider("path/to/file.txt");
// { "Key 1:": "Value", "Key2": "Value:With=" }
version.FromSourceAsync(provider);

provider.Delimiter = ":";
// { "Key 1": "=Value", "Key2=Value": "With=" }
version.FromSourceAsync(provider);
```

# Dependencies

The library targets `netstandard1.5`, due to its use of `Assembly.GetEntryAssembly()`.

This still enables use in applications targeting `.NET 4.61` and newer, and `.NET Core 1.0`.

For full implementation support see [here](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

# Building from source

We recommend building with the `dotnet` cli, but since the package targets `netstandard1.5` and depends only on `NetStandard.Library`, you should be able to build it in any tooling that supports those requirements.

- Have the .NET Core SDK
- `dotnet build`
- Optionally `dotnet pack`
- Reference the resulting assembly, or NuGet package.

# Contributing

If you make useful providers that depend only on `NetStandard.Library`, raise a PR and they can be added to the base package.

If there are issues open, please feel free to make pull requests for them, and they will be reviewed.