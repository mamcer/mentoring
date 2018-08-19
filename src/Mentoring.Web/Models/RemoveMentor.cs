using System.Collections.Generic;

namespace Mentoring.Web.Models
{
    public class RemoveMentor
    {
        public RemoveMentor()
        {
            MenteeNames = new List<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<string> MenteeNames { get; set; }
    }
}