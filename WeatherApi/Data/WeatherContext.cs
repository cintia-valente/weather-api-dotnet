using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using WeatherApi.Models;

namespace WeatherApi.Data;

public class WeatherContext : DbContext
{

    public WeatherContext(DbContextOptions<WeatherContext> opts)
      : base(opts) { 
    }
       

    public DbSet<Weather> WeatherData { get; set; }
    public DbSet<City> CityData { get; set; }
}
