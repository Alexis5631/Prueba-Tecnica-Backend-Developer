using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPrueba.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrueba.Controllers;

public class AuthController : BaseApiController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterDto model)
    {
        var result = await _userService.RegisterAsync(model);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var result = await _userService.GetTokenAsync(model);

        if (!result.EstaAutenticado)
            return Unauthorized(result.Mensaje);

        return Ok(result);
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result = await _userService.AddRoleAsync(model);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto model)
    {
        var result = await _userService.RefreshTokenAsync(model.RefreshToken);
        
        if (!result.EstaAutenticado)
            return Unauthorized(result.Mensaje);

        return Ok(result);
    }
}