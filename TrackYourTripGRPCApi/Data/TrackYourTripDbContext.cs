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

        public DbSet<MemberEntity> Members { get; set; }
        public DbSet<ExpenseEntity> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExpenseEntity>()
                .HasOne(m => m.Member)
                .WithMany()
                .HasForeignKey(i => i.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ExpenseEntity>()
                .HasOne(t => t.Trip)
                .WithMany()
                .HasForeignKey(i => i.TripId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
