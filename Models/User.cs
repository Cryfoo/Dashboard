using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Dashboard.Models
{
    public class User : BaseEntity
    {
        public User() {
            messages = new List<Message>();
        }

        public ICollection<Message> messages { get; set; }

        [Key]
        public long id { get; set; }

        [Required(ErrorMessage="The first name field is required.")]
        [MinLength(2)]
        public string first_name { get; set; }

        [Required(ErrorMessage="The last name field is required.")]
        [MinLength(2)]
        public string last_name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
        
        [Required]
        [MinLength(8)]
        public string password { get; set; }

        [Required(ErrorMessage="The password confirmation field is required.")]
        [Compare("password", ErrorMessage = "Password and password confirmation must match.")]
        public string confirm { get; set; }

        public int user_level { get; set; }

        public string description { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}