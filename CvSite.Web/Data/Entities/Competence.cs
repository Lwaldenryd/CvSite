using System.ComponentModel.DataAnnotations;

namespace CvSite.Web.Data.Entities
{
    public class Competence
    {
        public int id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [MaxLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}

