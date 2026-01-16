using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackYourTripGRPCApi.Models;

public class MemberEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;

    [Precision(10, 2)]
    public decimal TotalTripExpense { get; set; } = 0;
    
    public int TripId { get; set; }

    [NotMapped]
    public IEnumerable<ExpenseEntity> ExpenseList { get; set; } = new List<ExpenseEntity>();
}
