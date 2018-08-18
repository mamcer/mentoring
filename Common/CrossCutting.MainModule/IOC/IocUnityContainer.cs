using System;
using System.Configuration;
using System.Diagnostics;
using CrossCutting.Core.IOC;
using CrossCutting.Core.Logging;
using CrossCutting.MainModule.Logging;
using Mentoring.Application;
using Mentoring.Data;
using Microsoft.Practices.Unity;

namespace CrossCutting.MainModule.IOC
{
    public class IocUnityContainer : IContainer
    {
        private static UnityContainer _unityContainer;

        public IocUnityContainer() : this(new UnityContainer())
        {}

        public IocUnityContainer(UnityContainer container)
        {
            _unityContainer = container;
            RegisterTypes();
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public static void RegisterTypes()
        {
            bool realContainer = true;
            if (ConfigurationManager.AppSettings["IocRealContainer"] != null)
            {
                if (bool.TryParse(ConfigurationManager.AppSettings["IocRealContainer"], out realContainer) == false)
                {
                    realContainer = true;
                }
            }

            if (realContainer)
            {
                RegisterRealTypes();
            }
        }

        private static void RegisterRealTypes()
        {
            _unityContainer.RegisterType<ILogManager, LogManager>();
            _unityContainer.RegisterType<IApplicationLogger, ApplicationLogger>();
            _unityContainer.RegisterType<ILogWriter, MelLogWriter>(new InjectionConstructor(TraceEventType.Information));
            _unityContainer.RegisterType<IEmailMessageService, EmailMessageService>();
            _unityContainer.RegisterType<IEmailTemplateService, EmailTemplateService>();
            _unityContainer.RegisterType<IMenteeSeniorityService, MenteeSeniorityService>();
            _unityContainer.RegisterType<IMenteeService, MenteeService>();
            _unityContainer.RegisterType<IMentorService, MentorService>();
            _unityContainer.RegisterType<IProgramStatusService, ProgramStatusService>();
            _unityContainer.RegisterType<ITimeSlotService, TimeSlotService>();
            _unityContainer.RegisterType<ITopicService, TopicService>();
            _unityContainer.RegisterType<IUserLogService, UserLogService>();
            _unityContainer.RegisterType<IUserRoleService, UserRoleService>();
            _unityContainer.RegisterType<IUserService, UserService>();

            _unityContainer.RegisterType<IUnitOfWork, UnitOfWork>();
        }
    }
}