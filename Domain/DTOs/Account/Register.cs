using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.Account;

public class Register
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 characters")]
    public string FullName { get;set; }
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "UserName must be between 3 and 50 characters")]
    public string UserName { get;set; }
    public string PhoneNumber { get;set; }
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get;set; }
    public IFormFile? ProfileImage { get; set; }
    public int Age { get; set; }
}