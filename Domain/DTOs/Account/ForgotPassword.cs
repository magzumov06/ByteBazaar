using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account;

public class ForgotPassword
{
        [Required] 
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
         public string Email { get; } = string.Empty;
}