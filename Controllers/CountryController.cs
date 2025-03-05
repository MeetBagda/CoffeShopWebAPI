using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AudiSampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryRepository _countryRepository;

        public CountryController(CountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        #region GetAllCountries
        [HttpGet]
        public IActionResult GetAllCountries()
        {
            try
            {
                var country = _countryRepository.SelectAll();
                if (country == null)
                {
                    return NotFound("No countries found.");
                }
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion

        [HttpGet("{id}")]
        public IActionResult GetCountryById(int id)
        {
            var country = _countryRepository.SelectByPK(id);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            var isDeleted = _countryRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        #region Insert
        [HttpPost]
        public IActionResult InsertCountry(CountryModel country)
        {
            if (country == null)
            {
                return BadRequest("Invalid state data.");
            }

            try
            {
                _countryRepository.Insert(country); // Call the insert method from repository
                return CreatedAtAction(nameof(GetCountryById), new { id = country.CountryID }, country); // Return 201 Created with the location of the new resource
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdateState(int id, CountryModel country)
        {
            if (country == null)
            {
                return BadRequest("Invalid state data.");
            }

            try
            {
                var existingState = _countryRepository.SelectByPK(id);
                if (existingState == null)
                {
                    return NotFound(); // State not found for update
                }

                _countryRepository.Update(country); // Call the update method from repository
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
