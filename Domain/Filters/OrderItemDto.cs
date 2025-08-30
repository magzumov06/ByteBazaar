namespace Domain.Filters;

public class OrderItemDto:BaseFilter
{
    public int? OrderId{get;set;}
    public int? ProductId{get;set;}
    public int? Quantity{get;set;}
    public decimal? Price{get;set;}
}