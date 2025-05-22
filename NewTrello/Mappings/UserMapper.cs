using NewTrello.Dtos;
using NewTrello.Models;

namespace NewTrello.Mappings;

public static class UserMapper
{
    public static UserResponseDTO ToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
    };

    public static IEnumerable<UserResponseDTO> ToDto(IEnumerable<User> users)
    {
        return users.Select(u => ToDto(u));
    }

    public static User ToModel(UserCreateRequestDTO userDto) => new()
    {
        Id = Guid.NewGuid(),
        Name = userDto.Name,
        Email = userDto.Email,
        Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
    };
}
