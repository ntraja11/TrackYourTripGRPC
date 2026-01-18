using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPC.SharedProtos.Protos;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Utilities;

namespace TrackYourTripGRPCApi.Services;

public class AuthService : Auth.AuthBase
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

        LoginResponse response = new LoginResponse();
        response.IsSuccess = false;

        if (existingUser == null)
        {
            response.ErrorMessage = $"User with email '{request.Email}' not found.";
            response.StatusCode = SD.NotFound;
            return response;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(existingUser, request.Password, false);

        if (result.Succeeded)
        {
            response.IsSuccess = true;
            response.StatusCode = SD.Success;
            response.Token = await _jwtTokenGenerator.GenerateTokenAsync(existingUser);
        }
        else
        {
            response.StatusCode = SD.UnAuthorized;
            response.ErrorMessage = "Invalid credentials.";
        }

        return response;
    }

    [AllowAnonymous]
    public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
    {
        RegisterResponse response = new RegisterResponse();
        response.IsSuccess = false;

        ApplicationUser? existingUser = await GetUser(request.Email);
        if (existingUser != null)
        {
            response.StatusCode = SD.AlreadyExists;
            response.ErrorMessage = $"User with email '{request.Email}' already exists.";
            return response;
        }

        var existingGroup = await GetGroup(request.GroupName);

        if (request.IsNewGroup)
        {
            if (existingGroup is not null)
            {
                response.StatusCode = SD.AlreadyExists;
                response.ErrorMessage = $"Group with name '{request.GroupName}' already exists.";
                return response;
            }

            var newGroup = new Group { Name = request.GroupName };
            _dbContext.Groups.Add(newGroup);
            await _dbContext.SaveChangesAsync();

            return await GenerateRegisterResponseAsync(request, newGroup, response);
        }
        else
        {
            if (existingGroup == null)
            {
                response.StatusCode = SD.NotFound;
                response.ErrorMessage = $"Group with name '{request.GroupName}' not found.";
                return response;
            }
            else
            {
                return await GenerateRegisterResponseAsync(request, existingGroup, response);
            }
        }
    }

    private async Task<Group?> GetGroup(string groupName)
    {
        return await _dbContext.Groups
            .FirstOrDefaultAsync(g => g.Name == groupName);
    }

    private async Task<ApplicationUser?> GetUser(string email)
    {
        var normalizedEmail = _userManager.NormalizeEmail(email);
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
    }

    private async Task<RegisterResponse> GenerateRegisterResponseAsync(RegisterRequest request,
        Group group, RegisterResponse response)
    {
        var newUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            Name = request.Name,
            GroupId = group.Id
        };
        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (result.Succeeded)
        {
            response.IsSuccess = true;
            response.StatusCode = SD.Success;
        }
        else
        {
            if (request.IsNewGroup)
            {
                var groupToRemove = await _dbContext.Groups.FindAsync(group.Id);
                if (groupToRemove != null)
                {
                    _dbContext.Groups.Remove(groupToRemove);
                    await _dbContext.SaveChangesAsync();
                }
            }

            response.StatusCode = SD.ServerError;
            response.ErrorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
        }
        return response;
    }
}
