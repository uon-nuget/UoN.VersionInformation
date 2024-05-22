using System;
using System.IO;
using System.Threading.Tasks;

namespace UoN.VersionInformation.Providers
{
    /// <summary>
    /// A naive provider of version information that simply reads
    /// all the text content in a file and returns it as a string.
    /// 
    /// Can be used as a base class for other File providers.
    /// </summary>
    public class FileContentProvider : IVersionInformationProvider
    {
        public string FilePath { get; set; }
        public bool FileOptional { get; set; }

        public FileContentProvider(string filePath = null, bool optional = false)
        {
            FilePath = filePath;
            FileOptional = optional;
        }

        public virtual async Task<object> GetVersionInformationAsync()
            => await GetVersionInformationAsync(FilePath);

        public virtual async Task<object> GetVersionInformationAsync(object source)
        {
            if (string.IsNullOrWhiteSpace((string)source)) return null;
            try
            {
                using (var reader = File.OpenText((string)source))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (Exception e) when (e is DirectoryNotFoundException || e is FileNotFoundException)
            {
                if (!FileOptional) throw;
            }

            return null;
        }
    }
}