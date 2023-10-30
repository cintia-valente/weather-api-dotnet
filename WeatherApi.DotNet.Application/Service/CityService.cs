using WeatherApi.Entity;
using WeatherApi.Service.Interfaces;
using WeatherApi.Repository.Interfaces;
namespace WeatherApi.Service;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<City> Save(City city)
    {
        var citySaved = await _cityRepository.Save(city);
        return citySaved;
    }

    public async Task<IEnumerable<City>> FindAll()
    {
        return await _cityRepository.FindAll();
    }

    public async Task<City> FindById(Guid id)
    {
        var cityById = await _cityRepository.FindById(id);
        return cityById;
    }

}
