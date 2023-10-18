using WeatherApi.Entity;

namespace WeatherApi.Service.Interfaces;

public interface ICityService
{
    Task<City> Save(City city);
    Task<IEnumerable<City>> FindAll();
    Task<City> FindById(Guid id);
}

