using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewTrello.Dtos;
using NewTrello.Services.Interfaces;

namespace NewTrello.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(users);        
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<UserResponseDTO>> GetUser(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user is null)
            return NotFound("Usuário não encontrado!");

        return Ok(user);
    }

    [HttpPost("create")]
    public async Task <ActionResult<UserResponseDTO>> CreateUser ([FromBody] UserCreateRequestDTO userDTO)
    {
        if (string.IsNullOrWhiteSpace(userDTO.Name) || string.IsNullOrWhiteSpace(userDTO.Email))
            return BadRequest("Preencha os campos obrigatórios");

        var emailAlreadyUsed = await _userService.CheckIfEmailAlreadyUsed(userDTO.Email);
        if (emailAlreadyUsed)
            return BadRequest(new { message = "E-mail já utilizado!" });

        var userDto = await _userService.CreateUserAsync(userDTO);

        return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);
    }

    [HttpPut("{id}")]
    public async Task <ActionResult> UpdateUser(Guid id, UserUpdateRequestDTO dto)
    {
        var userExists = await _userService.GetUserByIdAsync(id);
        if (userExists is null)
            return NotFound(new { message = "Usuário não encontrado!" });

        var emailBelongsToAnotherUser = await _userService.CheckIfEmailAlreadyUsedByAnotherUser(id, dto.Email!);
        if (emailBelongsToAnotherUser)
            return BadRequest(new { message = "E-mail já utilizado por outro usuário!" });

        await _userService.UpdateUserAsync(id, dto);

        return Ok(new { message = "Usuário atualizado com sucesso" });
    }

    [HttpDelete("delete/{id}")]
    public async Task <IActionResult> DeleteUser(Guid id)
    {
        var userExists = await _userService.GetUserByIdAsync(id);
        if(userExists is null)
            return NotFound(new { message = "Usuário não encontrado!" });

        await _userService.DeleteUserAsync(id);

        return NoContent();
    }
}
