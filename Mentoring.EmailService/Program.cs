using System.ServiceProcess;
using CrossCutting.Core.Logging;
using CrossCutting.MainModule.IOC;
using Mentoring.Application;

namespace Mentoring.EmailService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            var unityContainer = new IocUnityContainer();
            ServicesToRun = new ServiceBase[] 
            { 
                new Service(unityContainer.Resolve<ILogManager>(), unityContainer.Resolve<IEmailMessageService>()) 
            };

            ServiceBase.Run(ServicesToRun);
        }
    }
}