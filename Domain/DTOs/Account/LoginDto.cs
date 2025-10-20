using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account;

public class LoginDto
{
    [Required]
    public required string UserName { get; set; }
    public required string Password { get; set; }
}