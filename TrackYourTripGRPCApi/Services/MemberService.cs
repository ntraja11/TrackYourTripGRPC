using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Protos;

namespace TrackYourTripGRPCApi.Services
{
    public class MemberService : Member.MemberBase
    {
        public TrackYourTripDbContext _dbContext { get; }
        public IMapper _mapper { get; }

        public MemberService(TrackYourTripDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public override async Task<GetMemberResponse> GetMember(GetMemberRequest request, ServerCallContext context)
        {
            var existingMember = await _dbContext.Members.FindAsync(request.Id);

            if(existingMember is not null)
            {
                return new GetMemberResponse { Member = _mapper.Map<MemberDetail>(existingMember) };
            }

            throw new RpcException(new Status(StatusCode.NotFound, $"Member with Id {request.Id} is not found."));
        }

        public override async Task<CreateMemberResponse> CreateMember(CreateMemberRequest request, ServerCallContext context)
        {
            var tripExists = await _dbContext.Trips.AnyAsync(t => t.Id == request.TripId);
            if (!tripExists)
                throw new RpcException(new Status(StatusCode.NotFound, $"Trip with Id {request.TripId} not found."));

            var member = new MemberEntity
            {
                Name = request.Name,
                Email = request.Email,
                TripId = request.TripId
            };
            _dbContext.Members.Add(member);
            await _dbContext.SaveChangesAsync();
            var memberDetail = _mapper.Map<MemberDetail>(member);
            return new CreateMemberResponse
            {
                Member = memberDetail
            };
        }

        public override async Task<DeleteMemberResponse> DeleteMember(DeleteMemberRequest request, ServerCallContext context)
        {
            var existingMember = await _dbContext.Members.FindAsync(request.Id);

            if (existingMember != null) {
                _dbContext.Members.Remove(existingMember);
                await _dbContext.SaveChangesAsync();
                return new DeleteMemberResponse
                {
                    Success = true
                };
            }

            return new DeleteMemberResponse
            {
                Success = false
            };
        }

        public override async Task<GetAllMembersByTripIdResponse> GetAllMembersByTripId(GetAllMembersByTripIdRequest request, ServerCallContext context)
        {
            var response = new GetAllMembersByTripIdResponse();

            var members = await _dbContext.Members.Where(m => m.TripId == request.TripId).ToListAsync();

            var mappedMembers = _mapper.Map<IEnumerable<MemberDetail>>(members);

            response.Members.AddRange(mappedMembers);

            return response;
        }

    }
}
