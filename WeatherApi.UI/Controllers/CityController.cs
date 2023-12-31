﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Data.DTOs;
using WeatherApi.Entity;
using WeatherApi.Service;
using WeatherApi.Service.Interfaces;

namespace WeatherApi.Controller;

[ApiController]
[Route("[controller]/api")]

public class CityController : ControllerBase
{
    private ICityService _cityService;
    private IMapper _mapper;

    public CityController(ICityService cityService, IMapper mapper)
    {
        _cityService = cityService;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria uma cidade
    /// </summary>
    [HttpPost("register-city")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostCity(
        [FromBody] CityRequestDto postCityDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var citySave =  await _cityService.Save(postCityDTO);

        return CreatedAtAction(nameof(GetCityForId), new { id = citySave.IdCity }, citySave);
    }

    /// <summary>
    /// Lista todas as cidades
    /// </summary>
    [HttpGet("cities/all")]
    public async Task<IEnumerable<City>> GetCitiesWithCityData()
    {
        var result = await _cityService.FindAll();
        return (IEnumerable<City>)result;
    }

    /// <summary>
    /// Lista uma cidade por id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCityForId(Guid id)
    {
        var city = await _cityService.FindById(id);
        return city is null ? NotFound("City not found") : Ok(city);
    }

}
