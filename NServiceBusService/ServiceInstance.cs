using Core.Messages;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NServiceBus;
using NServiceBus.Extensions.Logging;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace NServiceBusService
{
    public class ServiceInstance
    {
        public void Start() 
        {
            StartEndpoint();
        }

        public void Stop() 
        {  
        }

        public async Task StartEndpoint()
        {
            LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));
 
            var endpointConfiguration = new EndpointConfiguration("playground.service");
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
                                 .Settings(new JsonSerializerSettings { Formatting = Formatting.Indented });

            endpointConfiguration.UseTransport<RabbitMQTransport>()
                                 .ConnectionString("amqp://localhost")
                                 .UseConventionalRoutingTopology()
                                 .Routing().RouteToEndpoint(
                                   assembly: typeof(SampleMessage).Assembly,
                                   destination: "playground.service");

            endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.EnableInstallers();

            await Endpoint.Start(endpointConfiguration);
        }
    }
}
