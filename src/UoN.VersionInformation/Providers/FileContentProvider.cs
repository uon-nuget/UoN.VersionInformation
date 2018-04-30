using System;
using System.IO;
using System.Threading.Tasks;

namespace UoN.VersionInformation.Providers
{
    /// <summary>
    /// A naive provider of version information that simply reads
    /// all of the text content in a file and returns it as a string.
    /// 
    /// Can be used as a base class for other File providers.
    /// </summary>
    public class FileContentProvider : IVersionInformationProvider
    {
        public string FilePath { get; set; }

        public FileContentProvider(string filePath)
        {
            FilePath = filePath;
        }

        public virtual async Task<object> GetVersionInformationAsync()
            => await GetVersionInformationAsync(FilePath);

        public virtual async Task<object> GetVersionInformationAsync(object source)
        {
            using (var reader = File.OpenText((string)source))
                return await reader.ReadToEndAsync();
        }
    }
}
