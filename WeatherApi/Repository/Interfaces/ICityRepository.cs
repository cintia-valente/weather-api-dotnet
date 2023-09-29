using WeatherApi.Models;

namespace WeatherApi.Repository.Interfaces
{
    public interface ICityRepository
    {

        City Save(City city);
        IEnumerable<City> FindAll();
        IEnumerable<City> FindAllWithWeatherData();


        City? FindByID(Guid idCity);

       // bool DeleteById(Guid idCity);
    }
}

