namespace Mentoring.Web.Models
{
    public class UserProfileModel
    {
        public UserModel UserModel { get; set; }

        public bool IsProgramRenewalEnabled { get; set; }

        public bool IsLeaveProgramEnabled { get; set; }
    }
}