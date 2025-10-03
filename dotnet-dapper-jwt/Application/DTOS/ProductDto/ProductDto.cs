using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Sku { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateProductDto
    {
        [Required]
        [DefaultValue("")]
        public string? Name { get; set; } = "";
        
        [Required]
        [DefaultValue("")]
        public string? Sku { get; set; } = "";
        
        [Required]
        [Range(0, 999999.99, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        [DefaultValue(0)]
        public decimal Price { get; set; } = 0;
        
        [Required]
        [Range(0, 999999, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        [DefaultValue(0)]
        public int Stock { get; set; } = 0;
        
        [DefaultValue("")]
        public string? Category { get; set; } = "";
    }

    public class UpdateProductDto
    {
        [Required]
        [DefaultValue("")]
        public string? Name { get; set; } = "";
        
        [Required]
        [DefaultValue("")]
        public string? Sku { get; set; } = "";
        
        [Required]
        [Range(0, 999999.99, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        [DefaultValue(0)]
        public decimal Price { get; set; } = 0;
        
        [Required]
        [Range(0, 999999, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        [DefaultValue(0)]
        public int Stock { get; set; } = 0;
        
        [DefaultValue("")]
        public string? Category { get; set; } = "";
    }
}
