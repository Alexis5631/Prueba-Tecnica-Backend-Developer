using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize(Roles = "admin, user")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        // Crear usuario nuevo (rol por defecto: user)
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User request)
        {
            var existingUser = _unitOfWork.UserRepository
                .Find(u => u.Username!.ToLower() == request.Username!.ToLower())
                .FirstOrDefault();

            if (existingUser != null)
                return BadRequest(new { message = "El usuario ya existe" });

            var role = _unitOfWork.RoleRepository
                .Find(r => r.Name!.ToLower() == "user")
                .FirstOrDefault();
            
            if (role == null) 
                return BadRequest("No existe rol user");

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = request.PasswordHash,
                RoleId = role.Id,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _unitOfWork.UserRepository.Add(newUser);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Usuario creado correctamente", userId = newUser.Id });
        }

        // Obtener info de usuario
        [HttpGet("{id}")]
        [Authorize] // solo autenticados
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Username,
                Role = user.Role?.Name,
                user.CreatedAt
            });
        }
    }
}
