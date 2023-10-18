using WeatherApi.Entity;

namespace WeatherApi.Repository.Interfaces;

public interface ICityRepository
{
    Task<City> Save(City city);
    Task<IEnumerable<City>> FindAll();
    Task<City> FindAllByCityName(string cityName);
    Task<City?> FindById(Guid idCity);
}

