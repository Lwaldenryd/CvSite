namespace CvSite.Web.Data.Entities
{
    public class ProjectMember
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
