using AutoMapper;
using Moq;
using WeatherApi.Data.DTOs;
using WeatherApi.Entity;
using WeatherApi.Entity.Enums;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service;

namespace WeatherApiTest;

public class WeatherServiceTest
{

    private readonly Mock<IWeatherRepository> _weatherRepositoryMock;
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly IMapper _mapper;
    private readonly WeatherService _weatherService;

    public WeatherServiceTest()
    {
        _weatherRepositoryMock = new Mock<IWeatherRepository>();
        _cityRepositoryMock = new Mock<ICityRepository>();
       
        var configuration = new MapperConfiguration(cfg => {
            cfg.CreateMap<WeatherRequestDTO, Weather>();
            cfg.CreateMap<CityResponseDto, City>();
        });

        _mapper = configuration.CreateMapper();

        _weatherService = new WeatherService(_weatherRepositoryMock.Object, _cityRepositoryMock.Object, _mapper);
    }

    [Fact(DisplayName = "Dado um objeto Weather, quando salvar o objeto Weather, então chama os métodos FindByID e Save exatamente uma vez.")]
    public async Task SaveWeatherSucess()
    {
        // Arrange
        var validcityDto = new CityResponseDto
        {
            IdCity = Guid.NewGuid(),
            Name = "Porto Alegre"
        };

        var validWeatherDto = new WeatherRequestDTO
        {
            IdWeather = Guid.NewGuid(),
            Date = DateTime.Now,
            MaxTemperature = 20,
            MinTemperature = 10,
            Precipitation = 30,
            Humidity = 20,
            WindSpeed = 30,
            DayTime = DayTimeEnum.SOL,
            NightTime = NightTimeEnum.NEVE,
            IdCity = Guid.NewGuid(),
            City = validcityDto
        };

        var validWeather = new Weather(); // Crie um objeto Weather válido conforme necessário
        var validCity = new City(); // Crie um objeto City válido conforme necessário

        _cityRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
                          .ReturnsAsync(validCity);

        _weatherRepositoryMock.Setup(repo => repo.Save(It.IsAny<Weather>()))
                             .ReturnsAsync(validWeather); // Configura(Setup) o comportamento que deve ocorrer quando Save for chamado, Save aceita qualquer obj do tipo Weather e retorna um objeto do tipo Weather quando for chamado.

        // Act
        var result = await _weatherService.Save(validWeatherDto); //chama o método Save com o mock e armazena o resultado.

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validWeather, result);//verifica se o mock passado como parâmetro é igual ao obj retornado pelo método Save.

