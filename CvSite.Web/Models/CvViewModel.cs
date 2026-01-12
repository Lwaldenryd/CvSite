using CvSite.Data.Entities;

namespace CvSite.Web.Models
{
    public class CvViewModel
    {
        public ApplicationUser User { get; set; } = null!;
        public List<Competence> Competences { get; set; } = new();
        public List<Education> Educations { get; set; } = new();
        public List<Experience> Experiences { get; set; } = new();
        public List<ProjectMember> ProjectMembers { get; set; } = new();
    }
}
