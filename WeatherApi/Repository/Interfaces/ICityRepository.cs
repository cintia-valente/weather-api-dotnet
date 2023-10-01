using WeatherApi.Models;

namespace WeatherApi.Repository.Interfaces
{
    public interface ICityRepository
    {

        City Save(City city);
        IEnumerable<City> FindAll();

        City? FindByID(Guid idCity);

        City FindAllByCityName(string cityName);

    }
}

