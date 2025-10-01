using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Entities;
using Application.Interfaces;

namespace ApiPrueba.Controllers
{
    [ApiController]
    [Route("roles")]
    [Authorize(Roles = "admin")] // Solo un admin puede gestionar roles
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /roles → lista todos los roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _unitOfWork.RoleRepository.GetAllAsync();
            return Ok(roles);
        }

        // GET /roles/{id} → obtener un rol
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null) return NotFound(new { message = "Rol no encontrado" });
            return Ok(role);
        }

        // POST /roles → crear un nuevo rol
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            var existingRole = _unitOfWork.RoleRepository
                .Find(r => r.Name!.ToLower() == role.Name!.ToLower())
                .FirstOrDefault();

            if (existingRole != null)
                return BadRequest(new { message = "El rol ya existe" });

            role.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            role.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);

            _unitOfWork.RoleRepository.Add(role);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        // PUT /roles/{id} → actualizar un rol
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role updatedRole)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null) return NotFound();

            role.Name = updatedRole.Name;
            role.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            
            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveAsync();

            return Ok(role);
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
