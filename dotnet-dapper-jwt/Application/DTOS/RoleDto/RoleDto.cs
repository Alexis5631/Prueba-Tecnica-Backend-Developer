using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateRoleDto
    {
        [Required]
        public string? Name { get; set; }
    }

    public class UpdateRoleDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
