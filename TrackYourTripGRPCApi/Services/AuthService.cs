using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Protos;
using TrackYourTripGRPCApi.Utilities;

namespace TrackYourTripGRPCApi.Services
{
    public class AuthService : Protos.Auth.AuthBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TrackYourTripDbContext _dbContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, TrackYourTripDbContext dbContext, JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
                
        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            ApplicationUser? existingUser = await GetUser(request.Email);

            if (existingUser == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"User with email {request.Email} not found."));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(existingUser, request.Password, false);

            return result.Succeeded ?
                new LoginResponse
                {
                    Status = true,
                    Token = await _jwtTokenGenerator.GenerateTokenAsync(existingUser)
                }
                : throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid credentials."));
        }
        
        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            ApplicationUser? existingUser = await GetUser(request.Email);
            if (existingUser != null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, $"User with email {request.Email} already exists."));
            }

            if(request.NewGroup)
            {
                var group = new Group { Name = request.GroupName };
                _dbContext.Groups.Add(group);
                await _dbContext.SaveChangesAsync();

                return await GenerateRegisterResponseAsync(request, group);
            }
            else
            {
                var group = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Name == request.GroupName);
                if (group == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Group with name {request.GroupName} not found."));
                }
                else
                {
                    return await GenerateRegisterResponseAsync(request, group);
                }
            }

            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid registration request."));
        }

        private async Task<ApplicationUser?> GetUser(string email)
        {
            var normalizedEmail = _userManager.NormalizeEmail(email);
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }

        private async Task<RegisterResponse> GenerateRegisterResponseAsync(RegisterRequest request, Group group)
        {
            var newUser = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                GroupId = group.Id
            };
            var result = await _userManager.CreateAsync(newUser, request.Password);
            return new RegisterResponse
            {
                Status = result.Succeeded
            };
        }
    }
}
