using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;
using WeatherApi.Models;

namespace WeatherApi.Repository.Interfaces
{
    public interface IWeatherRepository
    {
        //Weather GetById(int id);
        //IEnumerable<Weather> GetAll();
        //void Add(Weather entity);
        //void Update(T entity);
        //void Delete(T entity);

        void Save(Weather weather);

        IEnumerable<Weather> FindAll();
        IEnumerable<Weather> FindAllByOrderByDateDesc(int page, int pageSize);
        IEnumerable<Weather> FindAllByCityNameIgnoreCase(string cityName, int page, int pageSize);
        //   IEnumerable<Weather> FindAllPage(int page, int pageSize);
        //  IEnumerable<Weather> FindAllPageByNameCity(string cityName, int page, int pageSize);

        IEnumerable<Weather> FindByCityNextSixWeek(string cityName);

        // Page<WeatherDataEntity> FindByCityNextSixWeek(String cityName, Pageable pageable);
       // IEnumerable<Weather> FindByCityNameIgnoreCaseAndDateBetween(string cityName, DateTime startDate, DateTime endDate, string sortField, bool ascending);
        Weather? FindByID(Guid idWeather);

        void Update();
        bool DeleteById(Guid idWheater);
    }
}
