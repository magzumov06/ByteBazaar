using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    [Phone] 
    public string PhoneNumber { get; set; }
    public Role Role { get; set; }
    public string AvatarUrl { get; set; }
    public bool IsDeleted { get; set; }
}