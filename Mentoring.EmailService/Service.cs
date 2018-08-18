using CrossCutting.Core.Logging;
using CrossCutting.MainModule.IOC;
using Mentoring.Application;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Timers;

namespace Mentoring.EmailService
{
    public partial class Service : ServiceBase
    {
        private readonly Timer _timer;
        private double _pollingInterval = -1;
        private string _host;
        private int _port = -1;
        private string _userName;
        private string _password;
        private bool? _enableSsl;
        private bool? _useDefaultCredentials;
        private readonly ILogManager _logManager;
        private readonly IEmailMessageService _emailMessageService;

        public Service(ILogManager logManager, IEmailMessageService emailMessageService)
        {
            InitializeComponent();
            _timer = new Timer();
            _timer.Elapsed += TimerElapsed;
            _logManager = logManager;
            _emailMessageService = emailMessageService;

            _logManager.DefaultLogger.Information.Write(string.Format("Mentoring Email Service started with interval: {0}", PollingInterval));
        }

        public double PollingInterval
        {
            get
            {
                if (_pollingInterval == -1)
                {
                    _pollingInterval = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["TimerInterval"]);
                }

                return _pollingInterval;
            } 
        }

        public string Host 
        {
            get 
            {
                if (string.IsNullOrEmpty(_host))
                {
                    _host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                }

                return _host;
            } 
        }

        public int Port
        {
            get
            {
                if (_port == -1)
                {
                    _port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
                }

                return _port;
            }
        }
        
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                {
                    _userName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
                }

                return _userName;
            }
        }

        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(_password))
                {
                    _password = System.Configuration.ConfigurationManager.AppSettings["Password"];
                }

                return _password;
            }
        }

        public bool EnableSsl
        {
            get
            {
                if (!_enableSsl.HasValue)
                {
                    _enableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableSsl"]);
                }

                return _enableSsl.Value;
            }
        }

        public bool UseDefaultCredentials
        {
            get
            {
                if (!_useDefaultCredentials.HasValue)
                {
                    _useDefaultCredentials = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                }

                return _useDefaultCredentials.Value;
            }
        }

        protected override void OnStart(string[] args)
        {
            _timer.Interval = PollingInterval;            
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Enabled = false;
            int lastMessageId = -1;
            
            try
            {
                var messages = _emailMessageService.GetPendingEmailMessages().ToList();

                if (messages.Any())
                {
                    using (SmtpClient smtp = new SmtpClient(Host, Port))
                    {
                        smtp.EnableSsl = EnableSsl;
                        smtp.UseDefaultCredentials = UseDefaultCredentials;
                        smtp.Credentials = new NetworkCredential(UserName, Password);

                        foreach (var message in messages)
                        {
                            lastMessageId = message.Id;
                            var mailMessage = new MailMessage(UserName, message.To, message.Subject, message.Message);
                            mailMessage.IsBodyHtml = true;
                            smtp.Send(mailMessage);
                            _emailMessageService.RegisterEmailSent(lastMessageId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (lastMessageId != -1)
                {
                    _emailMessageService.RegisterEmailError(lastMessageId);
                }
                
                _logManager.DefaultLogger.Error.Write("Mentoring.EmailService.TimerElapsed", ex);
            }

            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
            _logManager.DefaultLogger.Information.Write("Mentoring Email Service successfully stopped");
        }
    }
}