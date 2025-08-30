using Domain.Entities;

namespace Domain.DTOs.UserDto;

public class GetUserDto:BaseEntities
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}