namespace Domain.Filters;

public class ProductFilter : BaseFilter
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public int? CategoryId { get; set; }
    public decimal? AverageRating { get; set; }
    public int? RatingCount { get; set; }
}