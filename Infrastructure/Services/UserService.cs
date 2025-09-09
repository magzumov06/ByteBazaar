using System.Net;
using Domain.DTOs.UserDto;
using Domain.Entities;
using Domain.Filters;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Responces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class UserService(DataContext context) : IUserService
{
    public async Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter)
    {
        try
        {
            var query = context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(filter.FullName))
            {
                query = query.Where(x => x.FullName.Contains(filter.FullName));
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => x.Email.Contains(filter.Email));
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                query = query.Where(x => x.Address.Contains(filter.Address));
            }

            if (filter.Age.HasValue)
            {
                query = query.Where(x => x.Age == filter.Age);
            }

            query = query.Where(x => x.IsDeleted == false);
            var total =await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var users = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(users.Count == 0) return new PaginationResponce<List<GetUserDto>>(HttpStatusCode.OK,"Users not found");
            var dtos = users.Select(x=> new GetUserDto()
            {
                Id = x.Id,
                Age = x.Age,
                Address = x.Address,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new PaginationResponce<List<GetUserDto>>(dtos, total,filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PaginationResponce<List<GetUserDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}