using WeatherApi.Entity;

namespace WeatherApi.Repository.Interfaces;

public interface ICityRepository
{

    City Save(City city);
    IEnumerable<City> FindAll();

    City? FindById(Guid idCity);

    City FindAllByCityName(string cityName);
    void Save(City validCity);
}

