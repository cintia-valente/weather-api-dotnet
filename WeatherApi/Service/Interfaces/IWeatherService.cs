using WeatherApi.Models;

namespace WeatherApi.Service.Interfaces;

public interface IWeatherService
{
    //T GetEstadoById(int id);
    //IEnumerable<T> GetAllEstados();
    //void AddEstado(T estado);
    //void UpdateEstado(T estado);
    //void DeleteEstado(int id);

    Weather Save(Weather weather);
    IEnumerable<Weather> FindAll();
    IEnumerable<Weather> FindAllPageByNameCity(string cityName, int page, int pageSize);
    Weather FindById(Guid id);

    IEnumerable<Weather> FindAllPage(int page, int pageSize);
    IEnumerable<Weather> GetWeatherForNext7Days(string cityName);

    Weather Update(Guid idWheaterData, Weather weather);

    bool DeleteById(Guid idWheaterData);


}
