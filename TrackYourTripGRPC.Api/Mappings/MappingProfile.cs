using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGRPCApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
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


            CreateMap<MemberEntity, MemberDetail>()
                .ForMember(dest => dest.TotalTripExpense,
                    opt => opt.MapFrom(src => (double)src.TotalTripExpense));

            CreateMap<MemberDetail, MemberEntity>()
                .ForMember(dest => dest.TotalTripExpense,
                    opt => opt.MapFrom(src => (decimal)src.TotalTripExpense));


            CreateMap<ExpenseEntity, ExpenseDetail>()
                .ForMember(dest => dest.Amount,
                    opt => opt.MapFrom(src => (double)src.Amount))
                .ForMember(dest => dest.ExpenseDate,
                    opt => opt.MapFrom(src =>
                        Timestamp.FromDateTime(DateTime.SpecifyKind(src.ExpenseDate, DateTimeKind.Utc))));

            CreateMap<ExpenseDetail, ExpenseEntity>()
                .ForMember(dest => dest.Amount,
                    opt => opt.MapFrom(src => (decimal)src.Amount))
                .ForMember(dest => dest.ExpenseDate,
                    opt => opt.MapFrom(src => src.ExpenseDate.ToDateTime()));


        }        
    }
}
