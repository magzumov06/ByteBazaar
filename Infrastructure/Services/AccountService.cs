using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.Account;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Responces;
using Infrastructure.Services.EmailServices;
using Infrastructure.Services.HashServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AccountService(
    UserManager<User> userManager,
    RoleManager<IdentityRole<int>> roleManager,
    IConfiguration  configuration,
    DataContext  context,
    IEmailService emailService,
    IHashService hashService): IAccountService
{
    public async Task<Responce<string>> Register(Register register)
    {
        try
        {
            var existingUser = await userManager.FindByNameAsync(register.UserName);
            if (existingUser != null)
                return new Responce<string>(HttpStatusCode.BadRequest,"User already exists");
            var userResult = await UserManagementHelper.CreateUserAsync(
                register,
                userManager,
                Roles.User.ToString(),
                dto => dto.UserName,
                dto => dto.Email,
                dto => dto.FullName,
                dto => dto.PhoneNumber,
                dto=>dto.Age
            );
            if (userResult.StatusCode != (int)HttpStatusCode.OK)
                return new Responce<string>((HttpStatusCode)userResult.StatusCode, userResult.Message);
            
            var (_, password, username) = userResult.Data;
            
            if (!string.IsNullOrEmpty(register.Email))
            {
                await EmailHelper.SendLoginDetailsEmailAsync(
                    emailService,
                    register.Email,
                    username,
                    password, 
                    "User"
                    );
            }
            return new Responce<string>(HttpStatusCode.Created, "Корбар бо муваффақият сохта шуд. Маълумоти воридшавӣ ба почтаи электронӣ фиристода шуд.");
            
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> Login(LoginDto login)
    {
        var user = await userManager.FindByNameAsync(login.UserName);
        if (user == null)
            return new Responce<string>(HttpStatusCode.NotFound, "Номи корбар ё рамз нодуруст аст");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, login.Password);
        if (!isPasswordValid)
            return new Responce<string>(HttpStatusCode.BadRequest, "Номи корбар ё рамз нодуруст аст");

        var token = await GenerateJwtToken(user);
        return new Responce<string>(token) { Message = "Воридшавӣ бо муваффақият анҷом ёфт" };
    }

    public async Task<Responce<string>> ResetPassword(ResetPasswordDto resetPassword)
    {
        try
        {
            if (resetPassword == null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Маълумоти дархост нодуруст аст");

            var existingUser = await userManager.Users.FirstOrDefaultAsync(x => x.Email == resetPassword.Email);
            if (existingUser == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Корбар ёфт нашуд");

            if (resetPassword.Code != existingUser.Code)
                return new Responce<string>(HttpStatusCode.BadRequest, "Рамзи тасдиқ нодуруст аст");

            var timeElapsed = DateTimeOffset.UtcNow - existingUser.CodeDate;
            if (timeElapsed.TotalMinutes > 3)
                return new Responce<string>(HttpStatusCode.BadRequest, "Мӯҳлати рамзи тасдиқ гузаштааст");

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(existingUser);
            var resetResult = await userManager.ResetPasswordAsync(existingUser, resetToken, resetPassword.Password);
            if (!resetResult.Succeeded)
                return new Responce<string>(HttpStatusCode.BadRequest, IdentityHelper.FormatIdentityErrors(resetResult));

            existingUser.Code = null;
            existingUser.CodeDate = default;
            await context.SaveChangesAsync();

            return new Responce<string>(HttpStatusCode.OK, "Рамз бо муваффақият иваз карда шуд");
        }
        catch (Exception ex)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, $"Хатогӣ ҳангоми ивазкунии рамз: {ex.Message}");
        }
    }

    public async Task<Responce<string>> ForgotPasswordCodeGenerator(ForgotPassword forgotPassword)
    {
        try
        {
            if (forgotPassword == null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Маълумоти дархост нодуруст аст");

            var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == forgotPassword.Email);
            if (existingUser == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Корбар ёфт нашуд");

            var code = new Random().Next(1000, 9999).ToString();
            existingUser.Code = code;
            existingUser.CodeDate = DateTime.UtcNow;

            var res = await context.SaveChangesAsync();
            if (res <= 0)
                return new Responce<string>(HttpStatusCode.BadRequest, "Хатогӣ ҳангоми сохтани рамзи тасдиқ");

            await EmailHelper.SendResetPasswordCodeEmailAsync(emailService, forgotPassword.Email, code);

            return new Responce<string>(HttpStatusCode.OK, "Рамзи тасдиқ бо муваффақият фиристода шуд");
        }
        catch (Exception ex)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, $"Хатогӣ ҳангоми фиристодани рамзи тасдиқ: {ex.Message}");
        }
    }


    public async Task<Responce<string>> ChangePassword(ChangePassword changePassword, Guid userId)
    {
        try
        {
            if (changePassword == null)
                return new Responce<string>(HttpStatusCode.BadRequest, "Маълумоти рамз нодуруст аст");

            var existingUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (existingUser == null)
                return new Responce<string>(HttpStatusCode.NotFound, "Корбар ёфт нашуд");

            var changePassResult = await userManager.ChangePasswordAsync(existingUser, changePassword.OldPassword, changePassword.Password);
            if (!changePassResult.Succeeded)
                return new Responce<string>(HttpStatusCode.BadRequest, IdentityHelper.FormatIdentityErrors(changePassResult));

            return new Responce<string>(HttpStatusCode.OK, "Рамз бо муваффақият иваз карда шуд");
        }
        catch (Exception ex)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, $"Хатогӣ ҳангоми ивазкунии рамз: {ex.Message}");
        }
    }
    
     private async Task<string> GenerateJwtToken(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Корбар наметавонад null бошад");
        }

        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
        };

        if (!string.IsNullOrEmpty(user.UserName))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName));
        }

        if (!string.IsNullOrEmpty(user.FullName))
        {
            claims.Add(new Claim("Fullname", user.FullName));
        }

        if (!string.IsNullOrEmpty(user.Email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        }
        

        var roles = await userManager.GetRolesAsync(user);
        if (roles != null)
        {
            claims.AddRange(roles.Where(role => !string.IsNullOrEmpty(role)).Select(role => new Claim("role", role)));
        }
        

        var expirationDays = int.TryParse(configuration["Jwt:ExpirationDays"], out var days) ? days : 3;
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expirationDays),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
}


