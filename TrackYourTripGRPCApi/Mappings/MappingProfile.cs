using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGRPCApi.Mappings
{
    public class TripMappingProfile : Profile
    {
        public TripMappingProfile()
        {
            CreateMap<TripEntity, TripDetail>()

                .ForMember(dest => dest.StartDate,
                opt => opt.MapFrom(src => src.StartDate.HasValue
                        ? Timestamp.FromDateTime
                        (DateTime.SpecifyKind(src.StartDate.Value, DateTimeKind.Utc))
                        : null))

                .ForMember(dest => dest.EndDate,
                opt => opt.MapFrom(src => src.EndDate.HasValue
                        ? Timestamp.FromDateTime
                        (DateTime.SpecifyKind(src.EndDate.Value, DateTimeKind.Utc))
                        : null))

                .ForMember(dest => dest.TotalExpense,
                opt => opt.MapFrom(src => src.TotalExpense.HasValue
                        ? src.TotalExpense.Value.ToString()
                        : "0"));
        }        
    }
}
