using WeatherApi.Entity;
using WeatherApi.Entity.Enums;
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
        weather.City = _cityRepository.FindById(weather.IdCity);

        if (!Enum.IsDefined(typeof(DayTimeEnum), weather.DayTime) ||
            !Enum.IsDefined(typeof(NightTimeEnum), weather.NightTime))
        {
            throw new ArgumentException("Valores inválidos para enums DayTime e/ou NightTime.");
        }

        var weatherSaved = _weatherRepository.Save(weather);
       
        return weatherSaved;

    }

    public Weather FindById(Guid id)
    {
        var wheaterById = _weatherRepository.FindById(id);
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
        return _weatherRepository.FindAllByCityName(cityName, page, pageSize);
                                      
    }

    public IEnumerable<Weather> GetWeatherForNext7Days(string cityName)
    {
        return _weatherRepository.FindByCityNextSixWeek(cityName);
    }


    public Weather Update(Guid idWheaterData, Weather weather)
    {

        var data = FindById(idWheaterData);


        if (!Enum.IsDefined(typeof(DayTimeEnum), weather.DayTime) ||
        !Enum.IsDefined(typeof(NightTimeEnum), weather.NightTime))
        {
            throw new ArgumentException("Valores inválidos para enums DayTime e/ou NightTime.");
        }

        _weatherRepository.Update(idWheaterData, weather);

        return data;
     
    }

    public bool DeleteById(Guid idWheaterData)
    {
        return _weatherRepository.DeleteById(idWheaterData);
    }

}
