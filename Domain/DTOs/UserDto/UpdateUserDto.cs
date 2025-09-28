using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{
    public int Id { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string FullName { get; set; }
    [Range(13,100, ErrorMessage = "Age must be between 13 and 100")]
    public int Age { get; set; }
    [StringLength(150)]
    public string? Address { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    [Phone]
    [StringLength(13 , MinimumLength = 9 , ErrorMessage = "Phone length must be between 9 and 13")]
    public required string PhoneNumber { get; set; }
    public IFormFile? AvatarUrl { get; set; }
}