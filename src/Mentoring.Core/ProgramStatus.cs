using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Core
{
    [Table("ProgramStatus")]
    public class ProgramStatus
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public int StatusCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string StatusDescription { get; set; }
    }
}