using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int? RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? RoleName { get; set; }
    }

    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public int? RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
