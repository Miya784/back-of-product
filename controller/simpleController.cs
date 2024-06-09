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
            var user = _context.Users.SingleOrDefault(u => (u.Username == request.Username || u.Email == request.Username));
            if (user == null || !PasswordHasher.ValidatePassword(request.Password, user.Password))
            {
                return BadRequest("Username or password is incorrect");
            }
            var UserID = user.Id;
            return Ok(new
            {
                Message = "Welcome",
                UserID = UserID,
                Username = user.Username,
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
            var newUser = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                Email = request.Email
            };

            try
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "User created successfully",
                    User = newUser
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class addProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public addProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product request)
        {
            if (string.IsNullOrEmpty(request.Name) || request.Price == 0)
            {
                return BadRequest("Name or price is empty");
            }
            var user = _context.Users.SingleOrDefault(u => u.Id == request.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var newProduct = new Product
            {
                UserId = request.UserId,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description
            };

            try
            {
                _context.Products.Add(newProduct);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "Product added successfully",
                    Product = newProduct.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}