using WeatherApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace WeatherApi.Data;

public class CityContext : DbContext
{
    private readonly string _connectionString;
    public CityContext(DbContextOptions<CityContext> opts)
        : base(opts)
    {
        _connectionString = opts.FindExtension<NpgsqlOptionsExtension>()?.ConnectionString;
    }

    public DbSet<City> CityData { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasKey(c => c.IdCity);
    }
}
