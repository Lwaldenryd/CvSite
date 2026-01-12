using CvSite.Data.Entities;

namespace CvSite.Web.Models
{
    public class HomeIndexViewModel
    {
        public List<ApplicationUser> FeaturedCvs { get; set; } = new();
        public Project? LatestProject { get; set; }
    }
}
