using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentoring.Core
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public string ApplyTemplate(StringDictionary parameters)
        {
            string content = Content;
            
            if (parameters != null)
            {
                foreach (string key in parameters.Keys)
                {
                    content = content.Replace(key, parameters[key]);
                }
            }

            return content;
        }
    }
}