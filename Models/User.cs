using Microsoft.AspNetCore.Identity;
using MyInstagramApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.Model
{
    public class User : IdentityUser
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string CountryName { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}
