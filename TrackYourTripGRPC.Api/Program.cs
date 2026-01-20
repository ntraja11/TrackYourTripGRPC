using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Data;
using TrackYourTripGRPCApi.Models;
using TrackYourTripGRPCApi.Services;
using TrackYourTripGRPCApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TrackYourTripDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sql => 
    {
        sql.EnableRetryOnFailure(
            maxRetryCount : 5,
            maxRetryDelay : TimeSpan.FromSeconds(10),
            errorNumbersToAdd : null
            );
    })
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TrackYourTripDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHostedService<DatabaseWarmupService>();


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddGrpc();

builder.Services.AddScoped<JwtTokenGenerator>();

//builder.Services.AddAuthentication(options => {
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options => {
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
//    };
//});

//builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors();



//app.UseAuthentication();
//app.UseAuthorization();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

ApplyMigrations();

// Configure the HTTP request pipeline.
app.MapGrpcService<TripService>().EnableGrpcWeb();
app.MapGrpcService<AuthService>().EnableGrpcWeb();
app.MapGrpcService<MemberService>().EnableGrpcWeb();
app.MapGrpcService<ExpenseService>().EnableGrpcWeb();

app.MapGet("/status", () => Results.Ok("GRPC Api is running"));
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.MapGet("/api/status", async (TrackYourTripDbContext db) =>
{
    try
    {
        await db.Database.ExecuteSqlRawAsync("SELECT 1");
        return Results.Ok("Database OK");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


app.Run();


void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TrackYourTripDbContext>();
    dbContext.Database.Migrate();
}
