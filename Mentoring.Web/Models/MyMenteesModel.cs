using System.Collections.Generic;

namespace Mentoring.Web.Models
{
    public class MyMenteesModel
    {
        public IEnumerable<MenteeModel> ActiveMentees { get; set; }

        public IEnumerable<MenteeModel> PendingMentees { get; set; }

        public IEnumerable<MenteeModel> AcceptedMentees { get; set; }

        public IEnumerable<MenteeModel> RejectedMentees { get; set; }
    }
}