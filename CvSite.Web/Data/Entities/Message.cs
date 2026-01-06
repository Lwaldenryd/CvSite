using System;
using System.ComponentModel.DataAnnotations;

namespace CvSite.Web.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.Now;

        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}
