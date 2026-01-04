using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Models;

namespace TrackYourTripGRPCApi.Data
{
    public class TrackYourTripDbContext : DbContext
    {
        public TrackYourTripDbContext(DbContextOptions<TrackYourTripDbContext> options) : base(options)
        {
        }
        public DbSet<TripEntity> Trips { get; set; }
    }
}
