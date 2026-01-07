using System.ComponentModel.DataAnnotations;

namespace CvSite.Web.Models
{
    public class SendMessageViewModel
    {
        [Required]
        public string ReceiverId { get; set; }

        [Display(Name = "Your name")]
        public string? SenderName { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }
        
    }
}
