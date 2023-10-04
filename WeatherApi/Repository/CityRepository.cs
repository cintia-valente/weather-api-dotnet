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

        public IEnumerable<City> FindAll()
        {
            return _context.CityData.Include(city => city.WeatherDataList);
        }

        public City FindAllByCityName(string cityName)
        {
            return _context.CityData.FirstOrDefault(c => c.Name == cityName);
        }

        public City? FindById(Guid idCity)
        {
            return _context.CityData.Include(city => city.WeatherDataList).FirstOrDefault(data => data.IdCity == idCity);

        }

    }

}


    
