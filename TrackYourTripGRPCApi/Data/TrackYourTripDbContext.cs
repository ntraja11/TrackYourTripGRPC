using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Models;

namespace TrackYourTripGRPCApi.Data
{
    public class TrackYourTripDbContext : IdentityDbContext<ApplicationUser>
    {
        public TrackYourTripDbContext(DbContextOptions<TrackYourTripDbContext> options) : base(options)
        {
        }
        public DbSet<TripEntity> Trips { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configurations can be added here if needed
        }
    }
}
