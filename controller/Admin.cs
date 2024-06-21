using PosgresDb.Data;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;

namespace registerAdmin.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class RegisterAdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegisterAdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("register")]
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
            var newUserAdmin = new UserAdmin
            {
                Username = request.Username,
                Password = hashedPassword,
                Email = request.Email
            };
            try
            {
                _context.UsersAdmin.Add(newUserAdmin);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "User created successfully",
                    User = newUserAdmin
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
    [ApiExplorerSettings(GroupName = "Admin")]
    public class ProductAdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductAdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("create-product")]
        public IActionResult Post([FromBody] RequestProductAdmin request)
        {
            if (request.UserId == 0 || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Description) || request.Price == 0)
            {
                return BadRequest(new { message = "Name, description or price is empty" });
            }

            var productAdmin = _context.ProductsAdmin.SingleOrDefault(u => u.Name == request.Name);
            if (productAdmin != null)
            {
                return BadRequest(new { message = "Product already exists" });
            }
            var newProductAdmin = new ProductAdmin
            {
                UserId = request.UserId,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };
            try
            {
                _context.ProductsAdmin.Add(newProductAdmin);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "Product created successfully",
                    Product = newProductAdmin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

}