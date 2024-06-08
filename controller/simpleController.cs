using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PosgresDb.Data;
using simpleWebApp.Models;

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
        private readonly AppDbContext _context;
        public loginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username or password is empty");
            }
            // Find the user with the provided username or email
            var user = _context.Users.SingleOrDefault(u => (u.Username == request.Username || u.Email == request.Username));
            // Return a 400 Bad Request response if the user is not found or the password is incorrect
            if (user == null || !PasswordHasher.ValidatePassword(request.Password, user.Password))
            {
                return BadRequest("Username or password is incorrect");
            }

            return Ok(new
            {
                Message = "Welcome",
                Username = user.Username
            });
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

            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            // Create a new User object with data from the request
            var newUser = new User
            {
                Username = request.Username,
                Password = hashedPassword,
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