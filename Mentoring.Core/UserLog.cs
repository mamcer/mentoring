using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Core
{
    [Table("UserLog")]
    public class UserLog
    {
        public int Id { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string Action { get; set; }

        public string Description { get; set; }
    }
}