using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mentoring.Core.Enums;

namespace Mentoring.Core
{
    [Table("EmailMessage")]
    public class EmailMessage
    {
        public EmailMessage()
        {
            CreationDate = DateTime.Now;
            Status = EmailStatus.Pending;
        }

        public EmailMessage(string to, string subject, string message) : this()
        {
            To = to;
            Subject = subject;
            Message = message;
        }

        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        [MaxLength(150)]
        public string Subject { get; set; }

        [Required]
        public string Message { get; private set; }

        public EmailStatus Status { get; set; }
    }
}