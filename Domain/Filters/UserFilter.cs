using Domain.Enums;

namespace Domain.Filters;

public class UserFilter:BaseFilter
{
    public string? FullName { get; set; } 
    public int? Age { get; set; }
    public string? Email { get; set; } 
    public string? PhoneNumber { get; set; }
    public Role? Role { get; set; }
    public string? AvatarUrl { get; set; }
}