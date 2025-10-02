using System;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class DataUserDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool EstaAutenticado { get; set; }
        public string? Mensaje { get; set; }
        public string? Token { get; set; }
        public string? Username { get; set; }
        public List<string> Roles { get; set; } = new();
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}



