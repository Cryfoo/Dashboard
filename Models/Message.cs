using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Dashboard.Models
{
    public class Message : BaseEntity
    {
        public Message() {
            comments = new List<Comment>();
        }

        public ICollection<Comment> comments { get; set; }

        [Key]
        public long id { get; set; }

        [Required]
        public string message { get; set; }

        public int user_id { get; set; }
        public User user { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}