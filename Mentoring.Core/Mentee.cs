using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mentoring.Core.Enums;

namespace Mentoring.Core
{
    [Table("Mentee")]
    public class Mentee
    {
        public int Id { get; set; }

        [Required]
        public string CurrentActivity { get; set; }

        [Required]
        public string Focus { get; set; }

        [Required]
        public Mentor FirstOption { get; set; }

        public bool? FirstOptionAccepted { get; set; }

        public Mentor SecondOption { get; set; }

        public bool? SecondOptionAccepted { get; set; }

        public DateTime MentorOptionDate { get; set; }

        public User User { get; set; }

        public Mentor Mentor { get; set; }

        [Required]
        public MenteeStatus Status { get; set; }
    }
}