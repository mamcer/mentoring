namespace Mentoring.Web.Models
{
    public class AllMenteesCountModel
    {
        public int MenteesWithMentor { get; set; }

        public int MenteesWithoutMentor { get; set; }

        public int PendingApproval { get; set; }

        public int Inactive { get; set; }

        public int PendingRenewal { get; set; }        
        
        public int Rejected { get; set; }
    }
}