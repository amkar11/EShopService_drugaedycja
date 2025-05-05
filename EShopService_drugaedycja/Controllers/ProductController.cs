using Microsoft.AspNetCore.Mvc;
using EShop.Application;
using EShop.Domain;
using EShop.Domain.ProductProvidersExceptions;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService_drugaedycja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;
        public readonly ICacheService _cacheService;
        public readonly IRedisCacheService _redisCacheService;
        private readonly string _cacheKeyProducts = "productsCacheKey";
        private readonly string _cacheKeyProduct = "productCacheKey";
        public ProductController (IProductService productService, ICacheService cacheService, IRedisCacheService redisCacheService) 
        {
            _productService = productService;
            _cacheService = cacheService;
            _redisCacheService = redisCacheService;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> ShowAllProductsAsync()
        {

            var products = await _redisCacheService.GetOrAddValueAsync(_cacheKeyProducts,
                () => _productService.ShowAllProductsAsync(),
                TimeSpan.FromHours(24));
            return Ok(products);
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var _cacheKeyProductId = $"{_cacheKeyProduct}_{id}";
            var product = await _redisCacheService.GetOrAddValueAsync(_cacheKeyProductId,
                () => _productService.GetProductByIdAsync(id),
                TimeSpan.FromHours(24));
            return Ok(product);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromBody] Product product)
        {
            try
            {
                await _productService.AddProductAsync(product);
                await _redisCacheService.Delete(_cacheKeyProducts);
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
            var _cacheKeyProductId = $"{_cacheKeyProduct}_{id}";
            await _redisCacheService.Delete(_cacheKeyProductId);
            await _redisCacheService.Delete(_cacheKeyProducts);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeProductPartiallyAsync(int id, [FromBody] JsonPatchDocument<ProductDTO> patchDoc)
        {
            if (patchDoc == null) return BadRequest("patchDoc doesn`t exist!");

            var existing_product = await _productService.GetProductByIdAsync(id);
            if (existing_product == null) return BadRequest();

            var productDTO = new ProductDTO
            {
                Name = existing_product.Name,
                Price = existing_product.Price,
                Stock = existing_product.Stock
            };

            patchDoc.ApplyTo(productDTO, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest();
            }

            existing_product.Name = productDTO.Name;
            existing_product.Price = productDTO.Price;
            existing_product.Stock = productDTO.Stock;
            await _productService.UpdateProductAsync(existing_product);
            var _cacheKeyProductId = $"{_cacheKeyProduct}_{id}";
            await _redisCacheService.Delete(_cacheKeyProductId);
            await _redisCacheService.Delete(_cacheKeyProducts);
            return NoContent();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                var _cacheKeyProductId = $"{_cacheKeyProduct}_{id}";
                await _redisCacheService.Delete(_cacheKeyProductId);
                await _redisCacheService.Delete(_cacheKeyProducts);
                return NoContent();
            }
            catch (ProductDoesNotExistException ex) 
            {
                return BadRequest(new { error = $"{ex.Message}", code = HttpStatusCode.BadRequest });
            }
        }
    }
}
