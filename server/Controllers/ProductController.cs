using Microsoft.AspNetCore.Mvc;
using server.Dtos.Product;
using server.Helpers;
using server.Interfaces;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("server/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var products = await _productRepo.GetAllAsync(query);
            if(products == null) return NotFound();


            var productDto = products.Select(s => s.ToProductDto()).ToList();
            return Ok(productDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productRepo.GetByIdAsync(id);
            if(product == null) return NotFound();
            
            return Ok(product.ToProductDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto productDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productModel = productDto.ToProductFromCreateDTO();
            if (productModel == null) return NotFound();

            await _productRepo.CreateAsync(productModel);

            return CreatedAtAction(nameof(GetById), new { id = productModel.Id }, productModel.ToProductDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromForm] UpdateProductDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var productModel = await _productRepo.UpdateAsync(id, updateDto);
            if (productModel == null) return NotFound();

            return Ok(productModel.ToProductDto());
        }

        [HttpPut]
        [Route("{id}/stock")]
        public async Task<IActionResult> UpdateStock([FromRoute]int id, [FromBody] UpdateProductStockDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var productModel = await _productRepo.UpdateStockAsync(id, updateDto);
            if (productModel == null) return NotFound();

            return Ok(productModel.ToProductDto());

        }
    }
}