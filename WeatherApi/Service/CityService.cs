using WeatherApi.Models;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service.Interfaces;

namespace WeatherApi.Service
{
    public class CityService : ICityService
    {
        private ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public City Save(City city)
        {
            var citySaved = _cityRepository.Save(city);
            return citySaved;
        }

        public IEnumerable<City> FindAll()
        {
            return _cityRepository.FindAll().ToList();
        }

        public City FindById(Guid id)
        {
            var cityById = _cityRepository.FindById(id);
            return cityById;
        }

    }

}
