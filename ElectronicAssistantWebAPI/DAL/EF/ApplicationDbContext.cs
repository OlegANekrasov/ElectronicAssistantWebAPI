using ElectronicAssistantWebAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicAssistantWebAPI.DAL.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<RecommendedPrescription> RecommendedPrescriptions => Set<RecommendedPrescription>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
