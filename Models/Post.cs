using MyInstagramApi.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.Models
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        [Required]
        public string Filename { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestID { get; set; }

        public DateTime PostedAt { get; set; }
        public string Caption { get; set; }
        public int Likes { get; set; }
        public int CommentsCount { get; set; }
        public int share { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
