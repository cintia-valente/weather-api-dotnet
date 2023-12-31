﻿using Moq;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service;
using WeatherApi.Entity;
using AutoMapper;
using WeatherApi.Data.DTOs;
using WeatherApi.Service.Interfaces;

namespace WeatherApiTest;

public class CityServiceTest
{
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly IMapper _mapper;
    private readonly CityService _cityService;

    public CityServiceTest()
    {
        _cityRepositoryMock = new Mock<ICityRepository>();

        var configuration = new MapperConfiguration(cfg => {
            cfg.CreateMap<CityRequestDto, City>();
            cfg.CreateMap<CityResponseDto, City>();
        });

        _mapper = configuration.CreateMapper();

        _cityService = new CityService(_cityRepositoryMock.Object, _mapper);
    }

    [Fact(DisplayName = "Dado um objeto City, quando salvar o objeto, então chama os métodos FindByID e Save exatamente uma vez.")]
    public async Task SaveCitySucess()
    {
        // Arrange
        var validCityDto = new CityRequestDto
        {
            IdCity = Guid.NewGuid(),
            Name = "Porto Alegre"
        };

        var validCity = new City();

        _cityRepositoryMock.Setup(repo => repo.Save(It.Is<City>(c => c.IdCity == validCityDto.IdCity && c.Name == validCityDto.Name)))
                 .ReturnsAsync(validCity);

        // Act
        var result = await _cityService.Save(validCityDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validCity, result);

        _cityRepositoryMock.Verify(repo => repo.Save(It.IsAny<City>()), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada de Save, então lança uma exceção.")]
    public async Task SaveCityError()
    {
        // Arrange
        var invalidCityDto = new CityRequestDto
        {
            IdCity = Guid.NewGuid(),
            Name = "Porto Alegre"
        };

        _cityRepositoryMock.Setup(repo => repo.Save(It.IsAny<City>()))
        .Throws<ArgumentException>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<ArgumentException>(() => _cityService.Save(invalidCityDto));

        // Assert
        Assert.ThrowsAsync<ArgumentException>(() => _cityService.Save(invalidCityDto));
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então deve retornar uma lista de city.")]
    public async Task FindAllSucess()
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
        .ReturnsAsync(cityList.AsQueryable());

        // Act
        var result = await _cityService.FindAll();

        // Assert
        Assert.Equal(cityList, result);
        _cityRepositoryMock.Verify(repo => repo.FindAll(), Times.Once);
    }

    [Fact(DisplayName = "Dado uma chamada ao método FindAll, então lança uma exceção.")]
    public async Task FindAllError()
    {
        // Arrange
        _cityRepositoryMock.Setup(repo => repo.FindAll())
        .Throws<Exception>();

        // Act
        var exceptionSave = Assert.ThrowsAsync<Exception>(() => _cityService.FindAll());

        // Assert
        Assert.ThrowsAsync<Exception>(() => _cityService.FindAll());
    }

    [Fact(DisplayName = "Dado um id da City, então chama o método FindById exatamente uma vez.")]
    public async Task FindByIdSucess()
    {
        // Arrange
        Guid idCity = Guid.NewGuid();

        _cityRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .ReturnsAsync((Guid id) => new City { IdCity = id });

        // Act
        var result = await _cityService.FindById(idCity);

        // Assert
        Assert.Equal(idCity, result.IdCity);
        _cityRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact(DisplayName = "Dado um id da City, quando chamar o método FindById, então lança uma exceção.")]
    public async Task FindByIdError()
    {
        // Arrange
        Guid idCity = Guid.NewGuid();

        _cityRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
        .Throws<Exception>();

        // Act
        var exceptionFindById = Assert.ThrowsAsync<Exception>(() => _cityService.FindById(idCity));

        // Assert
        Assert.ThrowsAsync<Exception>(() => _cityService.FindById(idCity));
    }
}
