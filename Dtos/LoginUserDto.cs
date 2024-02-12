using System.ComponentModel.DataAnnotations;

namespace WebApi_Demo.Dtos;

public class LoginUserDto
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}
