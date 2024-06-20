using PosgresDb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using customer.Models;

namespace registerAdmin.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterCustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegisterCustomerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterAdminRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { message = "Username, password or email is empty" });
            }

            var userAdmin = _context.UsersAdmin.SingleOrDefault(u => u.Username == request.Username);
            if (userAdmin != null)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            var newUsersCustomer = new UserCustomer
            {
                Username = request.Username,
                Password = hashedPassword,
                Email = request.Email
            };
            try
            {
                _context.UsersCustomer.Add(newUsersCustomer);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "User created successfully",
                    User = newUsersCustomer
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
    public class CustomerbuyController : Controller
    {
        private readonly AppDbContext _context;
        public CustomerbuyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Port([FromBody] BuyRequest request)
        {
            if (string.IsNullOrEmpty(request.ProductName) || request.UserId == 0 || request.Price == 0.0m)
            {
                return BadRequest("Product name, UserId, or Price is invalid or empty");
            }
            var price = request.Price;
            var user = _context.UsersCustomer.SingleOrDefault(p => p.Id == request.UserId);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var product = _context.ProductsAdmin.SingleOrDefault(p => p.Name == request.ProductName);
            if (product == null)
            {
                return BadRequest("Product not found");
            }
            if (product.Stock < 1)
            {
                return BadRequest("Product out of stock");
            }
            var newHistory = new HistoryCustomer
            {
                UserId = user.Id,
                Name = product.Name,
                Price = price,
                Description = product.Description,
                DateTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };
            try
            {
                product.Stock -= 1;
                _context.ProductsAdmin.Update(product);
                _context.HistorysCustomer.Add(newHistory);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "Buy success",
                    Product = product
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}