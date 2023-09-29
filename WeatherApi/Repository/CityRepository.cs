
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherApi.Models;
using WeatherApi.Repository.Interfaces;

namespace WeatherApi.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly WeatherContext _context; // Substitua pelo seu contexto de banco de dados
        private IMapper _mapper;

        public CityRepository(WeatherContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public City Save(City city)
        {
            EntityEntry<City?> cityEntity = _context.CityData.Add(city);
            _context.SaveChanges();
            var cityConvert = _mapper.Map<City>(cityEntity.Entity);
            return cityConvert;
        }

        public IEnumerable<City> FindAllWithWeatherData()
        {
            return _context.CityData.Include(city => city.WeatherDataList);
        }


        public IEnumerable<City> FindAll()
        {
            return _context.CityData.Include(city => city.WeatherDataList);
        }


        public City? FindByID(Guid idCity)
        {
            return _context.CityData.Include(city => city.WeatherDataList).FirstOrDefault(data => data.IdCity == idCity);

        }

        //public bool DeleteById(Guid idCity)
        //{
        //    var cityToDelete = _context.CityData.FirstOrDefault(city => city.IdCity == idCity);
        //    if (cityToDelete != null)
        //    {
        //        _context.Remove(cityToDelete);
        //        _context.SaveChanges();  // Salva as mudanças no contexto (banco de dados)
        //        return true;
        //    }

        //    return false;
        //}
    }

}


    
