using Microsoft.EntityFrameworkCore;
using static TrackYourTripGRPCApi.Utilities.StaticDetails;

namespace TrackYourTripGRPCApi.Models
{
    public class TripEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string From { get; set; } = null!;
        public string To { get; set; } = null!;

        [Precision(10, 2)]
        public decimal? TotalExpense { get; set; }

        public TripStatus? Status { get; set; }
        public string? Notes { get; set; }

        public string? CreatedByUserEmail { get; set; }

    }
}