        _cityRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once); //verifica se FindByID foi chamado exatamente uma vez, passando como parâmetro qualquer argumento do tipo GUID.
        _weatherRepositoryMock.Verify(repo => repo.Save(It.IsAny<Weather>()), Times.Once); //verifica se Save foi chamado exatamente uma vez, passando como parâmetro o mock criado.
    }

    [Fact(DisplayName = "Dado um objeto Weather, quando passar valores de enums inválidos, então lança uma exceção.")]
    public async Task SaveWeatherEnumsError()
    {
        // Arrange
        var invalidWeatherDto = new WeatherRequestDTO
        {
            IdWeather = Guid.NewGuid(),
            Date = DateTime.Now,
            MaxTemperature = 20,
            MinTemperature = 10,
            Precipitation = 30,
            Humidity = 20,
            WindSpeed = 30,
            DayTime = (DayTimeEnum)10,
            NightTime = (NightTimeEnum).20,
            IdCity = Guid.NewGuid(),
            City = new CityResponseDto
            {
                IdCity = Guid.NewGuid(),
                Name = "Porto Alegre"
            }
        };

        //var WeatherDTO = _mapper.Map<WeatherRequestDTO>(invalidWeather);

        _weatherRepositoryMock.Setup(repo => repo.Save(It.IsAny<Weather>()))
        .Throws<ArgumentException>();

        // Act
        var exceptionSave = await Assert.ThrowsAsync<ArgumentException>(() => _weatherService.Save(invalidWeatherDto));

        // Assert
        Assert.Equal("Valores inválidos para enums DayTime e/ou NightTime.", exceptionSave.Message);
        _ = Assert.ThrowsAsync<ArgumentException>(() => _weatherService.Save(invalidWeatherDto)); // verifica se uma exceção é lançada durante a execução do método, mas não precisa do valor retornado por essa expressão.
    }

    [Fact(DisplayName = "Dado um id do Weather, então chama o método FindById exatamente uma vez.")]
    public async Task FindByIdSucess()
    {
        // Arrange
        Guid idWeather = Guid.NewGuid();

        _weatherRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .ReturnsAsync((Guid id) => new Weather { IdWeather = id });

        // Act
        var result = await _weatherService.FindById(idWeather);

        // Assert
        Assert.Equal(idWeather, result.IdWeather);
        _weatherRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact(DisplayName = "Dado um id do Weather, quando chamar o método FindById, então lança uma exceção.")]
    public async Task FindByIdError()
    {
        // Arrange
        Guid idWeather = Guid.NewGuid();

        _weatherRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .Throws<Exception>();

        // Act
        var exceptionFindById = Assert.ThrowsAsync<Exception>(() => _weatherService.FindById(idWeather));

        // Assert
        Assert.ThrowsAsync<Exception>(() => _weatherService.FindById(idWeather));
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então deve retornar uma lista de weather.")]
    public async Task FindAllSucess()
    {
        // Arrange
        var weatherList = new List<Weather>
        {
             new Weather
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now,
                MaxTemperature = 20,
                MinTemperature = 10,
                Precipitation = 30,
                Humidity = 20,
                WindSpeed = 30,
                DayTime = DayTimeEnum.SOL,
                NightTime = NightTimeEnum.NEVE,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            }
        };

        _weatherRepositoryMock.Setup(repo => repo.FindAll())
        .ReturnsAsync(weatherList.AsQueryable());

        // Act
        var result = await _weatherService.FindAll();

        // Assert
        Assert.Equal(weatherList, result);
        _weatherRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então lança uma exceção.")]
    public async Task FindAllError()
    {
        // Arrange
        _weatherRepositoryMock.Setup(repo => repo.FindAll())
        .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _weatherService.FindAll());

        // Assert
        Assert.ThrowsAsync<Exception>(() => _weatherService.FindAll());
    }

    [Fact(DisplayName = "Dado uma page e pageSize, então chama o método FindAllByOrderByDateDesc exatamente uma vez.")]
    public async Task FindAllPageSucess()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;

        var weatherList = new List<Weather>
        {
             new Weather
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now,
                MaxTemperature = 20,
                MinTemperature = 10,
                Precipitation = 30,
                Humidity = 20,
                WindSpeed = 30,
                DayTime = DayTimeEnum.SOL,
                NightTime = NightTimeEnum.NEVE,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            },
              new Weather
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(-1),
                MaxTemperature = 20,
                MinTemperature = 10,
                Precipitation = 30,
                Humidity = 20,
                WindSpeed = 30,
                DayTime = DayTimeEnum.SOL,
                NightTime = NightTimeEnum.NEVE,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            }
        };

        _weatherRepositoryMock.Setup(repo => repo.FindAllByOrderByDateDesc(It.IsAny<int>(), It.IsAny<int>()))
        .ReturnsAsync(weatherList.AsQueryable());

        // Act
        var result = await _weatherService.FindAllPage(page, pageSize);
        var sortedByDateDescending = result.OrderByDescending(x => x.Date);//ordena o retorno

        // Assert
        Assert.Equal(weatherList, result);
        Assert.Equal(pageSize, weatherList.Count());//verifica o número de itens da lista
        Assert.Equal(sortedByDateDescending, weatherList);//verifica se a lista ordenada é igual à lista mockada 
        _weatherRepositoryMock.Verify(repo => repo.FindAllByOrderByDateDesc(page, pageSize), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAllPage, então lança uma exceção.")]
    public async Task FindAllPageError()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;

        _weatherRepositoryMock.Setup(repo => repo.FindAllByOrderByDateDesc(page, pageSize))
        .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _weatherService.FindAllPage(page, pageSize));

        // Assert
        Assert.ThrowsAsync<Exception>(() => _weatherService.FindAllPage(page, pageSize));
    }

    [Fact(DisplayName = "Dado uma cidade, page e uma pageSize, então chama o método FindAllByCityName exatamente uma vez.")]
    public async Task FindAllPageByNameCitySucess()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;

        var weatherList = new List<Weather>
        {
             new Weather
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now,
                MaxTemperature = 15,
                MinTemperature = 5,
                Precipitation = 70,
                Humidity = 50,
                WindSpeed = 30,
                DayTime = DayTimeEnum.CHUVA,
                NightTime = NightTimeEnum.CHUVA,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            },
              new Weather
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(-1),
                MaxTemperature = 30,
                MinTemperature = 20,
                Precipitation = 30,
                Humidity = 20,
                WindSpeed = 30,
                DayTime = DayTimeEnum.SOL,
                NightTime = NightTimeEnum.LIMPO,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            }
        };

        _weatherRepositoryMock.Setup(repo => repo.FindAllByCityName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
        .ReturnsAsync(weatherList.AsQueryable());

        // Act
        var result = await _weatherService.FindAllPageByNameCity(weatherList[0].City.Name, page, pageSize);
        var sortedByDateDescending = result.OrderByDescending(x => x.Date);//ordena o retorno

        // Assert
        Assert.Equal(weatherList, result);
        Assert.Equal(weatherList[0].City.Name, sortedByDateDescending.First().City.Name);
        Assert.Equal(pageSize, weatherList.Count());
        Assert.Equal(sortedByDateDescending, weatherList);//verifica se a lista ordenada é igual à lista mockada 
        _weatherRepositoryMock.Verify(repo => repo.FindAllByCityName(weatherList[0].City.Name, page, pageSize), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAllPageByNameCity, então lança uma exceção.")]
    public void FindAllPageByNameCityError()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;

        var weatherList = new List<Weather>
            {
                 new Weather
                {
                    IdWeather = Guid.NewGuid(),
                    Date = DateTime.Now,
                    MaxTemperature = 15,
                    MinTemperature = 5,
                    Precipitation = 70,
                    Humidity = 50,
                    WindSpeed = 30,
                    DayTime = DayTimeEnum.CHUVA,
                    NightTime = NightTimeEnum.CHUVA,
                    IdCity = Guid.NewGuid(),
                    City = new City
                    {
                        IdCity = Guid.NewGuid(),
                        Name = "Porto Alegre"
                    }
                },
                  new Weather
                {
                    IdWeather = Guid.NewGuid(),
                    Date = DateTime.Now.AddDays(-1),
                    MaxTemperature = 30,
                    MinTemperature = 20,
                    Precipitation = 30,
                    Humidity = 20,
                    WindSpeed = 30,
                    DayTime = DayTimeEnum.SOL,
                    NightTime = NightTimeEnum.LIMPO,
                    IdCity = Guid.NewGuid(),
                    City = new City
                    {
                        IdCity = Guid.NewGuid(),
                        Name = "Porto Alegre"
                    }
                }
            };

        _weatherRepositoryMock.Setup(repo => repo.FindAllByCityName(weatherList[0].City.Name, page, pageSize))
           .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _weatherService.FindAllPageByNameCity(weatherList[0].City.Name, page, pageSize));

        // Assert
        Assert.ThrowsAsync<Exception>(() => _weatherService.FindAllPageByNameCity(weatherList[0].City.Name, page, pageSize));
    }

    [Fact(DisplayName = "Dado uma cidade, então chama o método FindByCityNextSixWeek exatamente uma vez.")]
    public async Task GetWeatherForNext7DaysSucess()
    {
        // Arrange
        var weatherList = new List<Weather>
        {
             new Weather
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(-1),
                MaxTemperature = 15,
                MinTemperature = 5,
                Precipitation = 70,
                Humidity = 50,
                WindSpeed = 30,
                DayTime = DayTimeEnum.CHUVA,
                NightTime = NightTimeEnum.CHUVA,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            },
              new Weather
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now,
                MaxTemperature = 30,
                MinTemperature = 20,
                Precipitation = 30,
                Humidity = 20,
                WindSpeed = 30,
                DayTime = DayTimeEnum.SOL,
                NightTime = NightTimeEnum.LIMPO,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            }
        };

        _weatherRepositoryMock.Setup(repo => repo.FindByCityNextSixWeek(It.IsAny<string>()))
        .ReturnsAsync(weatherList.AsQueryable());

        // Act
        var result = await _weatherService.GetWeatherForNext7Days(weatherList[0].City.Name);
        var sortedByDateAscending = result.OrderBy(x => x.Date);//ordena o retorno

        // Assert
        Assert.Equal(weatherList, result);
        Assert.Equal(weatherList[0].City.Name, sortedByDateAscending.First().City.Name);
        Assert.Equal(sortedByDateAscending, weatherList);
        _weatherRepositoryMock.Verify(repo => repo.FindByCityNextSixWeek(weatherList[0].City.Name), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método GetWeatherForNext7Days, então lança uma exceção.")]
    public async Task GetWeatherForNext7DaysError()
    {
        // Arrange
        var weatherList = new List<Weather>
            {
                 new Weather
                {
                    IdWeather = Guid.NewGuid(),
                    Date = DateTime.Now.AddDays(-1),
                    MaxTemperature = 15,
                    MinTemperature = 5,
                    Precipitation = 70,
                    Humidity = 50,
                    WindSpeed = 30,
                    DayTime = DayTimeEnum.CHUVA,
                    NightTime = NightTimeEnum.CHUVA,
                    IdCity = Guid.NewGuid(),
                    City = new City
                    {
                        IdCity = Guid.NewGuid(),
                        Name = "Porto Alegre"
                    }
                },
                  new Weather
                {
                    IdWeather = Guid.NewGuid(),
                    Date = DateTime.Now,
                    MaxTemperature = 30,
                    MinTemperature = 20,
                    Precipitation = 30,
                    Humidity = 20,
                    WindSpeed = 30,
                    DayTime = DayTimeEnum.SOL,
                    NightTime = NightTimeEnum.LIMPO,
                    IdCity = Guid.NewGuid(),
                    City = new City
                    {
                        IdCity = Guid.NewGuid(),
                        Name = "Porto Alegre"
                    }
                }
            };

        _weatherRepositoryMock.Setup(repo => repo.FindByCityNextSixWeek(weatherList[0].City.Name))
           .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _weatherService.GetWeatherForNext7Days(weatherList[0].City.Name));

        // Assert
        Assert.ThrowsAsync<Exception>(() => _weatherService.GetWeatherForNext7Days(weatherList[0].City.Name));
    }

    //[Fact(DisplayName = "Dado um objeto Weather, quando editar o objeto, então chama os métodos FindById e Update exatamente uma vez.")]
    //public async Task UpdateSucess()
    //{
    //    // Arrange
    //    var weather = new Weather
    //    {
    //        IdWeather = Guid.NewGuid(),
    //        Date = DateTime.Now,
    //        MaxTemperature = 20,
    //        MinTemperature = 10,
    //        Precipitation = 30,
    //        Humidity = 20,
    //        WindSpeed = 30,
    //        DayTime = DayTimeEnum.SOL,
    //        NightTime = NightTimeEnum.NEVE,
    //        IdCity = Guid.NewGuid(),
    //        City = new City
    //        {
    //            IdCity = Guid.NewGuid(),
    //            Name = "Porto Alegre"
    //        }
    //    };

    //    var updateWeather = new Weather
    //    {
    //        IdWeather = Guid.NewGuid(),
    //        Date = DateTime.Now,
    //        MaxTemperature = 30,
    //        MinTemperature = 20,
    //        Precipitation = 55,
    //        Humidity = 10,
    //        WindSpeed = 30,
    //        DayTime = DayTimeEnum.SOL,
    //        NightTime = NightTimeEnum.CHUVA,
    //        IdCity = Guid.NewGuid(),
    //        City = new City
    //        {
    //            IdCity = Guid.NewGuid(),
    //            Name = "Porto Alegre"
    //        }
    //    };

    //    _weatherRepositoryMock.Setup(repo => repo.FindById(weather.IdWeather))
    //       .ReturnsAsync(weather);

    //    Weather capturedWeather = null;

    //    _weatherRepositoryMock.Setup(repo => repo.Update(It.IsAny<Guid>(), It.IsAny<Weather>()))
    //    .Callback<Guid, Weather>((id, weather) =>
    //    {
    //        capturedWeather = weather;
    //    });

    //    // Act
    //    _weatherService.Update(weather.IdWeather, updateWeather);

    //    // Assert
    //    Assert.NotNull(capturedWeather);
    //    Assert.Equal(updateWeather, capturedWeather);

    //    _weatherRepositoryMock.Verify(repo => repo.FindById(weather.IdWeather), Times.Once);
    //    _weatherRepositoryMock.Verify(repo => repo.Update(weather.IdWeather, updateWeather), Times.Once);
    //}

    //[Fact(DisplayName = "Dado uma chamada ao método UpdateError, então lança uma exceção.")]
    //public async Task UpdateError()
    //{
    //    // Arrange
    //    var weather = new Weather
    //    {
    //        IdWeather = Guid.NewGuid(),
    //        Date = DateTime.Now,
    //        MaxTemperature = 20,
    //        MinTemperature = 10,
    //        Precipitation = 30,
    //        Humidity = 20,
    //        WindSpeed = 30,
    //        DayTime = DayTimeEnum.SOL,
    //        NightTime = NightTimeEnum.NEVE,
    //        IdCity = Guid.NewGuid(),
    //        City = new City
    //        {
    //            IdCity = Guid.NewGuid(),
    //            Name = "Porto Alegre"
    //        }
    //    };

    //    var updateWeather = new Weather
    //    {
    //        IdWeather = Guid.NewGuid(),
    //        Date = DateTime.Now,
    //        MaxTemperature = 30,
    //        MinTemperature = 20,
    //        Precipitation = 55,
    //        Humidity = 10,
    //        WindSpeed = 30,
    //        DayTime = DayTimeEnum.SOL,
    //        NightTime = NightTimeEnum.CHUVA,
    //        IdCity = Guid.NewGuid(),
    //        City = new City
    //        {
    //            IdCity = Guid.NewGuid(),
    //            Name = "Porto Alegre"
    //        }
    //    };

    //    _weatherRepositoryMock.Setup(repo => repo.FindById(weather.IdWeather))
    //       .Throws<Exception>();

    //    Weather capturedWeather = null;

    //    _weatherRepositoryMock.Setup(repo => repo.Update(It.IsAny<Guid>(), It.IsAny<Weather>()))
    //     .Throws<Exception>();

    //    // Act
    //    var exceptionSave = Assert.ThrowsAsync<Exception>(() => _weatherService.Update(updateWeather.IdWeather, weather));

    //    // Assert
    //    Assert.ThrowsAsync<Exception>(() => _weatherService.Update(updateWeather.IdWeather, weather));
    //}

    [Fact(DisplayName = "Dado um id do Weather, quando editar o objeto, então chama o método DeleteById exatamente uma vez.")]
    public async Task DeleteByIdSucess()
    {
        // Arrange
        Guid idWeather = Guid.NewGuid();

        _weatherRepositoryMock.Setup(repo => repo.DeleteById(It.IsAny<Guid>()))
        .ReturnsAsync(true);

        // Act
        var result = await _weatherService.DeleteById(idWeather);

        // Assert
        Assert.True(result);
        _weatherRepositoryMock.Verify(repo => repo.DeleteById(idWeather), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método DeleteById, então lança uma exceção.")]
    public async Task DeleteByIdError()
    {
        // Arrange
        Guid idWeather = Guid.NewGuid();

        _weatherRepositoryMock.Setup(repo => repo.DeleteById(idWeather))
        .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _weatherService.DeleteById(idWeather));

        // Assert
        Assert.ThrowsAsync<Exception>(() => _weatherService.DeleteById(idWeather));
    }
}