using System;
using Mentoring.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mentoring.Core.Test
{
    [TestClass]
    public class EmailMessageTest
    {
        [TestMethod]
        public void ConstructorWithParametersShouldSetProperties()
        {
            // Arrange
            EmailMessage emailMessage;
            string to = "mario@company.com";
            string subject = "hello";
            string message = "hello there";

            // Act
            emailMessage = new EmailMessage(to, subject, message);

            // Assert
            Assert.AreEqual(to, emailMessage.To);
            Assert.AreEqual(subject, emailMessage.Subject);
            Assert.AreEqual(message, emailMessage.Message);
            Assert.AreEqual(DateTime.Now.Year, emailMessage.CreationDate.Year);
            Assert.AreEqual(DateTime.Now.Month, emailMessage.CreationDate.Month);
            Assert.AreEqual(DateTime.Now.Year, emailMessage.CreationDate.Year);
            Assert.AreEqual(EmailStatus.Pending, emailMessage.Status);
        }

        [TestMethod]
        public void ConstructorWithoutParametersShouldSetCreationDateAndStatus()
        {
            // Arrange
            EmailMessage emailMessage;
            
            // Act
            emailMessage = new EmailMessage();

            // Assert
            Assert.AreEqual(DateTime.Now.Year, emailMessage.CreationDate.Year);
            Assert.AreEqual(DateTime.Now.Month, emailMessage.CreationDate.Month);
            Assert.AreEqual(DateTime.Now.Year, emailMessage.CreationDate.Year);
            Assert.AreEqual(EmailStatus.Pending, emailMessage.Status);
        }
    }
}
