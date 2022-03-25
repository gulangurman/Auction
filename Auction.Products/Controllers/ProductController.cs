using Auction.Products.Models;
using Auction.Products.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Products.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {            
            _productRepository = productRepository;
            _logger = logger;
            _logger.LogInformation("ProductController instance created.");
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            _logger.LogInformation("GET /api/v1/Product");
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name ="GetProduct")]
        public async Task<ActionResult> GetProduct(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product id [{id}] not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] Product product)
        {
            await _productRepository.Create(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            var result = await _productRepository.Update(product);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProductById(string id)
        {
            return Ok(await _productRepository.Delete(id));
        }



    }
}

