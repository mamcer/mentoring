using Mentoring.Core;
using System;

namespace Mentoring.Web.Models
{
    public class UserLogModel
    {
        public User User { get; set; }

        public DateTime Date { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }
    }
}