using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Review : BaseEntities
{
    public int Id{get;set;}
    public int UserId{get;set;}
    public int ProductId{get;set;}
    [Required]
    [Range(1,5, ErrorMessage = "Rating must be between 1 and 5")]
    public decimal Rating {get;set;}
    public string? Comment  { get; set; }
    public User? User { get; set; }
    public Product? Product { get; set; }
}