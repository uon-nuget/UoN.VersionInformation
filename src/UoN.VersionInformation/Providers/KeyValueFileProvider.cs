using System;
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
            string filePath,
            string delimiter = "=")
            : base(filePath)
        {
            Delimiter = delimiter;
        }

        public override async Task<object> GetVersionInformationAsync(object source)
        {
            using (var reader = File.OpenText((string)source))
            {
                var result = new Dictionary<string, string>();
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    result.Add(parts[0], parts[1]);
                }
                return result;
            }
        }
    }
}
