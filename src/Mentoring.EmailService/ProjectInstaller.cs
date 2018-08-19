namespace Mentoring.EmailService
{
    using System.ComponentModel;
    using System.ServiceProcess;

    [RunInstaller(true)]
    public class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceInstaller _serviceInstaller;

        private ServiceProcessInstaller _serviceProcessInstaller;

        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            _serviceInstaller = new ServiceInstaller();
            _serviceProcessInstaller = new ServiceProcessInstaller();

            // serviceInstaller
            _serviceInstaller.ServiceName = "MentoringEmailSvc";
            _serviceInstaller.DisplayName = "Mentoring Email Service";
            _serviceInstaller.Description = "Mentoring Email Service. A windows service that look for email messages queued in the application and send them.";
            _serviceInstaller.StartType = ServiceStartMode.Automatic;

            // serviceProcessInstaller
            _serviceProcessInstaller.Password = null;
            _serviceProcessInstaller.Username = null;

            // ProjectInstaller
            Installers.AddRange(new System.Configuration.Install.Installer[]
                                         {
                                             _serviceProcessInstaller,
                                             _serviceInstaller
                                         });
        }
    }
}