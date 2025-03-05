using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillRepository _billRepository;

        public BillController(BillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        [HttpGet]
        public IActionResult GetAllBills()
        {
            try
            {
                var bills = _billRepository.SelectAll();
                if (!bills.Any())
                {
                    return NotFound("No bills found.");
                }
                return Ok(bills);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBillById(int id)
        {
            var bill = _billRepository.SelectById(id);
            if (bill == null)
            {
                return NotFound();
            }
            return Ok(bill);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBill(int id)
        {
            var isDeleted = _billRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        #region Insert
        [HttpPost]
        public IActionResult InsertBill([FromBody] BillModel bill)
        {
            if (bill == null)
            {
                return BadRequest("Invalid bill data.");
            }

            try
            {
                _billRepository.Insert(bill);
                return CreatedAtAction(nameof(GetBillById), new { id = bill.BillID }, bill);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdateBill(int id, [FromBody] BillModel bill)
        {
            if (bill == null)
            {
                return BadRequest("Invalid bill data.");
            }

            try
            {
                var existingBill = _billRepository.SelectById(id);
                if (existingBill == null)
                {
                    return NotFound();
                }

                _billRepository.Update(bill);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion

        //#region Dropdown
        //[HttpGet("dropdown")]
        //public IActionResult GetBillsDropdown()
        //{
        //    try
        //    {
        //        var billsDropdown = _billRepository.SelectDropdown();
        //        return Ok(billsDropdown);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        //    }
        //}
        //#endregion
    }
}