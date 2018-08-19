using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CrossCutting.Core.Logging;
using CrossCutting.MainModule.IOC;
using Mentoring.Application;
using Mentoring.Web.Models;

namespace Mentoring.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnityConfig.RegisterComponents();
        }

        protected void Session_Start()
        {
            var unityContainer = new IocUnityContainer();
            var logManager = unityContainer.Resolve<ILogManager>();

            try
            {
                var userName = HttpContext.Current.User.Identity.Name;
                logManager.DefaultLogger.Information.Write(string.Format("User {0} logged", userName));
                var userService = unityContainer.Resolve<IUserService>();
                var user = userService.CreateUser(userName, UserHelper.GetDefaultAvatarUrl(HttpContext.Current.Request));

                UserHelper.SetUserInfo(user);
            }
            catch (Exception ex)
            {
                logManager.DefaultLogger.Error.Write("Session_Start", ex);
            }
        }

        protected void Application_Error()
        {
            var unityContainer = new IocUnityContainer();
            var logManager = unityContainer.Resolve<ILogManager>();
            logManager.DefaultLogger.Error.Write("An unhandled Error has occurred", Server.GetLastError());
        }
    }
}