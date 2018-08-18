namespace Mentoring.Web.Models
{
    public class ProgramStatusInfo
    {
        public bool ProgramInProgress { get; set; }

        public bool ProgramRenewal { get; set; }

        public bool MentorPendingRenewal { get; set; }

        public bool MenteePendingRenewal { get; set; }
    }
}