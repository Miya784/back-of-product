using PosgresDb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;

namespace registerAdmin.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterAdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegisterAdminController(AppDbContext context)
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
}