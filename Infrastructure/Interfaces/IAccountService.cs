using Domain.DTOs.Account;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces;

public interface IAccountService
{
    Task<Responce<string>> Register(Register register);
    Task<Responce<string>> Login(LoginDto login);
    Task<Responce<string>> ResetPassword(ResetPasswordDto resetPassword);
    Task<Responce<string>> ForgotPasswordCodeGenerator(ForgotPassword forgotPassword);
    Task<Responce<string>> ChangePassword(ChangePassword changePassword, Guid userId);
}