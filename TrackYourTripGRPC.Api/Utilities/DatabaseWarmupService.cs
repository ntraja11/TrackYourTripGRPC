using Microsoft.EntityFrameworkCore;
using TrackYourTripGRPCApi.Data;

public class DatabaseWarmupService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DatabaseWarmupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackYourTripDbContext>();

        try
        {
            await db.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
        }
        catch
        {
            // swallow errors — warm-up should never crash the app
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}