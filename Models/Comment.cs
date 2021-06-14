using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.Models
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime PostedAt { get; } = DateTime.Now;

        [ForeignKey("Post")]
        public string PostId { get; set; }

        public Post Post { get; set; }
    }
}
