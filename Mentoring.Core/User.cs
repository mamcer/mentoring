using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Core
{
    [Table("User")]
    public class User
    {
        public User()
        {
            Roles = new HashSet<UserRole>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string NickName { get; set; }

        [MaxLength(2083)]
        public string AvatarUrl { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        public ICollection<UserRole> Roles { get; set; }

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        [Required]
        [MaxLength(100)]
        public string Seniority { get; set; }
    }
}