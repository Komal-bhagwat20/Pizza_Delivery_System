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
    public class UsersController : ControllerBase
    {
        // Object of interface of UserServices 
        private readonly IUserServices _userService;

        // Constructor of class
        public UsersController(IUserServices _userService)
        {
            this._userService = _userService;
        }

        // Sign up controller 
        // Http Create method 
        [HttpPost("signup")]
        public IActionResult Register(User user)
        {

            bool flag;
            try
            {
                flag = _userService.registerUser(user);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);//409
            }
            if (flag)
                return Ok("Register Successfully");
            else
                return StatusCode(500, "An error occurred");
        }

        // Login controller
        // Http Create method 
        [HttpPost("login")]
        public IActionResult Login([FromQuery] string Email, [FromQuery] string Password)
        {
            string JwtToken;
            try
            {
                JwtToken = _userService.LoginUser(Email, Password);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);//404
            }
            catch (IncorrectEmailOrPasswordException ex)
            {
                return Unauthorized(ex.Message);//401
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred: " + ex.Message);//500
            }
            //return JWT Token.
            return Ok("JWT Token: " + JwtToken);//200
        }

        // forgot Password controller 
        // Http Create method 
        [HttpPost("forgotpassword")]
        public IActionResult ForgetPass([FromQuery] string email)
        {
            string Forget;
            try
            {
                Forget = _userService.ForgetPassword(email);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);//409
            }
            return Ok("Your Password :" + Forget);
        }
        
        // View all orders controller 
        // Authoriser user 
        [Authorize(Roles = "user")]

        // Http get method
        [HttpGet("view-menu")]
        public IActionResult ViewMenu()
        {
            return Ok(_userService.ViewMenu());
        }

        // Create order controller 
        // Authoriser user 
        [Authorize(Roles = "user")]

        // Http Create method 
        [HttpPost("createOrder")]
        public IActionResult CreateOrders([FromBody] Orders order)
        {
            return Ok(_userService.CreateOrder(order));
        }

        // track order controller 
        // Authoriser user 
        [Authorize(Roles = "user")]
        // Http get method
        [HttpGet("track-order")]
        public IActionResult TrackOrders()
        {
            return Ok(_userService.TrackOrder());
        }


        // Authoriser user 
        [Authorize(Roles = "user")]
        // Http get method
        [HttpGet("order-history")]
        public IActionResult OrderHistory()
        {
            return Ok(_userService.OrderHistory());
        }
    }
}
