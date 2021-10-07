using Core;
using Core.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NServiceBus;
using NServiceBus.Extensions.Logging;
using NServiceBus.Logging;
using System;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices(sp => sp.AddSingleton<IHostedService>(new ProceedIfRabbitMqIsAlive("rabbitmq")))
                .UseNServiceBus(c =>
                {
                    LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));

                    var endpointConfiguration = new EndpointConfiguration("playground.api");

                    endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                                         .Settings(new JsonSerializerSettings { Formatting = Formatting.Indented });

                    var rabbitMqHost = c.Configuration["rabbitmqhost"];

                    endpointConfiguration.UseTransport<RabbitMQTransport>()
                                         .ConnectionString($"host={rabbitMqHost}")
                                         .UseConventionalRoutingTopology()
                                         .Routing().RouteToEndpoint(
                                           assembly: typeof(SampleMessage).Assembly,
                                           destination: "playground.service");

                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    endpointConfiguration.SendOnly();

                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
