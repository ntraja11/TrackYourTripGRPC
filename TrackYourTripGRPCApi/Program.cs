using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Services;
using TrackYourTripGRPCApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TrackYourTripDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TrackYourTripDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddGrpc();

builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();


//app.UseAuthentication();
//app.UseAuthorization();

ApplyMigrations();

// Configure the HTTP request pipeline.
app.MapGrpcService<TripService>();
app.MapGrpcService<AuthService>();
app.MapGrpcService<MemberService>();
app.MapGrpcService<ExpenseService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();




void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TrackYourTripDbContext>();
    dbContext.Database.Migrate();
}
