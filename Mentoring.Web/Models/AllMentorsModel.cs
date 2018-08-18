using System.Collections.Generic;

namespace Mentoring.Web.Models
{
    public class AllMentorsModel
    {
        public IEnumerable<MentorModel> MentorsWithSlots { get; set; }

        public IEnumerable<MentorModel> MentorsWithoutSlots { get; set; }
        
        public IEnumerable<MentorModel> Inactive { get; set; }

        public IEnumerable<MentorModel> PendingApproval { get; set; }

        public IEnumerable<MentorModel> PendingRenewal { get; set; }
    
        public IEnumerable<MentorModel> Rejected { get; set; }
    }
}