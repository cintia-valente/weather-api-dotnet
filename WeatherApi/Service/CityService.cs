using WeatherApi.Models;
using WeatherApi.Repository;
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


        //public IEnumerable<City> FindAllWithWeatherData()
        //{
        //    return _cityRepository.FindAllWithWeatherData().ToList();
        //}

        //public IEnumerable<City> FindAll()
        //{
        //    // Include para carregar os dados meteorológicos relacionados a cada cidade
        //    return _cityRepository.FindAll().Include(city => city.WeatherDataList).ToList();
        //}

        public IEnumerable<City> FindAll()
        {
            return _cityRepository.FindAllWithWeatherData().ToList();
            //_cityRepository.FindAll();
            //return cityAll;
        }

        public City FindById(Guid id)
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
