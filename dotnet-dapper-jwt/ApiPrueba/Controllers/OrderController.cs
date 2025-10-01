using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Interfaces;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("orders")]
    [Authorize(Roles = "admin, user")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Crear orden
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<OrderItem> items)
        {
            var userId = int.Parse(User.FindFirstValue("uid") ?? "0");

            if (items == null || !items.Any()) return BadRequest("Items vacíos");

            // Calcular total y validar stock
            decimal total = 0;
            foreach (var item in items)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null) return BadRequest($"Producto {item.ProductId} no existe");
                if (product.Stock < item.Quantity) return BadRequest($"Stock insuficiente para {product.Name}");

                total += product.Price * item.Quantity;
                product.Stock -= item.Quantity;
                product.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                _unitOfWork.ProductRepository.Update(product);
            }

            var order = new Order
            {
                UserId = userId,
                Total = total,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                OrderItems = items.Select(i => new OrderItem
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

            return Ok(new { order.Id, order.Total });
        }

        // Obtener detalles de orden
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            
            if (order == null) return NotFound();

            return Ok(order);
        }

        // Listar órdenes del usuario autenticado
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = int.Parse(User.FindFirstValue("uid") ?? "0");

            var orders = _unitOfWork.OrderRepository
                .Find(o => o.UserId == userId)
                .ToList();

            return Ok(orders);
        }
    }
}
