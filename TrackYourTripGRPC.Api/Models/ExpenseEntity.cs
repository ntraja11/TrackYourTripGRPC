using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackYourTripGRPCApi.Protos;

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

        public int MemberId { get; set; }

        public int TripId { get; set; }
    }
}
