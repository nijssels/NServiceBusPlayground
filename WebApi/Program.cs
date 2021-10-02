using Core.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NServiceBus;
using NServiceBus.Extensions.Logging;
using NServiceBus.Logging;

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
                .UseNServiceBus(c =>
                {
                    LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));

                    var endpointConfiguration = new EndpointConfiguration("playground.api");

                    endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                                         .Settings(new JsonSerializerSettings { Formatting = Formatting.Indented });

                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    transport.Routing().RouteToEndpoint(
                        assembly: typeof(SampleMessage).Assembly,
                        destination: "playground.service");

                    endpointConfiguration.UsePersistence<LearningPersistence>();

                    endpointConfiguration.SendOnly();

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
