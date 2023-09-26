using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using WeatherApi.Models;

namespace WeatherApi.Data;

public class WeatherContext : DbContext
{

    public WeatherContext(DbContextOptions<WeatherContext> opts)
      : base(opts) { 
    }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurando o relacionamento many-to-one entre Weather e City
            modelBuilder.Entity<Weather>()
                .HasOne(w => w.City)
                .WithMany()
                .HasForeignKey(w => w.IdCity);
        }

    public DbSet<Weather> WeatherData { get; set; }
    public DbSet<City> CityData { get; set; }
}
