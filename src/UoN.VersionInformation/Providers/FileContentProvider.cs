using System;
using System.IO;
using System.Threading.Tasks;

namespace UoN.VersionInformation.Providers
{
    public class FileContentProvider : IVersionInformationProvider
    {
        public string FilePath { get; set; }

        public FileContentProvider(string filePath)
        {
            FilePath = filePath;
        }

        public async Task<object> GetVersionInformationAsync()
            => await GetVersionInformationAsync(FilePath);

        public async Task<object> GetVersionInformationAsync(object source)
        {
            using (var reader = File.OpenText((string)source))
                return await reader.ReadToEndAsync();
        }
    }
}
