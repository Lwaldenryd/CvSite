using System;
using System.ComponentModel.DataAnnotations;

namespace CvSite.Web.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
