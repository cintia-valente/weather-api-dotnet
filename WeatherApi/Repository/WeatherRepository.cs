using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherApi.Models;
using WeatherApi.Repository.Interfaces;

namespace WeatherApi.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherContext _context; // Substitua pelo seu contexto de banco de dados
        private IMapper _mapper;
        public WeatherRepository(WeatherContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Weather Save(Weather weather)
        {
            EntityEntry<Weather?> weatherEntity = _context.WeatherData.Add(weather);
           
            _context.SaveChanges();
            var weatherConvert = _mapper.Map<Weather>(weatherEntity.Entity);
            return weatherConvert;
        }

        public IQueryable<Weather> FindAll()
        {
            return _context.WeatherData.Include(w => w.City).Select(x => new Weather() { 
                City = new City(){ 
                    IdCity = x.IdCity,
                    Name = x.City.Name,
                    WeatherDataList = null
                }, 
                IdWeather = x.IdWeather,
                Date = x.Date,
                MaxTemperature = x.MaxTemperature,
                MinTemperature = x.MinTemperature,
                Precipitation = x.Precipitation,
                Humidity = x.Humidity,
                WindSpeed = x.WindSpeed,
                DayTime = x.DayTime,
                NightTime = x.NightTime
            }).AsQueryable();
        }

        public IEnumerable<Weather> FindAllByOrderByDateDesc(int page, int pageSize)
        {
            return _context.WeatherData
            .Include(w => w.City)
            .OrderByDescending(x => x.Date)  // Ordena por data descendente
            .Skip((page - 1) * pageSize)     // Pula os registros das páginas anteriores
            .Take(pageSize)                  // Pega a quantidade de registros por página
            .Select(x => new Weather
            {
                City = new City
                {
                    IdCity = x.IdCity,
                    Name = x.City.Name,
                    WeatherDataList = null
                },
                IdWeather = x.IdWeather,
                Date = x.Date,
                MaxTemperature = x.MaxTemperature,
                MinTemperature = x.MinTemperature,
                Precipitation = x.Precipitation,
                Humidity = x.Humidity,
                WindSpeed = x.WindSpeed,
                DayTime = x.DayTime,
                NightTime = x.NightTime
            })
            .AsQueryable();
        }

        
        public IQueryable<Weather> FindAllByCityName(string cityName, int page, int pageSize)
        {
            return _context.WeatherData
            .Include(w => w.City)
            .Where(w => w.City.Name == cityName)
            .OrderByDescending(x => x.Date)
            .Skip((page - 1) * pageSize)     
            .Take(pageSize)
            .Select(x => new Weather
                {
                    City = new City
                    {
                        IdCity = x.IdCity,
                        Name = x.City.Name,
                        WeatherDataList = null
                    },
                    IdWeather = x.IdWeather,
                    Date = x.Date,
                    MaxTemperature = x.MaxTemperature,
                    MinTemperature = x.MinTemperature,
                    Precipitation = x.Precipitation,
                    Humidity = x.Humidity,
                    WindSpeed = x.WindSpeed,
                    DayTime = x.DayTime,
                    NightTime = x.NightTime
            })
            .AsQueryable();
        }

        public IQueryable<Weather> FindByCityNextSixWeek(string cityName)
        {
            return _context.WeatherData
              .Include(w => w.City)
              .Where(w => w.City.Name == cityName)
              .OrderBy(x => x.Date)
              .Select(x => new Weather
              {
                  City = new City
                  {
                      IdCity = x.IdCity,
                      Name = x.City.Name,
                      WeatherDataList = null
                  },
                  IdWeather = x.IdWeather,
                  Date = x.Date,
                  MaxTemperature = x.MaxTemperature,
                  MinTemperature = x.MinTemperature,
                  Precipitation = x.Precipitation,
                  Humidity = x.Humidity,
                  WindSpeed = x.WindSpeed,
                  DayTime = x.DayTime,
                  NightTime = x.NightTime
              })
              .AsQueryable();
        }

        public Weather? FindByID(Guid idWeather)
        {
            return _context.WeatherData.FirstOrDefault(metData => metData.IdWeather == idWeather);
           
        }

        public IEnumerable<Weather> FindByDates(List<DateTime> dates)
        {
            return _context.WeatherData
                .Where(w => dates.Contains(w.Date))
                .Include(w => w.City)
                .ToList();
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public bool DeleteById(Guid idWeather)
        {
            var weatherToDelete = _context.WeatherData.FirstOrDefault(weather => weather.IdWeather == idWeather);
            if (weatherToDelete != null)
            {
                _context.Remove(weatherToDelete);
                _context.SaveChanges();
                return true;
            }

            return false;
           
        }

    }
    
}
