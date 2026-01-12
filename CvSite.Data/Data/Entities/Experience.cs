using System.ComponentModel.DataAnnotations;

namespace CvSite.Data.Entities;

public class Experience
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Company is required.")]
    [MaxLength(200)]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    [MaxLength(200)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }
}

