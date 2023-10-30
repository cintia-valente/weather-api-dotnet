using WeatherApi.Data.DTOs;
using WeatherApi.Entity;

namespace WeatherApi.Service.Interfaces;

public interface IWeatherService
{ 
    Task<Weather> Save(WeatherRequestDTO weatherDto);
    Task<IEnumerable<Weather>> FindAll();
    Task<IEnumerable<Weather>> FindAllPageByNameCity(string cityName, int page, int pageSize);
    Task<Weather> FindById(Guid id, bool tracking = true);

    Task<IEnumerable<Weather>> FindAllPage(int page, int pageSize);
    Task<IEnumerable<Weather>> GetWeatherForNext7Days(string cityName);
    Task Update(Guid idWeatherData, WeatherRequestDTO weatherDto);
    Task<bool> DeleteById(Guid idWheaterData);
}
