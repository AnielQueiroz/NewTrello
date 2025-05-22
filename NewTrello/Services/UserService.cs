using AutoMapper;
using NewTrello.Dtos;
using NewTrello.Mappings;
using NewTrello.Models;
using NewTrello.Repositories;
using NewTrello.Services.Interfaces;

namespace NewTrello.Services;

public class UserService(UserRepository repo) : IUserService
{
    private readonly UserRepository _repo = repo;

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await _repo.GetAllAsync();
       
        var usersDtos = UserMapper.ToDto(users);
        return usersDtos;
    }

    public async Task<UserResponseDTO?> GetUserByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is null)
            return null;

        var userDto = UserMapper.ToDto(user);
        return userDto;
    }

    public async Task<UserResponseDTO> CreateUserAsync(UserCreateRequestDTO userDTO)
    {
        var user = UserMapper.ToModel(userDTO);

        user.Id = Guid.NewGuid();        

        await _repo.AddAsync(user);
        await _repo.SaveAsync();

        var userDto = UserMapper.ToDto(user);
        return userDto;
    }

    public async Task UpdateUserAsync(Guid id, UserUpdateRequestDTO dto)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is not null)
        {
            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.Password = hashedPassword;
            }                

            await _repo.SaveAsync();
        }
    }

    public async Task<bool> CheckIfEmailAlreadyUsed(string email)
    {
        return await _repo.EmailExistsAsync(email);
    }

    public async Task<bool> CheckIfEmailAlreadyUsedByAnotherUser(Guid userId, string email)
    {
        return await _repo
            .GetAllAsync()
            .ContinueWith(task => task.Result
                .Any(u => u.Email == email && u.Id != userId));
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is not null)
        {
            _repo.Delete(user);
            await _repo.SaveAsync();
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user is null)
            return null;

        return user;
    }
}

