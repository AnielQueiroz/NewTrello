using System.ComponentModel.DataAnnotations;

namespace NewTrello.Dtos;

//public class RegisterDto
//{
//    public string Name { get; set; }
//    public string Email { get; set; }
//    public string Password { get; set; }
//}

public class LoginDto
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}

public class AuthResponseDto
{
    [Required]
    public required string Token { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Email { get; set; }
}
