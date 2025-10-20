using System.Net;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.FileStorage;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class UserService(DataContext context,
    IFileStorage file) : IUserService
{
    public async Task<Responce<string>> UpdateUser(UpdateUserDto user)
    {
        try
        {
            Log.Information("Updating user");
            var update = await context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if(update == null) return new Responce<string>(HttpStatusCode.NotFound, "User not found");
            if (user.AvatarUrl != null)
            {
                if (!string.IsNullOrEmpty(update.AvatarUrl))
                {
                    await file.DeleteFile(update.AvatarUrl);
                }
                await file.SaveFile(user.AvatarUrl, "UserAvatar");
            }
            update.FullName = user.FullName;
            update.Email = user.Email;
            update.Age = user.Age;
            update.Address = user.Address;
            update.PhoneNumber = user.PhoneNumber;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("User updated");
            }
            else
            {
                Log.Fatal("Failed to update user");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "User successfully updated")
                : new Responce<string>(HttpStatusCode.NotFound, "User not update");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateUser");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteUser(int id)
    {
        try
        {
            Log.Information("Deleting user");
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user == null) return new Responce<string>(HttpStatusCode.NotFound, "User not found");
            user.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("User deleted");
            }
            else
            {
                Log.Fatal("Failed to delete user");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "User successfully deleted")
                : new Responce<string>(HttpStatusCode.NotFound, "User not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteUser");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetUserDto>> GetUser(int id)
    {
        try
        {
            Log.Information("Getting user");
            var get =  await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(get == null) return new Responce<GetUserDto>(HttpStatusCode.NotFound, "User not found");
            var dto = new GetUserDto()
            {
                Id = get.Id,
                FullName = get.FullName,
                Email = get.Email,
                Age = get.Age,
                Address = get.Address,
                PhoneNumber = get.PhoneNumber,
                AvatarUrl = get.AvatarUrl,
                CreatedAt = get.CreatedAt,
                UpdatedAt = get.UpdatedAt
            };
            return new Responce<GetUserDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetUser");
            return new Responce<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter)
    {
        try
        {
            Log.Information("Getting users");
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
                FullName = x.FullName,
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
            Log.Error("Error in GetUsers");
            return new PaginationResponce<List<GetUserDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}