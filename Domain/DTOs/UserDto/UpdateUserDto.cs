using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{
    public int Id { get; set; }

    public  string FullName { get; set; }
    public int Age { get; set; }
    public string? Address { get; set; }
    public string Email { get; set; }
    public  string PhoneNumber { get; set; }
    public IFormFile? AvatarUrl { get; set; }
}