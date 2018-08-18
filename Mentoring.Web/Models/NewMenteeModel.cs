using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mentoring.Web.Models
{
    public class NewMenteeModel
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
        [Display(Name = "First Mentor Option")]
        public List<SelectMentorModel> FirstMentorOption { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Second Mentor Option")]
        public List<SelectMentorModel> SecondMentorOption { get; set; }

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