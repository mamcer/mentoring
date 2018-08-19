using System.ComponentModel.DataAnnotations;

namespace Mentoring.Web.Models
{
    public class MenteeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Mention what technology, area or project you are working on at the moment")]
        public string CurrentActivity { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Please specify the skills you would like to focus on")]
        public string SkillFocus { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "First Mentor option")]
        public MentorModel FirstMentorOption { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Second Mentor option")]
        public MentorModel SecondMentorOption { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Mentor")]
        public MentorModel Mentor { get; set; }

        public int FirstMentorOptionId { get; set; }

        public int SecondMentorOptionId { get; set; }

        [Display(Name = "First Option Accepted")]
        public bool FirstMentorOptionAccepted { get; set; }

        [Display(Name = "Second Option Accepted")]
        public bool SecondMentorOptionAccepted { get; set; }
    }
}