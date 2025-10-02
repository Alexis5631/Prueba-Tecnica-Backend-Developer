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
    [Route("roles")]
    [Authorize(Roles = "admin")] // Solo un admin puede gestionar roles
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET /roles → lista todos los roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync();
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            return Ok(roleDtos);
        }

        // GET /roles/{id} → obtener un rol
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null) return NotFound(new { message = "Rol no encontrado" });
            
            var roleDto = _mapper.Map<RoleDto>(role);
            return Ok(roleDto);
        }

        // POST /roles → crear un nuevo rol
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto roleDto)
        {
            var existingRole = _unitOfWork.RoleRepository
                .Find(r => r.Name!.ToLower() == roleDto.Name!.ToLower())
                .FirstOrDefault();

            if (existingRole != null)
                return BadRequest(new { message = "El rol ya existe" });

            var role = _mapper.Map<Role>(roleDto);
            role.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            role.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.RoleRepository.Add(role);
            await _unitOfWork.SaveAsync();

            var createdRoleDto = _mapper.Map<RoleDto>(role);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, createdRoleDto);
        }

        // PUT /roles/{id} → actualizar un rol
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto updateDto)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null) return NotFound();

            _mapper.Map(updateDto, role);
            role.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            
            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveAsync();

            var updatedRoleDto = _mapper.Map<RoleDto>(role);
            return Ok(updatedRoleDto);
        }

        // DELETE /roles/{id} → eliminar un rol
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null) return NotFound();

            _unitOfWork.RoleRepository.Remove(role);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Rol eliminado correctamente" });
        }
    }
}
