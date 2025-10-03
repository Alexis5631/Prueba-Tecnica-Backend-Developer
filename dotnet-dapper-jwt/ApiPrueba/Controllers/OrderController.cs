using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Domain.Entities; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Interfaces;
using AutoMapper;
using Application.DTOs;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("orders")]
    [Authorize(Roles = "admin, user")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Crear orden
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var userId = int.Parse(User.FindFirstValue("uid") ?? "0");

            // Validate user exists
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null) return BadRequest("Usuario no válido");

            if (orderDto.OrderItems == null || !orderDto.OrderItems.Any()) 
                return BadRequest("Items vacíos");

            // Calcular total y validar stock
            decimal total = 0;
            foreach (var itemDto in orderDto.OrderItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null) return BadRequest($"Producto {itemDto.ProductId} no existe");
                if (product.Stock < itemDto.Quantity) return BadRequest($"Stock insuficiente para {product.Name}");

                total += product.Price * itemDto.Quantity;
                product.Stock -= itemDto.Quantity;
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _unitOfWork.ProductRepository.Update(product);
            }

            var order = new Order
            {
                UserId = userId,
                Total = total,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                OrderItems = orderDto.OrderItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = _unitOfWork.ProductRepository.GetByIdAsync(i.ProductId).Result!.Price,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
                }).ToList()
            };

            _unitOfWork.OrderRepository.Add(order);
            await _unitOfWork.SaveAsync();

            var orderResponseDto = _mapper.Map<OrderDto>(order);
            return Ok(new { order.Id, order.Total, order = orderResponseDto });
        }

        // Obtener detalles de orden
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            
            if (order == null) return NotFound();

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        // Listar órdenes del usuario autenticado
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirstValue("uid") ?? "0");

            var orders = _unitOfWork.OrderRepository
                .FindByUserId(userId)
                .ToList();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        // Listar todas las órdenes (solo admin)
        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();
            var orderDtos = _mapper.Map<List<OrderDto>>(orders);
            return Ok(orderDtos);
        }

        // Actualizar orden
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderDto)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();

            // Validate user exists if UserId is being updated
            if (orderDto.UserId.HasValue)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(orderDto.UserId.Value);
                if (user == null) return BadRequest("Usuario no válido");
            }

            _mapper.Map(orderDto, order);
            order.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveAsync();

            var updatedOrderDto = _mapper.Map<OrderDto>(order);
            return Ok(updatedOrderDto);
        }

        // Eliminar orden
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();

            _unitOfWork.OrderRepository.Remove(order);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Orden eliminada correctamente" });
        }
    }
}
