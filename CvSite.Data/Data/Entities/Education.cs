using System.ComponentModel.DataAnnotations;

namespace CvSite.Data.Entities
{
    public class Education
    {
        public int id {  get; set; }

        [Required(ErrorMessage = "School/university is required.")]
        [MaxLength(200)]
        public string Institution { get; set; } = string.Empty;

        [Required(ErrorMessage = "education, course, program is required.")]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
