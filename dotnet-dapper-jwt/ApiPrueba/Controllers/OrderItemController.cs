using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Application.Interfaces;
using AutoMapper;
using Application.DTOs;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("order-items")]
    [Authorize(Roles = "admin")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET /order-items → lista todos los items (solo admin)
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var items = await _unitOfWork.OrderItemRepository.GetAllAsync();
            var itemDtos = _mapper.Map<List<OrderItemDto>>(items);
            return Ok(itemDtos);
        }

        // GET /order-items/{id} → obtener un item específico
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem(int id)
        {
            var item = await _unitOfWork.OrderItemRepository.GetByIdAsync(id);

            if (item == null) return NotFound(new { message = "Item no encontrado" });

            var itemDto = _mapper.Map<OrderItemDto>(item);
            return Ok(itemDto);
        }

        // POST /order-items → agregar un ítem a una orden existente
        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] OrderItemDto itemDto)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(itemDto.OrderId);
            if (order == null) return BadRequest(new { message = "La orden no existe" });

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(itemDto.ProductId);
            if (product == null) return BadRequest(new { message = "El producto no existe" });

            if (product.Stock < itemDto.Quantity)
                return BadRequest(new { message = "Stock insuficiente" });

            product.Stock -= itemDto.Quantity;
            product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            _unitOfWork.ProductRepository.Update(product);

            var item = _mapper.Map<OrderItem>(itemDto);
            item.UnitPrice = product.Price;
            item.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            item.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.OrderItemRepository.Add(item);
            await _unitOfWork.SaveAsync();

            var createdItemDto = _mapper.Map<OrderItemDto>(item);
            return Ok(createdItemDto);
        }

        // PUT /order-items/{id} → actualizar cantidad
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] UpdateOrderItemDto updateDto)
        {
            var item = await _unitOfWork.OrderItemRepository.GetByIdAsync(id);
            if (item == null) return NotFound();

            // Ajustar stock: devolver la cantidad previa
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity; // devolver stock previo
                if (product.Stock < updateDto.Quantity)
                    return BadRequest(new { message = "Stock insuficiente" });

                product.Stock -= updateDto.Quantity;
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _unitOfWork.ProductRepository.Update(product);
            }

            _mapper.Map(updateDto, item);
            item.UnitPrice = product?.Price ?? item.UnitPrice;
            item.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.OrderItemRepository.Update(item);
            await _unitOfWork.SaveAsync();
            
            var updatedItemDto = _mapper.Map<OrderItemDto>(item);
            return Ok(updatedItemDto);
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
