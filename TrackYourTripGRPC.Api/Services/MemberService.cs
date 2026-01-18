using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPC.SharedProtos.Protos;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;

namespace TrackYourTripGRPCApi.Services;

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

        if (existingMember is not null)
        {
            return new GetMemberResponse { Member = _mapper.Map<MemberDetail>(existingMember) };
        }

        throw new RpcException(new Status(StatusCode.NotFound, $"Member with Id {request.Id} is not found."));
    }

    public override async Task<CreateMembersResponse> CreateMembers(CreateMembersRequest request, ServerCallContext context)
    {
        if (request.Members.Any())
        {
            foreach (var member in request.Members)
            {
                var memberEntity = new MemberEntity
                {
                    Name = member.Name,
                    Email = member.Email,
                    TripId = member.TripId,
                };

                _dbContext.Members.Add(memberEntity);
            }
            await _dbContext.SaveChangesAsync();


            return new CreateMembersResponse { Success = true };
        }

        return new CreateMembersResponse { Success = false };

    }

    public override async Task<DeleteMembersResponse> DeleteMembers(DeleteMembersRequest request, ServerCallContext context)
    {
        if (request.MemberIds.Any())
        {
            foreach (var memberId in request.MemberIds)
            {
                var existingMember = await _dbContext.Members.FindAsync(memberId);

                if (existingMember != null)
                {
                    _dbContext.Members.Remove(existingMember);
                }
            }
            await _dbContext.SaveChangesAsync();
            return new DeleteMembersResponse { Success = true };
        }

        return new DeleteMembersResponse { Success = false };
    }

    public override async Task<GetAllMembersByTripResponse> GetAllMembersByTrip(GetAllMembersByTripRequest request, ServerCallContext context)
    {
        var response = new GetAllMembersByTripResponse();

        var members = await _dbContext.Members.Where(m => m.TripId == request.TripId).ToListAsync();

        var mappedMembers = _mapper.Map<IEnumerable<MemberDetail>>(members);

        response.Members.AddRange(mappedMembers);

        return response;
    }

    public override async Task<GetAllMembersByGroupResponse> GetAllMembersByGroup(
        GetAllMembersByGroupRequest request, ServerCallContext context)
    {
        var response = new GetAllMembersByGroupResponse();

        var availableUsers = await _dbContext.ApplicationUsers.Where(g => g.GroupId == request.GroupId).ToListAsync();

        IList<MemberDetail> availableMembers = new List<MemberDetail>();

        foreach (var user in availableUsers)
        {
            var member = new MemberDetail
            {
                Name = user.Name,
                Email = user.Email
            };

            availableMembers.Add(member);
        }

        response.Members.AddRange(availableMembers);

        return response;
    }

}
