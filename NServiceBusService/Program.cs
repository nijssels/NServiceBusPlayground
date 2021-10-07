using Core.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NServiceBus;
using NServiceBus.Extensions.Logging;
using NServiceBus.Logging;
using System;

namespace NServiceBusService
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .UseNServiceBus(ctx =>
                {
                    LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));

                    var endpointConfiguration = new EndpointConfiguration("playground.service");
                    endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                                         .Settings(new JsonSerializerSettings { Formatting = Formatting.Indented });

                    var rabbitMqHost = ctx.Configuration["rabbitmqhost"];

                    endpointConfiguration.UseTransport<RabbitMQTransport>()
                                         .ConnectionString($"host={rabbitMqHost}")
                                         .UseConventionalRoutingTopology()
                                         .Routing().RouteToEndpoint(
                                           assembly: typeof(SampleMessage).Assembly,
                                           destination: "playground.service");

                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                ;
        }
    }
}
