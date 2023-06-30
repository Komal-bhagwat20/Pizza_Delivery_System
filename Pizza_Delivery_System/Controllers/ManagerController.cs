using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pizza_Delivery_System.Exceptions;
using Pizza_Delivery_System.InterFaces;
using Pizza_Delivery_System.Models;
using System.Data;

namespace Pizza_Delivery_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        // object of managerServices interface
        private readonly IManagerServices managerService;
        // Contructor
        public ManagerController(IManagerServices managerService)
        {
            this.managerService = managerService;
        }

        // Login controller for manager 
        // Http Create method 
        [HttpPost("login")]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {

            string jwttoken;
            try
            {
                // Fetch Login method through interface 
                jwttoken = managerService.LoginManager(username, password);

            }
            catch (IncorrectEmailOrPasswordException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);//409
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok("Jwt Token :" + jwttoken);
        }

        // View all orders controller 
        // Authoriser manager 
        [Authorize(Roles = "manager")]
        // Http get method
        [HttpGet("view-all-orders")]
        public IActionResult ViewAllOrders()
        {
            // Fetch method through interface 
            return Ok(managerService.ViewAllOrders());
        }

        // Manage orders controller 
        // Authoriser manager 
        [Authorize(Roles = "manager")]
        [HttpPut("manage-order")]
        public IActionResult ManageOrder([FromQuery] string order_Id, [FromQuery] string order_status)
        {
            try
            {
                // Fetch method through interface 
                managerService.ManageOrder(order_Id, order_status);
            }
            catch (OrderNotFound ex)
            {
                return NotFound(ex.Message);
            }
            return Ok("Order Status Update Success");
        }

        // Order Detaills Controller 
        // Authoriser manager 
        [Authorize(Roles = "manager")]

        // Http get method
        [HttpGet("order-details")]
        public IActionResult OrderDetails(string order_id)
        {
            var orderDetails = new OrderDetails();
            try
            {
                // Fetch method through interface 
                orderDetails = managerService.OrderDetails(order_id);
            }
            catch (OrderNotFound ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }
    }
}
