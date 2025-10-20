using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<int>
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public required string FullName { get; set; }
    [Range(13,100, ErrorMessage = "Age must be between 13 and 100")]
    public int Age { get; set; }
    [StringLength(150)]
    public string? Address { get; set; }
    [EmailAddress]
    public override required string Email { get; set; }
    [Phone]
    [StringLength(13 , MinimumLength = 9 , ErrorMessage = "Phone length must be between 9 and 13")]
    public override required string PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public List<Review>? Reviews {get;set;}
    public List<CartItem>? CartItems { get; set; }
    public List<Order>? Orders { get; set; }
}