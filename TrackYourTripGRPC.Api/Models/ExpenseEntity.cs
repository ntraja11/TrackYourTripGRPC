using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackYourTripGRPCApi.Models
{
    public class ExpenseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        [Precision(10, 2)]
        public decimal Amount { get; set; } = 0;
        public DateTime ExpenseDate { get; set; }

        [ForeignKey(nameof(Member))]
        public int MemberId { get; set; }
        public MemberEntity? Member { get; set; }

        [ForeignKey(nameof(Trip))]
        public int TripId { get; set; }
        public TripEntity? Trip { get; set; }
    }
}
