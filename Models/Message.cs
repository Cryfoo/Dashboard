using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int id { get; set; }

        [Required]
        public string message { get; set; }

        public int creatorId { get; set; }
        public User creator { get; set; }

        public int recipientId { get; set; }
        public User recipient { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}