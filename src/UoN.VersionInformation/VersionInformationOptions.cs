using System;
using System.Collections.Generic;

namespace UoN.VersionInformation
{
    public class VersionInformationOptions
    {
        public Dictionary<Type, IVersionInformationProvider> TypeHandlers;

        public Dictionary<string, IVersionInformationProvider> KeyHandlers;
    }
}
