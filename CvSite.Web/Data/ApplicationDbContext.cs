using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CvSite.Web.Data.Entities;
using CvSite.Web.Models;

namespace CvSite.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Competence> Competences { get; set; } = null!;

        public DbSet<Message> Messages { get; set; }
    }
}
