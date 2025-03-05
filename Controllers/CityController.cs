
using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AudiSampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly CityRepository _cityRepository;

        public CityController(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public IActionResult GetAllCities()
        {
            try
            {
                var cities = _cityRepository.SelectAll();
                if (cities == null)
                {
                    return NotFound("No cities found.");
                }
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCityById(int id)
        {
            var city = _cityRepository.SelectByPK(id);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCity(int id)
        {
            var isDeleted = _cityRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        #region Insert
        [HttpPost]
        public IActionResult InsertCity([FromBody] CityModel city)
        {
            if (city == null )
            {
                return BadRequest("Invalid city data.");
            }

            try
            {
                _cityRepository.Insert(city); // Call the insert method from repository
                return CreatedAtAction(nameof(GetCityById), new { id = city.CityID }, city); // Return 201 Created with the location of the new resource
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id, [FromBody] CityModel city)
        {
            if (city == null )
            {
                return BadRequest("Invalid city data.");
            }

            try
            {
                var existingCity = _cityRepository.SelectByPK(id);
                if (existingCity == null)
                {
                    return NotFound(); // City not found for update
                }

                _cityRepository.Update(city); // Call the update method from repository
                return NoContent(); // Return 204 No Content on successful update
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion
    }
}