using System;
using System.IO;
using Afinn.Configs;
using Afinn.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Afinn
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var provider = BootstrapApplication();
            Log.Logger.Information("AFINN start");

            try
            {
                var appService = provider.GetRequiredService<IAppService>();
                appService.Run();
            }
            catch (Exception e)
            {
                Log.Error(e, "Something went wrong");
                throw;
            }

            Log.Logger.Information("Press any key to terminate");
            Console.ReadKey();
        }

        private static IServiceProvider BootstrapApplication()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureLogging();
            RegisterServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static IConfiguration BuildConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

        private static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAppConfiguration>(new AppConfiguration(BuildConfiguration()));
            serviceCollection.AddTransient<IAppService, AppService>();
            serviceCollection.AddTransient<ILexiconService, LexiconService>();
            serviceCollection.AddTransient<ICommentsService, CommentsService>();
        }
    }
}