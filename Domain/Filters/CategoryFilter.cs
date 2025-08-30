namespace Domain.Filters;

public class CategoryFilter:BaseFilter
{
    public string? Name { get; set; }
    public int? ParentCategoryId { get; set; }
}