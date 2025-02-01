using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();

var app = builder.Build();

app.MapGet("/currencies/no-cache", async (AppDbContext dbContext) =>
{
    return await dbContext.Currencies
        .AsNoTracking()
        .TagWith("N0-CACHE")
        .ToListAsync();
});

app.MapGet("/currencies/in-memory-cache", async (AppDbContext dbContext, IMemoryCache memoryCache) =>
{
    const string cacheKey = "currency_list_inmemory";

    var currencies = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);

        return await dbContext.Currencies
            .AsNoTracking()
            .TagWith("IN-MEMORY-CACHE")
            .ToListAsync();
    });

    return currencies;
});

app.Run();

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Currency> Currencies { get; set; }
}

public record Currency(int Id, string Code, string Name);
