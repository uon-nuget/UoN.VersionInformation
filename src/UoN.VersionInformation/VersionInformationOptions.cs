using System;
using System.Collections.Generic;

namespace UoN.VersionInformation;

public class VersionInformationOptions
{
    public Dictionary<Type, IVersionInformationProvider> TypeHandlers { get; set; }
        = VersionInformationService.DefaultTypeHandlers;

    public Dictionary<string, IVersionInformationProvider> KeyHandlers { get; set; }
        = new Dictionary<string, IVersionInformationProvider>();
}