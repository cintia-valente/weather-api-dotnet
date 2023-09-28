using WeatherApi.Models;

namespace WeatherApi.Service.Interfaces;

public interface ICityService
{
    //IEnumerable<Weather> GetAll();
    City Save(City city);
    IEnumerable<City> FindAll();

    City FindById(Guid id);
    IEnumerable<City> FindAllWithWeatherData();

    //Page<WeatherDataEntity> FindAllByCityNameIgnoreCase(String cityName, Pageable pageable);
    //IEnumerable<City> FindAllById(String cityName, Guid IdCity);
    //bool DeleteById(Guid idCity);

}
