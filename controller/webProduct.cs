using Microsoft.AspNetCore.Mvc;
using PosgresDb.Data;

namespace webProduct.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Web Product")]
    public class WebProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WebProductController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("web-product")]
        public IActionResult Get()
        {
            var data = _context.ProductsAdmin.Select(p => new
            {
                name = p.Name,
                description = p.Description,
                price = p.Price,
                stock = p.Stock
            }).ToArray();
            var response = new
            {
                data = data
            };
            return Ok(response);
        }
    }
}