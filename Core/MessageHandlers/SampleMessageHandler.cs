using Core.Messages;
using NLog;
using NServiceBus;
using System.Threading.Tasks;

namespace Core.MessageHandlers
{
    public class SampleMessageHandler : IHandleMessages<SampleMessage>
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public Task Handle(SampleMessage message, IMessageHandlerContext context)
        {
            logger.Info("Daar is ie dan!");

            return Task.CompletedTask;
        }
    }
}
