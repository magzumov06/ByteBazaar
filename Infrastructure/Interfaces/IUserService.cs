using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter);
}