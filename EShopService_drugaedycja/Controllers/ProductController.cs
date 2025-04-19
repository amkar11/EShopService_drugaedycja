using Microsoft.AspNetCore.Mvc;
using EShop.Application;
using EShop.Domain;
using EShop.Domain.ProductProvidersExceptions;
using System.Net;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService_drugaedycja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;
        public ProductController (IProductService productService) 
        {
            _productService = productService;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> ShowAllProductsAsync()
        {
            var products = await _productService.ShowAllProductsAsync();
            return Ok(products);
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody] Product product)
        {
            try
            {
                await _productService.AddProductAsync(product);
                return Created($"api/Product/{product.Id}", product);
            }
            catch (ProductAllreadyExistsException ex)
            {
                return BadRequest(new { error = $"{ex.Message}", code = HttpStatusCode.BadRequest } );
            }

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeProductAsync(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest("Id you entered and actual product Id does not match!");
            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (ProductDoesNotExistException ex) 
            {
                return BadRequest(new { error = $"{ex.Message}", code = HttpStatusCode.BadRequest });
            }
        }
    }
}
