// using System.Net;
// using Domain.DTOs.Account;
// using Domain.Entities;
// using Infrastructure.Responces;
// using Microsoft.AspNetCore.Identity;
//
// namespace Infrastructure.Helpers;
//
// public static class UserManagementHelper
// {
//     public static async Task<Responce<(User User, string Password, string Username)>> CreateUserAsync<T>(
//         T createDto,
//         UserManager<User> userManager,
//         string role,
//         Func<T, string> getUserNameOrPhoneNumber,
//         Func<T, string> getEmail,
//         Func<T, string> getFullName,
//         Func<T, string> getAddress,
//         Func<T, int> getAge,
//         bool usePhoneNumberAsUsername = true
//     )
//     {
//         string username = getUserNameOrPhoneNumber(createDto);
//         if (usePhoneNumberAsUsername)
//         {
//             username = username.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
//             if (username.StartsWith("+"))
//                 username = username.Substring(1);
//         }
//         var existingUser = await userManager.FindByNameAsync(username);
//         int counter = 0;
//         string originalUsername = username;
//         while (existingUser != null)
//         {
//             counter++;
//             username = originalUsername + counter;
//             existingUser = await userManager.FindByNameAsync(username);
//         }
//
//         var user = new User
//         {
//             UserName = username,
//             Email = getEmail(createDto),
//             PhoneNumber = usePhoneNumberAsUsername
//                 ? getUserNameOrPhoneNumber(createDto)
//                 : null,
//             FullName = getFullName(createDto),
//             Address = getAddress(createDto),
//             Age = getAge(createDto),
//             IsDeleted = false,
//             CreatedAt = DateTime.UtcNow,
//             UpdatedAt = DateTime.UtcNow,
//         };
//         var password = PasswordUtil.GenerateRandomPassword();
//         var result = await userManager.CreateAsync(user, password);
//         if (!result.Succeeded)
//             return new Responce<(User, string, string)>(HttpStatusCode.BadRequest, IdentityHelper.FormatIdentityErrors(result));
//         await userManager.AddToRoleAsync(user, role);
//
//         return new Responce<(User, string, string)>((user, password, username));
//     }
//     public static async Task<Responce<string>> UpdateUserAsync<T>(
//         User user,
//         T updateDto,
//         UserManager<User> userManager,
//         Func<T, string> getEmail,
//         Func<T, string> getFullName,
//         Func<T, string> getPhoneNumber,
//         Func<T, string> getAddress)
//     {
//         user.Email = getEmail(updateDto);
//         user.FullName = getFullName(updateDto);
//         user.PhoneNumber = getPhoneNumber(updateDto);
//         user.Address = getAddress(updateDto);
//         
//         var result = await userManager.UpdateAsync(user);
//         return result.Succeeded
//             ? new Responce<string>(HttpStatusCode.OK, "User updated successfully")
//             : new Responce<string>(HttpStatusCode.BadRequest, IdentityHelper.FormatIdentityErrors(result));
//
//     }
//     
// }
