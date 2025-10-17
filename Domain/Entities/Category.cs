using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Category:BaseEntities
{
    public int Id { get; set; }
    [Required]
    [MinLength(3 , ErrorMessage = "Name minimum length is 3")]
    public required string Name { get; set; }
    public List<Product>? Products { get; set; }
}