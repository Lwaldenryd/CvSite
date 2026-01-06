using System.ComponentModel.DataAnnotations;

namespace CvSite.Web.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        public string OwnerId { get; set; } = null!;
        public virtual ApplicationUser Owner { get; set; } = null!;

        
        public virtual ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    }
}
