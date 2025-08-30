using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) :  DbContext(options)
{
    public DbSet<User> Users{get;set;}
    public DbSet<Review> Reviews{get;set;}
    public DbSet<Product> Products{get;set;}
    public DbSet<OrderItem>OrderItems{get;set;}
    public DbSet<Order> Orders{get;set;}
    public DbSet<Category> Categories{get;set;}
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.UserId);
        
        
        modelBuilder.Entity<CartItem>()
            .HasOne(c=>c.User)
            .WithMany(u=> u.CartItems)
            .HasForeignKey(c=>c.UserId);
        
        modelBuilder.Entity<Review>()
            .HasOne(r=>r.Product)
            .WithMany(p=> p.Reviews)
            .HasForeignKey(r=>r.ProductId);
        
        modelBuilder.Entity<OrderItem>()
            .HasOne(o=> o.Product)
            .WithMany(p=> p.OrderItems)
            .HasForeignKey(o=>o.ProductId);
        
        modelBuilder.Entity<CartItem>()
            .HasOne(c=>c.Product)
            .WithMany(p=>p.CartItems)
            .HasForeignKey(c=>c.ProductId);
            
        modelBuilder.Entity<OrderItem>()
            .HasOne(o=>o.Order)
            .WithMany(o=>o.OrderItems)
            .HasForeignKey(o=>o.OrderId);
            
    }
    
}