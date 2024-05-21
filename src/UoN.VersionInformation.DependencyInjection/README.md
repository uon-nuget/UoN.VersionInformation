# UoN.VersionInformation.DependencyInjection

[![License](https://img.shields.io/badge/licence-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://travis-ci.org/uon-nuget/UoN.VersionInformation.DependencyInjection.svg?branch=master)](https://travis-ci.org/uon-nuget/UoN.VersionInformation.DependencyInjection)
[![NuGet](https://img.shields.io/nuget/v/UoN.VersionInformation.DependencyInjection.svg)](https://www.nuget.org/packages/UoN.VersionInformation.DependencyInjection/)

# What is it?

`IServiceCollection` extensions for using [UoN.VersionInformation](https://github.com/uon-nuget/UoN.VersionInformation) with .NET Core Dependency Injecton.

# What are its features?

## IServiceCollection Extensions

The package provides 3 extension methods which add a `VersionInformationService` to an `IServiceCollection`.

- One without options
- Two that take options either as an object or a delegate.

Refer to the sample application for example usage.

# Dependencies

The library targets `netstandard2.0` and depends on `Microsoft.Extensions.DependencyInjection` 2.0.0.

# Building from source

We recommend building with the `dotnet` cli, but since the package targets `netstandard2.0` and depends on `Microsoft.Extensions.DependencyInjection` 2.0.0, you should be able to build it in any tooling that supports those requirements.

- Have the .NET Core SDK
- `dotnet build`
- Optionally `dotnet pack`
- Reference the resulting assembly, or NuGet package.

# Contributing

If there are issues open, please feel free to make pull requests for them, and they will be reviewed.