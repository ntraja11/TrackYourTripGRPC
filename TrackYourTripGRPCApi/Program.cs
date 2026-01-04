using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TrackYourTripDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TrackYourTripDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddGrpc();

var app = builder.Build();


ApplyMigrations();

// Configure the HTTP request pipeline.
app.MapGrpcService<TripService>();
app.MapGrpcService<AuthService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();




void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TrackYourTripDbContext>();
    dbContext.Database.Migrate();
}
