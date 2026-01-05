using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGRPCApi.Services
{
    public class TripService : Protos.Trip.TripBase
    {
        public TrackYourTripDbContext _dbContext { get; }
        public IMapper _mapper { get; }

        public TripService(TrackYourTripDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public override async Task<CreateTripResponse> CreateTrip(CreateTripRequest request, ServerCallContext context)
        {
            var trip = new TripEntity
            {                
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate.ToDateTime(),
                EndDate = request.EndDate?.ToDateTime() is DateTime dt ? dt : null,
                From = request.From,
                To = request.To,
                Status = Utilities.StaticDetails.TripStatus.Planned,
                Notes = request.Notes,
                CreatedByUserEmail = request.CreatedByUserEmail
            };
            _dbContext.Trips.Add(trip);
            await _dbContext.SaveChangesAsync();

            var tripDetail = _mapper.Map<TripDetail>(trip);

            return new CreateTripResponse
            {
                Trip = tripDetail
            };
        }

        public override async Task<UpdateTripResponse> UpdateTrip(UpdateTripRequest request, ServerCallContext context)
        {
            var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == request.Id);
            if (trip == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Trip with ID {request.Id} not found."));
            }
            trip.Title = request.Title;
            trip.Description = request.Description;
            trip.StartDate = request.StartDate.ToDateTime();
            trip.EndDate = request.EndDate?.ToDateTime() is DateTime dt ? dt : null;
            trip.From = request.From;
            trip.To = request.To;
            //trip.Status = request.Status;
            trip.Notes = request.Notes;

            _dbContext.Trips.Update(trip);
            await _dbContext.SaveChangesAsync();

            return new UpdateTripResponse
            {
                Trip = _mapper.Map<TripDetail>(trip)
            };
        }

        public override async Task<GetTripResponse> GetTrip(GetTripRequest request, ServerCallContext context)
        {
            var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == request.Id);
            if (trip == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Trip with ID {request.Id} not found."));
            }
            return new GetTripResponse
            {
                Trip = _mapper.Map<TripDetail>(trip)
            };
        }

        public override async Task<DeleteTripResponse> DeleteTrip(DeleteTripRequest request, ServerCallContext context)
        {
            var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == request.Id);
            if (trip == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Trip with ID {request.Id} not found."));
            }
            _dbContext.Trips.Remove(trip);
            await _dbContext.SaveChangesAsync();
            return new DeleteTripResponse
            {
                Success = true
            };
        }

        public override async Task<GetAllTripsResponse> GetAllTrips(Empty request, ServerCallContext context)
        {
            var response = new GetAllTripsResponse();
            response.Trips.AddRange(_mapper.Map<IEnumerable<TripDetail>>( await _dbContext.Trips.ToListAsync()));
            return response;
        }

      

    }
}
