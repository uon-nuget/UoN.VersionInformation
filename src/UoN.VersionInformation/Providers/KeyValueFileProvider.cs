using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace UoN.VersionInformation.Providers
{
    /// <summary>
    /// A naive provider which reads a text file line by line
    /// parsing each line into a key and a value.
    /// 
    /// Keys must not contain the Key / Value delimiter.
    /// </summary>
    public class KeyValueFileProvider : FileContentProvider
    {
        public string Delimiter { get; set; }

        public KeyValueFileProvider(
            string filePath = null,
            string delimiter = "=") : base(filePath)
        {
            Delimiter = delimiter;
        }

        public override async Task<object> GetVersionInformationAsync(object source)
        {
            var fileContent = await base.GetVersionInformationAsync((string)source);
            if (fileContent == null) return null;

            using var reader = new StringReader((string)fileContent);
            var result = new Dictionary<string, string>();
            while (await reader.ReadLineAsync() is { } line)
            {
                var parts = line.Split(new[] { '=' }, 2);
                result.Add(parts[0], parts[1]);
            }

            return result;
        }
    }
}