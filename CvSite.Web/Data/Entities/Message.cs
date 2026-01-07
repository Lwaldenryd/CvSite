using System;
using System.ComponentModel.DataAnnotations;

namespace CvSite.Web.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public ApplicationUser? Sender { get; set; }
        
        [StringLength(50)]
        public string? SenderName { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } 
        
        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}
