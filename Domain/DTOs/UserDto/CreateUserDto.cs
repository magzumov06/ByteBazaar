using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTOs.UserDto;

public class CreateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string FullName { get; set; }
    [Range(13,100, ErrorMessage = "Age must be between 13 and 100")]
    public int Age { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    [MinLength(6, ErrorMessage = "Password minimum length is 6")]
    public required string PasswordHash { get; set; }
    [Phone]
    [StringLength(13 , MinimumLength = 9 , ErrorMessage = "Phone length must be between 9 and 13")]
    public required string PhoneNumber { get; set; }
    public Role Role { get; set; }
    public string? AvatarUrl { get; set; }
}