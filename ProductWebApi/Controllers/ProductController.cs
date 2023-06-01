using Microsoft.AspNetCore.Mvc;
using ProductWebApi.Models;

namespace ProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _context.Products;
        }

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<Product>> GetById(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return Created("api", product.ProductId);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Product product)
        {
            var productFound = await _context.Products.FindAsync(product.ProductId);
            if (productFound != null)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{productId:int}")]
        public async Task<ActionResult> Delete(int productId)
        {
            var productFound = await _context.Products.FindAsync(productId);
            if (productFound != null)
            {
                _context.Remove(productFound);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
