using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using LittleFish.Core;
using LittleFish.Core.Models;
using LittleFish.Core.Services;

namespace LittleFish.Api.Controllers; 

[Authorize]
[Controller]
[Route("api/[controller]")]
public class ProductController: Controller {
    
    private readonly ProductsMongoDBService _productsMongoDBService;

    public ProductController(ProductsMongoDBService productsMongoDBService) {
        _productsMongoDBService = productsMongoDBService;
    }

    [HttpGet]
    public async Task<List<Product>> GetProductsAsync() {
        return await _productsMongoDBService.GetAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<List<Product>> GetProductAsync() {
        return await _productsMongoDBService.GetAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] Product product) {
        await _productsMongoDBService.CreateAsync(product);
        return CreatedAtAction(nameof(Product), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductAsync(string id, [FromBody] Product product) {
        await _productsMongoDBService.UpdateAsync(id, product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(string id) {
        await _productsMongoDBService.DeleteAsync(id);
        return NoContent();
    }
}
