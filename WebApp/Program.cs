



using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Reviews___Ratings;
using Infrastructure.Services;
using Infrastructure.Services.Products___Categories;
using Infrastructure.Services.Reviews___Ratings;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options=>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DataContext>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IReviewsRatings, ReviewsRatings>();
builder.Services.AddScoped<IuserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

