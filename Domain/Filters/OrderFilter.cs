﻿using Domain.Enums;

namespace Domain.Filters;

public class OrderFilter:BaseFilter
{
    public int? Id { get; set; }
    public int? UserID{get;set;}
    public DateTime? OrderDate{get;set;}
    public decimal? TotalAmount{get;set;}
    public Status? Status{get;set;}
    public string? Address{get;set;}
    public PaymentMethod? PaymentMethod {get;set;}
}