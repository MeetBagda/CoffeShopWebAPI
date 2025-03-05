using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AudiSampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly StateRepository _stateRepository;

        public StateController(StateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        [HttpGet]
        public IActionResult GetAllStates()
        {
            try
            {
                var states = _stateRepository.SelectAll();
                if (states == null || !states.Any())
                {
                    return NotFound("No states found.");
                }
                return Ok(states);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetStateById(int id)
        {
            var state = _stateRepository.SelectByPK(id);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteState(int id)
        {
            var isDeleted = _stateRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        #region Insert
        [HttpPost]
        public IActionResult InsertState([FromBody] StateModel state)
        {
            if (state == null)
            {
                return BadRequest("Invalid state data.");
            }

            try
            {
                _stateRepository.Insert(state); // Call the insert method from repository
                return CreatedAtAction(nameof(GetStateById), new { id = state.StateID }, state); // Return 201 Created with the location of the new resource
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdateState(int id, [FromBody] StateModel state)
        {
            if (state == null)
            {
                return BadRequest("Invalid state data.");
            }

            try
            {
                var existingState = _stateRepository.SelectByPK(id);
                if (existingState == null)
                {
                    return NotFound(); // State not found for update
                }

                _stateRepository.Update(state); // Call the update method from repository
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