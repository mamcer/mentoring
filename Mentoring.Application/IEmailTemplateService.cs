using System;
using Mentoring.Core;

namespace Mentoring.Application
{
    public interface IEmailTemplateService
    {
        EmailTemplate GetEmailTemplateByName(string name);

        void SaveEmailTemplate(string name, string content);
    }
}