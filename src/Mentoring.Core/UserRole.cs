using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Core
{
    [Table("UserRole")]
    public class UserRole
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}