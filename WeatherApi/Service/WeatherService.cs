using WeatherApi.Models;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service.Interfaces;

namespace WeatherApi.Service;

public class WeatherService : IWeatherService
{
    private IWeatherRepository _weatherRepository;
    private ICityRepository _cityRepository;

    public WeatherService(IWeatherRepository weatherRepository, ICityRepository cityRepository)
    {
        _weatherRepository = weatherRepository;
        _cityRepository = cityRepository;

    }

    public Weather Save(Weather weather)
    {
        weather.City = _cityRepository.FindByID(weather.IdCity);

        var weatherSaved = _weatherRepository.Save(weather);
        return weatherSaved;

    }

    public Weather FindById(Guid id)
    {
        var wheaterById = _weatherRepository.FindByID(id);
        return wheaterById;
    }

    public IEnumerable<Weather> FindAll()
    {
        return _weatherRepository.FindAll();
    }

    public IEnumerable<Weather> FindAllPage(int page, int pageSize)
    {
        return _weatherRepository.FindAllByOrderByDateDesc(page, pageSize);
    }

    public IEnumerable<Weather> FindAllPageByNameCity(string cityName, int page, int pageSize)
    {
        var weatherList = _weatherRepository.FindAllByCityName(cityName)
                                       .Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToList();

        return weatherList;
    }

    public IEnumerable<Weather> GetWeatherForNext7Days()
        {
            DateTime today = DateTime.UtcNow;
            List<DateTime> dateList = new List<DateTime>();

            for (int i = 0; i < 7; i++)
            {
                DateTime date = today.AddDays(i);
                dateList.Add(date.Date);
            }

            return _weatherRepository.FindByDates(dateList);
        }


    //public Weather Update(Guid idWheaterData, Weather weather)
    //{

    //    var data = _weatherRepository.FindByID(idWheaterData);
    //    _weatherRepository.Update();
    //    return data;
    //    //var weatheredit = _weatherRepository.FindByID(idWheaterData);

    //    //if (weatheredit == null)
    //    //    return null;

    //    //weatheredit.Date = weather.Date;
    //    //weatheredit.MaxTemperature = weather.MaxTemperature;
    //    //weatheredit.MinTemperature = weather.MinTemperature;
    //    //weatheredit.Precipitation = weather.Precipitation;
    //    //weatheredit.Humidity = weather.Humidity;
    //    //weatheredit.WindSpeed = weather.WindSpeed;
    //    //weatheredit.DayTime = weather.DayTime;
    //    //weatheredit.NightTime = weather.NightTime;
    //    //weatheredit.City.Name = weather.City.Name;

    //    //return _weatherRepository.Save(weatheredit);


    //}

    //public bool DeleteById(Guid idWheaterData)
    //{
    //    return _weatherRepository.DeleteById(idWheaterData);
    //}

}
