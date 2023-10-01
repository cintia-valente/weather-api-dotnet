using WeatherApi.Models;

namespace WeatherApi.Service.Interfaces;

public interface ICityService
{
    City Save(City city);
    IEnumerable<City> FindAll();

    City FindById(Guid id);

}

