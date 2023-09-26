using WeatherApi.Models;

namespace WeatherApi.Repository.Interfaces
{
    public interface ICityRepository
    {

        void Save(City city);
     //   IEnumerable<City> FindAll();
      
        City? FindByID(long idCity);

       // bool DeleteById(Guid idCity);
    }
}

