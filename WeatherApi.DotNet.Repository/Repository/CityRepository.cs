using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherApi.Entity;
using WeatherApi.Persistence;
using WeatherApi.Repository.Interfaces;

namespace WeatherApi.Repository;

public class CityRepository : ICityRepository
{
    private readonly WeatherContext _context; // Substitua pelo seu contexto de banco de dados
    private IMapper _mapper;

    public CityRepository(WeatherContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<City> Save(City city)
    {
        EntityEntry<City?> cityEntity = _context.CityData.Add(city);
        await _context.SaveChangesAsync();
        var cityConvert = _mapper.Map<City>(cityEntity.Entity);
        return cityConvert;
    }

    public async Task<IEnumerable<City>> FindAll()
    {
        return await _context.CityData.Include(city => city.WeatherDataList).ToListAsync(); 
    }

    public async Task<City> FindAllByCityName(string cityName)
    {
        return await _context.CityData.FirstOrDefaultAsync(c => c.Name == cityName); 
    }

    public async Task<City?> FindById(Guid idCity)
    {
        return await _context.CityData.Include(city => city.WeatherDataList).FirstOrDefaultAsync(data => data.IdCity == idCity);

    }

}



