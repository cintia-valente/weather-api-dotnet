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
            _cityRepository.Save(city);
            return city;
        }

        public City FindById(long id)
        {
            var cityById = _cityRepository.FindByID(id);
            return cityById;
        }

        //public bool DeleteById(Guid idCity)
        //{
        //    return _cityRepository.DeleteById(idCity);
        //}
    }
}
