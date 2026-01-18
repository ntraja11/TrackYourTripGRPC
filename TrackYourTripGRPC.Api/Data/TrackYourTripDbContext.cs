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

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExpenseEntity>()
                .HasOne(e => e.Trip)
                .WithMany(t => t.Expenses) // Ensure Trip has a collection of Expenses
                .HasForeignKey(e => e.TripId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            // Configure Expense -> Participant relationship
            modelBuilder.Entity<ExpenseEntity>()
                .HasOne(e => e.Member)
                .WithMany(p => p.Expenses) // Ensure Participant has a collection of Expenses
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
