using System.Collections.Generic;

namespace Mentoring.Web.Models
{
    public class AllMenteesModel
    {
        public IEnumerable<MenteeModel> MenteesWithMentor { get; set; }

        public IEnumerable<MenteeModel> MenteesWithoutMentor { get; set; }

        public IEnumerable<MenteeModel> PendingApproval { get; set; }

        public IEnumerable<MenteeModel> Inactive { get; set; }

        public IEnumerable<MenteeModel> PendingRenewal { get; set; }        
        
        public IEnumerable<MenteeModel> Rejected { get; set; }
    }
}