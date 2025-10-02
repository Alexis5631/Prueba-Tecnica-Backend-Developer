using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using AutoMapper;
using Application.DTOs;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize(Roles = "admin, user")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET /products?category=ropa&name=camisa
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? category, [FromQuery] string? name)
        {
            var allProducts = await _unitOfWork.ProductRepository.GetAllAsync();
            var query = allProducts.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name!.ToLower().Contains(name.ToLower()));

            var products = query.ToList();
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }

        // Crear producto (solo admin)
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var existingProduct = _unitOfWork.ProductRepository
                .Find(p => p.Sku == productDto.Sku)
                .FirstOrDefault();

            if (existingProduct != null)
                return BadRequest(new { message = "SKU ya registrado" });

            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveAsync();

            var createdProductDto = _mapper.Map<ProductDto>(product);
            return Ok(createdProductDto);
        }

        // Actualizar producto
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
        {
            var existing = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(productDto, existing);
            existing.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.ProductRepository.Update(existing);
            await _unitOfWork.SaveAsync();

            var updatedProductDto = _mapper.Map<ProductDto>(existing);
            return Ok(updatedProductDto);
        }

        // Obtener producto por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        // Eliminar producto
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            _unitOfWork.ProductRepository.Remove(product);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Producto eliminado" });
        }
    }
}
