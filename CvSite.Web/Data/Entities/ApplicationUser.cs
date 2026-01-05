using Microsoft.AspNetCore.Identity;

namespace CvSite.Web.Data.Entities


{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public bool publicProfile { get; set; } = true;
        }
}
