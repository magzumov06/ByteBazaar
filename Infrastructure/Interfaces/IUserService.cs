using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Responce<string>> UpdateUser (UpdateUserDto user);
    Task<Responce<string>> DeleteUser (int id);
    Task<Responce<GetUserDto>> GetUser(int id);
    Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter);
}