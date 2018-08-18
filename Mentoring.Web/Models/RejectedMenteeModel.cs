using System.ComponentModel.DataAnnotations;

namespace Mentoring.Web.Models
{
    public class RejectedMenteeModel
    {
        public int Id { get; set; }

        [Display(Name = "Optional Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}