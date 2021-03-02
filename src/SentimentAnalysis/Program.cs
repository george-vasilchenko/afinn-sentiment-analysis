using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentimentAnalysis.Afinn.Configs;
using SentimentAnalysis.Afinn.Services;
using SentimentAnalysis.AzureTextAnalytics.Configs;
using SentimentAnalysis.AzureTextAnalytics.Services;
using SentimentAnalysis.Common.Configs;
using SentimentAnalysis.Common.Services;
using SentimentAnalysis.Configs;
using SentimentAnalysis.Services;
using Serilog;

namespace SentimentAnalysis
{
    internal static class Program
    {
        private static async Task Main()
        {
            var provider = BootstrapApplication();
            Log.Logger.Information("SentimentAnalysis start");

            try
            {
                var appService = provider.GetRequiredService<IAppService>();
                await appService.RunAsync();
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

        private static void RegisterServices(IServiceCollection serviceCollection)
        {
            var configuration = BuildConfiguration();

            serviceCollection.AddSingleton<IAppConfiguration>(new AppConfiguration(configuration));
            serviceCollection.AddTransient<INaturalExpressionsProvider, NaturalExpressionsProvider>();
            serviceCollection.AddTransient<IAppService, AppService>();

            serviceCollection.AddSingleton<IAfinnConfiguration>(new AfinnConfiguration(configuration));
            serviceCollection.AddTransient<ILexiconService, LexiconService>();
            serviceCollection.AddTransient<IAnalysisService, AfinnAnalysisService>();
            serviceCollection.AddTransient<IAnalysisService, AzureTextAnalyticsAnalysisService>();
            serviceCollection.AddSingleton<IAzureTextAnalyticsConfiguration>(new AzureTextAnalyticsConfiguration(configuration));
        }

        private static IConfiguration BuildConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.local.json", true)
                .Build();
    }
}