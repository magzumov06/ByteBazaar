using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces;

public interface IuserService
{
    Task<Responce<string>> CreateUser(CreateUserDto dto);
    Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter);
}