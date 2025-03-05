using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly OrderDetailsRepository _orderDetailsRepository;

        public OrderDetailsController(OrderDetailsRepository orderDetailsRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
        }

        [HttpGet]
        public IActionResult GetAllOrderDetails()
        {
            try
            {
                var orderDetailsList = _orderDetailsRepository.SelectAll();
                if (!orderDetailsList.Any())
                {
                    return NotFound("No order details found.");
                }
                return Ok(orderDetailsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderDetailById(int id)
        {
            var orderDetail = _orderDetailsRepository.SelectById(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return Ok(orderDetail);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrderDetail(int id)
        {
            var isDeleted = _orderDetailsRepository.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        #region Insert
        [HttpPost]
        public IActionResult InsertOrderDetail([FromBody] OrderDetailsModel orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("Invalid order detail data.");
            }

            try
            {
                _orderDetailsRepository.Insert(orderDetail);
                return CreatedAtAction(nameof(GetOrderDetailById), new { id = orderDetail.OrderDetailID }, orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public IActionResult UpdateOrderDetail(int id, [FromBody] OrderDetailsModel orderDetail)
        {
            if (orderDetail == null)
            {
                return BadRequest("Invalid order detail data.");
            }

            try
            {
                var existingOrderDetail = _orderDetailsRepository.SelectById(id);
                if (existingOrderDetail == null)
                {
                    return NotFound();
                }

                _orderDetailsRepository.Update(orderDetail);
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
        //public IActionResult GetOrdersDropdown()
        //{
        //    try
        //    {
        //        var ordersDropdown = _orderDetailsRepository.SelectDropdown();
        //        return Ok(ordersDropdown);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        //    }
        //}
        //#endregion
    }
}