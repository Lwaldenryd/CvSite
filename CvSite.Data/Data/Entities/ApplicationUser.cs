using Microsoft.AspNetCore.Identity;

namespace CvSite.Data.Entities


{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsPrivate { get; set; } = false;

        public byte[]? ProfilePicture { get; set; }


        public virtual ICollection<Competence> Competences { get; set; } = new List<Competence>();
        public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
        public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
    }
}