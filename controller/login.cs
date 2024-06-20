using login.Models;
using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using PosgresDb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using customer.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;




namespace login.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class loginAdminController : Controller
    {
        private readonly AppDbContext _context;
        public loginAdminController(AppDbContext context)
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
            var user = _context.UsersAdmin.SingleOrDefault(u => (u.Username == request.Username || u.Email == request.Username));
            if (user == null || !PasswordHasher.ValidatePassword(request.Password, user.Password))
            {
                return BadRequest("Username or password is incorrect");
            }
            var UserID = user.Id;
            return Ok(new
            {
                Message = "Welcome admin",
                UserID = UserID,
                Username = user.Username,
            });
        }
    }
    [ApiController]
    [Route("api/[controller]")]
    public class loginUserController : Controller
    {
        private readonly AppDbContext _context;
        private IConfiguration _config;
        public loginUserController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username or password is empty");
            }
            var user = _context.UsersCustomer.SingleOrDefault(u => (u.Username == request.Username || u.Email == request.Username));
            if (user == null || !PasswordHasher.ValidatePassword(request.Password, user.Password))
            {
                return BadRequest("Username or password is incorrect");
            }
            var UserID = user.Id;
            return Ok(new
            {
                Message = "Welcome Customer",
                UserID = UserID,
                Username = user.Username,
            });
        }
    }
}