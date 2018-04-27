using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UoN.VersionInformation;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var version = new VersionInformationService();

            // original and best
            Console.WriteLine(await version.EntryAssemblyAsync());

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
            Console.WriteLine(JsonConvert.SerializeObject(obj));
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
                    Source2 = new {Key1 = 1, Key2 = "2"},
                    Source3 = await version.EntryAssemblyAsync()
                })));

            Console.Read();
        }
    }

    internal class MyProvider : IVersionInformationProvider
    {
        public async Task<object> GetVersionInformationAsync()
            => "Custom Version Output";

        public async Task<object> GetVersionInformationAsync(object source)
            => source.ToString();
    }
}
