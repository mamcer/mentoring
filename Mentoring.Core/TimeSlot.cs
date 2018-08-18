using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Core
{
    [Table("TimeSlot")]
    public class TimeSlot
    {
        public TimeSlot()
        {
            Mentors = new HashSet<Mentor>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Mentor> Mentors { get; set; }
    }
}