using System.Web.Mvc;
using CrossCutting.MainModule.IOC;
using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace Mentoring.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            var unityContainer = new IocUnityContainer(container);
           
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}