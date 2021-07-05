using CashMastersPOSCore.Interfaces;
using CashMastersPOSCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

using Serilog;


namespace CashMastersPOS
{
    public class Startup
    {
        private IConfiguration configuration;        
        private readonly IServiceProvider serviceProvider;

        //Access the built service pipeline
        public IServiceProvider ServiceProvider => serviceProvider;
        //Access the built configuration
        public IConfiguration Configuration => configuration;

        public Startup()
        {            
            var services = ConfigureServices();
            serviceProvider = services.BuildServiceProvider();            
        }


        private IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();            

            // add services                       
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<ICashMastersService, CashMastersService>();

            //Logs configuration.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddSerilog();
            });

            return services;
        }
    }
}
