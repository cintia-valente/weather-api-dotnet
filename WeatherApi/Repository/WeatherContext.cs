using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherApi.Models;
using WeatherApi.Models.Enums;

namespace WeatherApi.Repository
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options)
            : base(options)
        {
        }

        public DbSet<Weather> WeatherData { get; set; }
        public DbSet<City> CityData { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create an index on the "Name" column of the CityEntity table
            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name)
                .IsUnique();

            // Configure IdCity to be generated automatically as a GUID
            modelBuilder.Entity<City>()
                .Property(c => c.IdCity)
                .ValueGeneratedOnAdd();

            // Configure IdWeather to be generated automatically as a GUID
            modelBuilder.Entity<Weather>()
                .Property(w => w.IdWeather)
                .ValueGeneratedOnAdd();

            // Configure the relationship between CityEntity and WeatherForecast
            // to delete all WeatherForecast entities when a CityEntity is deleted
            modelBuilder.Entity<City>()
                .HasMany(city => city.WeatherDataList)
                .WithOne(weather => weather.City)
                .HasForeignKey(weather => weather.IdCity)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Weather>()
            //   .HasOne(w => w.City)
            //   .WithMany()
            //   .HasForeignKey(w => w.IdCity) // Chave estrangeira
            //   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
