using System;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class User : BaseEntity
    {
        [Key]
        public long id { get; set; }

        [Required]
        [MinLength(2)]
        public string first_name { get; set; }

        [Required]
        [MinLength(2)]
        public string last_name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
        
        [Required]
        [MinLength(8)]
        public string password { get; set; }

        [Compare("password", ErrorMessage = "Password and Confirm Password must match!")]
        public string confirm { get; set; }

        public int user_level { get; set; }
        
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}