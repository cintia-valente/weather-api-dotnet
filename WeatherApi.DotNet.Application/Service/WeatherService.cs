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

    public async Task<Weather> Save(Weather weather) 
    {
        weather.City = await _cityRepository.FindById(weather.IdCity);

        if (!Enum.IsDefined(typeof(DayTimeEnum), weather.DayTime) ||
            !Enum.IsDefined(typeof(NightTimeEnum), weather.NightTime))
        {
            throw new ArgumentException("Valores inválidos para enums DayTime e/ou NightTime.");
        }

        var weatherSaved = await _weatherRepository.Save(weather);
       
        return weatherSaved;
    }
    public async Task<Weather> FindById(Guid id)
    {
        return await _weatherRepository.FindById(id);
    }

    public async Task<IEnumerable<Weather>> FindAll()
    {
        return await _weatherRepository.FindAll();
    }

    public async Task<IEnumerable<Weather>> FindAllPage(int page, int pageSize)
    {
        return await _weatherRepository.FindAllByOrderByDateDesc(page, pageSize);
    }

    public async Task<IEnumerable<Weather>> FindAllPageByNameCity(string cityName, int page, int pageSize)
    {
        return await _weatherRepository.FindAllByCityName(cityName, page, pageSize);
    }

    public async Task<IEnumerable<Weather>> GetWeatherForNext7Days(string cityName)
    {
        return await _weatherRepository.FindByCityNextSixWeek(cityName);
    }

    public async Task<Weather> Update(Guid idWeatherData, Weather weather)
    {
        var data = await FindById(idWeatherData);

        weather.IdWeather = idWeatherData;

        if (!Enum.IsDefined(typeof(DayTimeEnum), weather.DayTime) ||
        !Enum.IsDefined(typeof(NightTimeEnum), weather.NightTime))
        {
            throw new ArgumentException("Valores inválidos para enums DayTime e/ou NightTime.");
        }

        await _weatherRepository.Update(idWeatherData, weather);

        return data;
    }

    public async Task<bool> DeleteById(Guid idWheaterData)
    {
        return await _weatherRepository.DeleteById(idWheaterData);
    }

}
