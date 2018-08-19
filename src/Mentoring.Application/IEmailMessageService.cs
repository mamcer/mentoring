using System.Collections.Generic;
using System.Collections.Specialized;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface IEmailMessageService
    {
        IEnumerable<EmailMessage> GetPendingEmailMessages();

        void RegisterEmailError(int id);
        
        void RegisterEmailSent(int id);
        
        void SaveMessage(string to, string templateName, StringDictionary parameters);
    }
}