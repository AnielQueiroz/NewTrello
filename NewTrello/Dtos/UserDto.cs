using System.ComponentModel.DataAnnotations;

namespace NewTrello.Dtos;

public class UserResponseDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}

public class UserCreateRequestDTO
{    
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}

public class UserUpdateRequestDTO
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}