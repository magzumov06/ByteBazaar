using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.Account;

public class Register
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 50 characters")]
    public required string FullName { get;set; }
    [StringLength(50, MinimumLength = 3, ErrorMessage = "UserName must be between 3 and 50 characters")]
    public required string UserName { get;set; }
    public string? Address { get; set; }
    
    [Phone]
    [StringLength(13 , MinimumLength = 9 , ErrorMessage = "Phone length must be between 9 and 13")]
    public required string PhoneNumber { get;set; }
    
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get;set; }
    
    [Range(13,100, ErrorMessage = "Age must be between 13 and 100")]
    public required int Age { get; set; }
    public IFormFile? ProfileImage { get; set; }
}