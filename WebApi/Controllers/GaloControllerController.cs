using Core.Messages;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NServiceBus;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GaloController : ControllerBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMessageSession messageSession;

        public GaloController(IMessageSession messageSession) 
        {
            this.messageSession = messageSession;
        }

        [HttpGet]
        public async Task<bool> Get()
        {
            logger.Info("Daar gaat ie dan!");
            
            await messageSession.Send(new SampleMessage());
            
            return true;
        }
    }
}
