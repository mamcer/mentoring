using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mentoring.Core.Enums;

namespace Mentoring.Core
{
    [Table("Mentor")]
    public class Mentor
    {
        public Mentor()
        {
            Topic = new HashSet<Topic>();
            Availability = new HashSet<TimeSlot>();
            Mentees = new HashSet<Mentee>();
            MenteeSeniorities = new HashSet<MenteeSeniority>();
        }

        public int Id { get; set; }

        [Required]
        public string CurrentActivity { get; set; }

        public virtual ICollection<Topic> Topic { get; set; }

        public string OtherTopic { get; set; }

        [Required]
        public string MentoringTarget { get; set; }

        [Required]
        public int MaxMentees { get; set; }

        public virtual ICollection<TimeSlot> Availability { get; set; }

        public virtual ICollection<MenteeSeniority> MenteeSeniorities { get; set; }

        public string OtherAvailability { get; set; }

        [Required]
        public MeetingMode MeetingsMode { get; set; }

        [Required]
        public string Expectations { get; set; }

        [Required]
        public MentorStatus Status { get; set; }
        
        public User User { get; set; }

        public virtual ICollection<Mentee> Mentees { get; set; }
    }
}