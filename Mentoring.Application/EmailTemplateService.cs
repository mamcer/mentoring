using System;
using System.Linq;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public EmailTemplateService(IUnitOfWork unitOfWork, ILogManager logManager)
        {
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public void SaveEmailTemplate(string name, string content)
        {
            try
            {
                _unitOfWork.EmailTemplateRepository.Insert(new EmailTemplate
                {
                    Name = name,
                    Content = content
                });

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.EmailTemplateService.SaveEmailTemplate", ex);
            }
        }

        public EmailTemplate GetEmailTemplateByName(string name)
        {
            try
            {
                var emailTemplate = _unitOfWork.EmailTemplateRepository.Search(m => m.Name == name).FirstOrDefault();
                return emailTemplate;
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.EmailTemplateService.GetEmailTemplateByName", ex);
                return null;
            }
        }
    }
}