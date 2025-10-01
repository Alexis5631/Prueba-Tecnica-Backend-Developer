using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("products")]
    [Authorize(Roles = "admin, user")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            return Ok(products);
        }

        // Crear producto (solo admin)
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var existingProduct = _unitOfWork.ProductRepository
                .Find(p => p.Sku == product.Sku)
                .FirstOrDefault();

            if (existingProduct != null)
                return BadRequest(new { message = "SKU ya registrado" });

            product.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveAsync();
            return Ok(product);
        }

        // Actualizar producto
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            var existing = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.Category = product.Category;
            existing.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.ProductRepository.Update(existing);
            await _unitOfWork.SaveAsync();
            return Ok(existing);
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
