using System;
using Topshelf;

namespace NServiceBusService
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc = HostFactory.Run(x =>
            {
                x.Service<ServiceInstance>(s =>
                {
                    s.ConstructUsing(name => new ServiceInstance());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("NService bus service instance");
                x.SetDisplayName("NService bus service instance");
                x.SetServiceName("NService bus service instance");
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
