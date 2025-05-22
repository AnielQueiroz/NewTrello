using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewTrello.Dtos;
using NewTrello.Services;
using NewTrello.Services.Interfaces;

namespace NewTrello.Controllers;

[ApiController]
[Route("/api/[controller]")]
[AllowAnonymous]
public class AuthController(IUserService userService, AuthService authService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly AuthService _authService = authService;

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDTO>> Register([FromBody] UserCreateRequestDTO registerDto)
    {
        var existingUser = await _userService.GetUserByEmailAsync(registerDto.Email);
        if (existingUser is not null)
            return BadRequest(new { message = "E-mail já cadastrado." });

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new UserCreateRequestDTO
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Password = hashedPassword,
        };

        var userDto = await _userService.CreateUserAsync(user);

        return Ok(userDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userService.GetUserByEmailAsync(loginDto.Email);
        if (user is null)
            return NotFound(new { message = "Usuário não encontrado!" });

        if (string.IsNullOrEmpty(loginDto.Password) || string.IsNullOrEmpty(loginDto.Email))
            return BadRequest(new { message = "Preencha os campos obrigatórios. " });

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            return Unauthorized(new { message = "Senha incorreta!" });

        var token = _authService.GenerateJwtToken(user);
        var response = new AuthResponseDto
        {
            Token = token,
            Name = user.Name!,
            Email = user.Email!
        };

        return Ok(response);
    }
}
