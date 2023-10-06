using Moq;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service;
using WeatherApi.Entity;

namespace WeatherApiTest;

public class CityServiceTest
{
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly CityService _cityService;

    public CityServiceTest()
    {
        _cityRepositoryMock = new Mock<ICityRepository>();

        _cityService = new CityService(_cityRepositoryMock.Object);
    }

    [Fact(DisplayName = "Dado um objeto City, quando salvar o objeto Weather, então chama os métodos FindByID e Save exatamente uma vez.")]
    public void SaveWeatherSucess()
    {
        // Arrange
        var validCity = new City
        {
            IdCity = Guid.NewGuid(),
            Name = "Porto Alegre"
        };

        _cityRepositoryMock.Setup(repo => repo.Save(It.IsAny<City>()))
       .Returns((City c) => c); 

        // Act
        var result = _cityService.Save(validCity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validCity, result);

        _cityRepositoryMock.Verify(repo => repo.Save(validCity), Times.Once); 
    }

    [Fact(DisplayName = "Dado uma chamada de Save, então lança uma exceção.")]
    public void SaveWeatherEnumsError()
    {
        // Arrange
        var invalidCity = new City
        {
            IdCity = Guid.NewGuid(),
            Name = "Porto Alegre"
        };

        _cityRepositoryMock.Setup(repo => repo.Save(It.IsAny<City>()))
        .Throws<ArgumentException>();

        // Act
        var exceptionSave = Assert.Throws<ArgumentException>(() => _cityService.Save(invalidCity));

        // Assert
        Assert.Throws<ArgumentException>(() => _cityService.Save(invalidCity));
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então deve retornar uma lista de city.")]
    public void FindAllSucess()
    {
        // Arrange
        var cityList = new List<City>
        {
              new City
               {
                IdCity = Guid.NewGuid(),
                Name = "Porto Alegre"
              }
        };

        _cityRepositoryMock.Setup(repo => repo.FindAll())
        .Returns(cityList.AsQueryable());

        // Act
        var result = _cityService.FindAll();

        // Assert
        Assert.Equal(cityList, result);
        _cityRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então lança uma exceção.")]
    public void FindAllError()
    {
        // Arrange
        _cityRepositoryMock.Setup(repo => repo.FindAll())
        .Throws<Exception>();

        // Act
        var exceptionSave = Assert.Throws<Exception>(() => _cityService.FindAll());

        // Assert
        Assert.Throws<Exception>(() => _cityService.FindAll());
    }

    [Fact(DisplayName = "Dado um id da City, então chama o método FindById exatamente uma vez.")]
    public void FindByIdSucess()
    {
        // Arrange
        Guid idCity = Guid.NewGuid();

        _cityRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .Returns((Guid id) => new City { IdCity = id });

        // Act
        var result = _cityService.FindById(idCity);

        // Assert
        Assert.Equal(idCity, result.IdCity);
        _cityRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact(DisplayName = "Dado um id da City, quando chamar o método FindById, então lança uma exceção.")]
    public void FindByIdError()
    {
        // Arrange
        Guid idCity = Guid.NewGuid();

        _cityRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .Throws<Exception>();

        // Act
        var exceptionFindById = Assert.Throws<Exception>(() => _cityService.FindById(idCity));

        // Assert
        Assert.Throws<Exception>(() => _cityService.FindById(idCity));
    }
}
