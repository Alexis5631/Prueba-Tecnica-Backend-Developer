using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Application.Interfaces;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("order-items")]
    [Authorize(Roles = "admin")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /order-items → lista todos los items (solo admin)
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var items = await _unitOfWork.OrderItemRepository.GetAllAsync();
            return Ok(items);
        }

        // GET /order-items/{id} → obtener un item específico
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem(int id)
        {
            var item = await _unitOfWork.OrderItemRepository.GetByIdAsync(id);

            if (item == null) return NotFound(new { message = "Item no encontrado" });

            return Ok(item);
        }

        // POST /order-items → agregar un ítem a una orden existente
        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] OrderItem item)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(item.OrderId);
            if (order == null) return BadRequest(new { message = "La orden no existe" });

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
            if (product == null) return BadRequest(new { message = "El producto no existe" });

            if (product.Stock < item.Quantity)
                return BadRequest(new { message = "Stock insuficiente" });

            product.Stock -= item.Quantity;
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            _unitOfWork.ProductRepository.Update(product);

            item.UnitPrice = product.Price;
            item.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            item.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.OrderItemRepository.Add(item);
            await _unitOfWork.SaveAsync();

            return Ok(item);
        }

        // PUT /order-items/{id} → actualizar cantidad
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItem updated)
        {
            var item = await _unitOfWork.OrderItemRepository.GetByIdAsync(id);
            if (item == null) return NotFound();

            // Ajustar stock: devolver la cantidad previa
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity; // devolver stock previo
                if (product.Stock < updated.Quantity)
                    return BadRequest(new { message = "Stock insuficiente" });

                product.Stock -= updated.Quantity;
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _unitOfWork.ProductRepository.Update(product);
            }

            item.Quantity = updated.Quantity;
            item.UnitPrice = product?.Price ?? item.UnitPrice;
            item.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.OrderItemRepository.Update(item);
            await _unitOfWork.SaveAsync();
            return Ok(item);
        }

        // DELETE /order-items/{id} → eliminar un ítem de una orden
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var item = await _unitOfWork.OrderItemRepository.GetByIdAsync(id);
            if (item == null) return NotFound();

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity; // devolver stock al eliminar item
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _unitOfWork.ProductRepository.Update(product);
            }

            _unitOfWork.OrderItemRepository.Remove(item);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Item eliminado correctamente" });
        }
    }
}
