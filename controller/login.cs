using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using login.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PosgresDb.Data;

namespace login.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Admin")]
    public class loginAdminController : Controller
    {
        private readonly AppDbContext _context;
        private IConfiguration _configuration;
        public loginAdminController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            var jwtKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                Audience = _configuration["Jwt:Audience"], // Set the audience claim
                Issuer = _configuration["Jwt:Issuer"], // Set the issuer claim
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated Token: {tokenString}"); // Log the generated token

            return Ok(new
            {
                Token= tokenString,
                Message = "Welcome admin",
                UserID = UserID,
                Username = user.Username,
            });
        }
    }
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Customer")]
    public class loginUserController : Controller
    {
        private readonly AppDbContext _context;
        private IConfiguration _configuration;
        public loginUserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            var jwtKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                Audience = _configuration["Jwt:Audience"], // Set the audience claim
                Issuer = _configuration["Jwt:Issuer"], // Set the issuer claim
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(jwtKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated Token: {tokenString}"); // Log the generated token

            return Ok(new
            {
                Token= tokenString,
                Message = "Welcome Customer",
                UserID = UserID,
                Username = user.Username,
            });
        }
    }
}