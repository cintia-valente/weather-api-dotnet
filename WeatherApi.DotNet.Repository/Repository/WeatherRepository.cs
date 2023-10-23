using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherApi.Entity;
using WeatherApi.Persistence;
using WeatherApi.Repository.Interfaces;

namespace WeatherApi.Repository;

public class WeatherRepository : IWeatherRepository
{
    private readonly WeatherContext _context; // Substitua pelo seu contexto de banco de dados
    private IMapper _mapper;
    public WeatherRepository(WeatherContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Weather> Save(Weather weather)
    {
        EntityEntry<Weather?> weatherEntity = _context.WeatherData.Add(weather);

        await _context.SaveChangesAsync();
        var weatherConvert = _mapper.Map<Weather>(weatherEntity.Entity);
        return weatherConvert;
    }

    public async Task<IQueryable<Weather>> FindAll()
    {
        var weatherData = await _context.WeatherData.Include(w => w.City).Select(x => new Weather() { 
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
        }).ToListAsync();
        
        return weatherData.AsQueryable();
    }

    public async Task<IEnumerable<Weather>> FindAllByOrderByDateDesc(int page, int pageSize)
    {
        var weatherDataDyDateDesc = await _context.WeatherData
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
        }).ToListAsync();

        return weatherDataDyDateDesc.AsQueryable();
    }

    
    public async Task<IQueryable<Weather>> FindAllByCityName(string cityName, int page, int pageSize)
    {
        var weatherDataByCityName = await _context.WeatherData
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
        }).ToListAsync();

        return weatherDataByCityName.AsQueryable();
    }

    public async Task<IQueryable<Weather>> FindByCityNextSixWeek(string cityName)
    {
        DateTime today = DateTime.UtcNow.Date;
        DateTime next7Days = today.AddDays(7);

        var weatherDataNextSixWeek = await _context.WeatherData
          .Include(w => w.City)
          .Where(w => w.City.Name == cityName && w.Date >= today && w.Date <= next7Days)
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
              Date = DateTime.SpecifyKind(x.Date, DateTimeKind.Utc),
              MaxTemperature = x.MaxTemperature,
              MinTemperature = x.MinTemperature,
              Precipitation = x.Precipitation,
              Humidity = x.Humidity,
              WindSpeed = x.WindSpeed,
              DayTime = x.DayTime,
              NightTime = x.NightTime
          }).ToListAsync();

            return weatherDataNextSixWeek.AsQueryable();
    }

    public async Task<Weather> FindById(Guid idWeather)
    {
        return await _context.WeatherData.Include(w => w.City).FirstOrDefaultAsync(metData => metData.IdWeather == idWeather);  
    }

    //public IEnumerable<Weather> FindByDates(List<DateTime> dates)
    //{
    //    return _context.WeatherData
    //        .Where(w => dates.Contains(w.Date))
    //        .Include(w => w.City)
    //        .ToList();
    //}

    public async Task Update (Guid idWheaterData, Weather weather)
    {
        //var data = await FindById(idWheaterData);

        //if (data == null)
        //{
        //    throw new DllNotFoundException("Weather data not found.");
        //}

        //data.Date = weather.Date;
        //data.MaxTemperature = weather.MaxTemperature;
        //data.MinTemperature = weather.MinTemperature;
        //data.Precipitation = weather.Precipitation;
        //data.Humidity = weather.Humidity;
        //data.WindSpeed = weather.WindSpeed;
        //data.DayTime = weather.DayTime;
        //data.NightTime = weather.NightTime;

        //data.City = weather.City;

        _context.Update(weather);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> DeleteById(Guid idWeather)
    {
        var weatherToDelete = await _context.WeatherData.FirstOrDefaultAsync(weather => weather.IdWeather == idWeather);

        if (weatherToDelete != null)
        {
            _context.Remove(weatherToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

}

