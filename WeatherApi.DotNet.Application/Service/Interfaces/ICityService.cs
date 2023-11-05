using WeatherApi.Data.DTOs;
using WeatherApi.Entity;

namespace WeatherApi.Service.Interfaces;

public interface ICityService
{
    Task<City> Save(CityRequestDto cityDto);
    Task<IEnumerable<City>> FindAll();
    Task<City> FindById(Guid id);
}

