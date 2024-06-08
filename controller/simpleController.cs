using Microsoft.AspNetCore.Mvc;
using simpleWebApp.Models;
using PosgresDb.Data;

namespace simpleWebApp.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class simpleController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class loginController : Controller
    {
        [HttpPost]
        public IActionResult post([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username or password is empty");
            }
            var response = new
            {
                Massage = "Welcome",
                Username = request.Username,
                Password = request.Password
            };
            return Ok(response);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class registerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public registerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Username, password, or email is empty");
            }

            // Create a new User object with data from the request
            var newUser = new User
            {
                Username = request.Username,
                Password = request.Password,
                Email = request.Email
            };

            try
            {
                // Add the new user to the database
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Return a success response with the newly created user data
                return Ok(new
                {
                    Message = "User created successfully",
                    User = newUser
                });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error if an exception occurs
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}