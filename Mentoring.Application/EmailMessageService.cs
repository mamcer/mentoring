using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using CrossCutting.Core.Logging;
using Mentoring.Core;
using Mentoring.Core.Enums;
using Mentoring.Data;

namespace Mentoring.Application
{
    public class EmailMessageService : Mentoring.Application.IEmailMessageService
    {
        private IEmailTemplateService _emailTemplateService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogManager _logManager;

        public EmailMessageService(IUnitOfWork unitOfWork, ILogManager logManager, IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
            _unitOfWork = unitOfWork;
            _logManager = logManager;
        }

        public IEnumerable<EmailMessage> GetPendingEmailMessages()
        {
            try
            {
                var messages = _unitOfWork.EmailMessageRepository.Search(m => m.Status == EmailStatus.Pending);
                return messages.ToList();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.EmailMessageService.GetPendingEmailMessages", ex);
                return null;
            }
        }

        public void SaveMessage(string to, string templateName, StringDictionary parameters)
        {
            try
            {
                var template = _emailTemplateService.GetEmailTemplateByName(templateName);
                var message = template.ApplyTemplate(parameters);
                _unitOfWork.EmailMessageRepository.Insert(new EmailMessage(to, template.Subject, message));
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.EmailMessageService.SaveMessage", ex);
            }
        }

        public void RegisterEmailSent(int id)
        {
            try
            {
                var message = _unitOfWork.EmailMessageRepository.Get(id);
                message.Status = EmailStatus.Sent;
                _unitOfWork.EmailMessageRepository.Update(message);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.EmailMessageService.RegisterEmailSent", ex);
            }
        }

        public void RegisterEmailError(int id)
        {
            try
            {
                var message = _unitOfWork.EmailMessageRepository.Get(id);
                message.Status = EmailStatus.Error;
                _unitOfWork.EmailMessageRepository.Update(message);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logManager.DefaultLogger.Error.Write("Mentoring.Application.EmailMessageService.RegisterEmailError", ex);
            }
        }
    }
}