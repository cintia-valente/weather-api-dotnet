using WeatherApi.Data;
using WeatherApi.Models;
using WeatherApi.Repository.Interfaces;

namespace WeatherApi.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly WeatherContext _context; // Substitua pelo seu contexto de banco de dados

        public CityRepository(WeatherContext context)
        {
            _context = context;
        }

        public void Save(City city)
        {
            _context.CityData.Add(city);
            _context.SaveChanges();
        }

        //public IEnumerable<City> FindAll()
        //{
        //    return _context.CityData.ToList();
        //}


        public City? FindByID(Guid idCity)
        {
            return _context.CityData.FirstOrDefault(data => data.IdCity == idCity);

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


    
