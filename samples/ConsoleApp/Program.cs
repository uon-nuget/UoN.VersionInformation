using System;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UoN.VersionInformation;
using UoN.VersionInformation.Providers;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var version = new VersionInformationService();

            // original and best
            Console.WriteLine(await version.EntryAssemblyAsync());

            // assembly using the default type handler
            Console.WriteLine(await version.FromSourceAsync(Assembly.GetEntryAssembly()));

            // a custom string
            Console.WriteLine(await version.FromSourceAsync("Hello"));

            // a custom object
            dynamic obj = new
            {
                Major = 1,
                Minor = 2,
                Codename = "Alhambra",
                TimeStamp = DateTime.UtcNow
            };
            Console.WriteLine(JsonConvert.SerializeObject(
                await version.FromSourceAsync(obj)));

            // a custom provider
            Console.WriteLine(await version.FromSourceAsync(new MyProvider()));

            // With a Keyed Handler
            const string key = "MyKey";
            version.KeyHandlers[key] = new MyProvider();
            // with no source needed
            Console.WriteLine(await version.ByKeyAsync(key));
            // or with a "source"
            Console.WriteLine(await version.FromSourceAsync(null, key));

            // With a Typed Handler
            version.TypeHandlers[typeof(bool)] = new MyProvider();
            Console.WriteLine(await version.FromSourceAsync(true));

            // Aggregating multiple sources
            Console.WriteLine(JsonConvert.SerializeObject(
                await version.FromSourceAsync(new
                {
                    Source1 = await version.ByKeyAsync(key),
                    Source2 = new { Key1 = 1, Key2 = "2" },
                    Source3 = await version.EntryAssemblyAsync()
                })));

            // all the text content from a file
            var fileProvider = new FileContentProvider("filecontent.txt");
            Console.WriteLine(await version.FromSourceAsync(fileProvider));
            version.KeyHandlers.Add("file", fileProvider);
            fileProvider.FilePath = "";
            Console.WriteLine(await version.ByKeyAsync("file", "filecontent.txt"));

            // key value string pairs from a file
            Console.WriteLine(JsonConvert.SerializeObject(
                await version.FromSourceAsync(
                    new KeyValueFileProvider("keyvalue.txt"))));

            // no file (returns null)
            Console.WriteLine(JsonConvert.SerializeObject(
                await version.FromSourceAsync(
                    new KeyValueFileProvider { FileOptional = true })));

            // hiding null properties
            Console.WriteLine(JsonConvert.SerializeObject(
                new
                {
                    Prop1 = "Prop2 should be hidden because it's null",
                    Prop2 = await version.FromSourceAsync(
                        new KeyValueFileProvider { FileOptional = true })
                },
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            // some synchronous tests
            Console.WriteLine(version.EntryAssembly());
            Console.WriteLine(version.FromSource("hello there"));
            Console.WriteLine(JsonConvert.SerializeObject(version.ByKey("file")));

            Console.Read();
        }
    }

    internal class MyProvider : IVersionInformationProvider
    {
        public Task<object> GetVersionInformationAsync()
            => Task.FromResult<object>("Custom Version Output");

        public Task<object> GetVersionInformationAsync(object source)
            => Task.FromResult<object>(source.ToString());
    }
}
