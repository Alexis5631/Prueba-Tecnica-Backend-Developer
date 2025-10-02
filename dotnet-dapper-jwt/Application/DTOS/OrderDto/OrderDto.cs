using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Username { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class CreateOrderDto
    {
        [Required]
        public int UserId { get; set; }
        
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class UpdateOrderDto
    {
        public int? UserId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }
}
