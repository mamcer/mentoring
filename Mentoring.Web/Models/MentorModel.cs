using Mentoring.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mentoring.Core.Enums;

namespace Mentoring.Web.Models
{
    public class MentorModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Nickname")]
        public string NickName { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Topics")]
        public List<TopicModel> Topics { get; set; }

        public string OtherTopic { get; set; }

        [Display(Name = "Specific skills/ topics/ process to mentor")]
        public string MentoringTarget { get; set; }

        [Display(Name = "Current Activity")]
        public string CurrentActivity { get; set; }

        [Display(Name = "Expectations")]
        public string Expectations { get; set; }

        [Display(Name = "Mentor Seniority")]
        public string Seniority { get; set; }
        
        [Display(Name = "Mentor Location")]
        public string Location { get; set; }

        [Display(Name = "Status")]
        public MentorStatus Status { get; set; }

        [Display(Name = "Max Mentees")]
        public int MaxMentees { get; set; }

        [Display(Name = "Meetings Mode")]
        public string MeetingsMode { get; set; }

        [Display(Name = "Seniority Level")]
        public string SeniorityLevel { get; set; }
    }
}