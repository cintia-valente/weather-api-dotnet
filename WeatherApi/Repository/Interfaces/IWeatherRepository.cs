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

        Weather Save(Weather weather);

        IQueryable<Weather> FindAll();
        IEnumerable<Weather> FindAllByOrderByDateDesc(int page, int pageSize);
        IQueryable<Weather> FindAllByCityName(string cityName);
        IQueryable<Weather> FindByCityNextSixWeek(string cityName);
       
        Weather? FindByID(Guid idWeather);

        IEnumerable<Weather> FindByDates(List<DateTime> dates);

        void Update();
        bool DeleteById(Guid idWheater);
    }
}
