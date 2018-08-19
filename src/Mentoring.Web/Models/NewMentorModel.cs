using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mentoring.Core.Enums;

namespace Mentoring.Web.Models
{
    public class NewMentorModel
    {
        public NewMentorModel()
        {
            Topics = new List<TopicModel>();
            Availability = new List<TimeSlotModel>();
            SeniorityLevel = new List<MenteeSeniorityModel>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage="This field is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mention what technology, area or project you are working on at the moment")]
        public string CurrentActivity { get; set; }

        [Display(Name = "What topics would you like to mentor on?")]
        public List<TopicModel> Topics { get; set; }

        [Display(Name = "Other Topic")]
        public string OtherTopic { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mention the specific skills, topics or process you would like to mentor on")]
        public string MentoringTarget { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Range(1, 3, ErrorMessage = "Must be between 1 and 3")]
        [Display(Name = "Max Number of mentees")]
        public int MaxMentees { get; set; }

        [Display(Name = "What time slot do you think you would usually be available for mentoring meetings? (ART)")]
        public List<TimeSlotModel> Availability { get; set; }

        [Display(Name = "Other Availability")]
        public string OtherAvailability { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Meetings mode preference")]
        public MeetingMode MeetingsMode { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Mentee seniority preference")]
        public List<MenteeSeniorityModel> SeniorityLevel { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Please describe your expectations about the program")]
        public string Expectations { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Current Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Current Seniority")]
        public string Seniority { get; set; }
    }
}