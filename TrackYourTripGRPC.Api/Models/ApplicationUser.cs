using Microsoft.AspNetCore.Identity;

namespace TrackYourTripGRPCApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public required int GroupId { get; set; }
    }
}
