using NewTrello.Dtos;
using NewTrello.Models;

namespace NewTrello.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    Task<UserResponseDTO?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);   
    Task<UserResponseDTO> CreateUserAsync(UserCreateRequestDTO user);
    Task UpdateUserAsync(Guid id, UserUpdateRequestDTO dto);
    Task<bool> CheckIfEmailAlreadyUsed(string email);
    Task<bool> CheckIfEmailAlreadyUsedByAnotherUser(Guid userId, string email);
    Task DeleteUserAsync(Guid id);
}

