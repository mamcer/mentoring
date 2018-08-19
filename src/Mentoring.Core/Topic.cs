using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Core
{
    [Table("Topic")]
    public class Topic
    {
        public Topic()
        {
            Mentors = new HashSet<Mentor>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Mentor> Mentors { get; set; }
    }
}