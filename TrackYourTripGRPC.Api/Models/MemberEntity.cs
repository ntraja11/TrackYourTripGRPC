using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGRPCApi.Models
{
    public class MemberEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;

        [Precision(10, 2)]
        public decimal TotalTripExpense { get; set; } = 0;
        
        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public TripEntity? Trip { get; set; }

        [NotMapped]
        public IEnumerable<ExpenseEntity> ExpenseList { get; set; } = new List<ExpenseEntity>();
    }
}
