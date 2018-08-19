using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class EmailTemplateTest
    {
        [TestMethod]
        public void ApplyTemplateWithNullParametersShouldReturnTemplate()
        {
            // Arrange
            var content = "<html><body><h1>Basic Email Template</h1><p>Lorem ipsum dolor sit amet</p></body></html>";
            var emailTemplate = new EmailTemplate
                {
                    Content = content
                };
            string emailBody;

            // Act
            emailBody = emailTemplate.ApplyTemplate(null);

            // Assert
            Assert.AreEqual(content, emailBody);
        }

        [TestMethod]
        public void ApplyTemplateWithParametersShouldReturnTemplateWithParametersApplied()
        {
            // Arrange
            var content = "<html><body><h1>Basic Email Template</h1><p>@@body</p></body></html>";
            var emailTemplate = new EmailTemplate
            {
                Content = content
            };
            string emailBody;
            var body = "Lorem ipsum dolor sit amet";
            var parameters = new StringDictionary();
            parameters.Add("@@body", body);
            
            // Act
            emailBody = emailTemplate.ApplyTemplate(parameters);

            // Assert
            Assert.AreEqual(content.Replace("@@body", body), emailBody);
        }
    }
}
