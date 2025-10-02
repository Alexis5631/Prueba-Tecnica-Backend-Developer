using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Application.DTOs;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize(Roles = "admin, user")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        // Crear usuario nuevo (rol por defecto: user)
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto request)
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

            var newUser = _mapper.Map<User>(request);
            newUser.RoleId = role.Id;
            newUser.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            newUser.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.UserRepository.Add(newUser);
            await _unitOfWork.SaveAsync();

            var userDto = _mapper.Map<UserDto>(newUser);
            return Ok(new { message = "Usuario creado correctamente", user = userDto });
        }

        // Obtener info de usuario
        [HttpGet("{id}")]
        [Authorize] // solo autenticados
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            
            if (user == null) return NotFound();

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // Obtener todos los usuarios
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        // Actualizar usuario
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateDto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            _mapper.Map(updateDto, user);
            user.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveAsync();

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // Eliminar usuario
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return NotFound();

            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Usuario eliminado correctamente" });
        }
    }
}
