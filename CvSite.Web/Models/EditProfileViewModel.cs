using System.ComponentModel.DataAnnotations;

namespace CvSite.Web.Models
{
    public class EditProfileViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Display(Name = "Private profile")]
        public bool IsPrivate { get; set; }

        public IFormFile ProfileImage { get; set; }
    }
}
