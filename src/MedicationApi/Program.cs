namespace MedicationApi
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public static class Program
    {
        public static Task Main(string[] args)
        {
            return CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("conf/appsettings.json")
                .Build();

            return Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseDefaultServiceProvider(options => options.ValidateOnBuild = false)
                        .UseConfiguration(config)
                        .UseStartup<Startup>();
                });
        }
    }
}
