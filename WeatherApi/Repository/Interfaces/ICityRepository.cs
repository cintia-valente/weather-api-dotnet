using WeatherApi.Models;

namespace WeatherApi.Repository.Interfaces
{
    public interface ICityRepository
    {

        void Save(City city);
     //   IEnumerable<City> FindAll();
      
        City? FindByID(Guid idCity);

       // bool DeleteById(Guid idCity);
    }
}

