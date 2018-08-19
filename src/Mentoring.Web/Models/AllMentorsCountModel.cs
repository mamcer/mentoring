namespace Mentoring.Web.Models
{
    public class AllMentorsCountModel
    {
        public int MentorsWithSlots { get; set; }

        public int MentorsWithoutSlots { get; set; }
        
        public int Inactive { get; set; }

        public int PendingApproval { get; set; }

        public int PendingRenewal { get; set; }
    
        public int Rejected { get; set; }
    }
}