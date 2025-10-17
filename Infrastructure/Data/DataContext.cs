using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Review> Reviews{get;set;}
    public DbSet<Product> Products{get;set;}
    public DbSet<OrderItem>OrderItems{get;set;}
    public DbSet<Order> Orders{get;set;}
    public DbSet<Category> Categories{get;set;}
    public DbSet<CartItem> CartItems { get; set; }
}