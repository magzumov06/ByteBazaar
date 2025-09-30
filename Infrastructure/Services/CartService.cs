using System.Net;
using Domain.DTOs.CartItemDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class CartService(DataContext context) : ICartService
{
    public async Task<Responce<string>> AddToCart(CreateCartItemDto create)
    {
        try
        {
            Log.Information("Adding to cart");
            if(create.Quantity < 1 )
                return new Responce<string>(HttpStatusCode.BadRequest,"Quantity must be greater than 0");
            var cartItem = await context.CartItems.FirstOrDefaultAsync(x=>x.UserId == create.UserId && x.ProductId == create.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += create.Quantity;
                cartItem.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                return new Responce<string>(HttpStatusCode.OK,"Product quantity updated successfully");
            }
            var newCart = new CartItem
            {
                UserId = create.UserId,
                ProductId = create.ProductId,
                Quantity = create.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            await context.CartItems.AddAsync(newCart);
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Adding to cart");
            }
            else
            {
                Log.Fatal("Failed to add to cart");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created, "CartItem successfully added")
                : new Responce<string>(HttpStatusCode.BadRequest, "Cart item could not be added");
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateCart");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateCart(UpdateCartItemDto update)
    {
        try
        {
            Log.Information("Updating cart");
            var updatedCart = await context.CartItems.FirstOrDefaultAsync(x => x.Id == update.Id);
            if (updatedCart == null) return new Responce<string>(HttpStatusCode.NotFound, "CartItem not found");
            updatedCart.Quantity = update.Quantity;
            updatedCart.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Updating cart");
            }
            else
            {
                Log.Fatal("Failed to update cart");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "CartItem successfully updated")
                : new Responce<string>(HttpStatusCode.BadRequest, "Error");
        }
        catch (Exception e)
        {
            Log.Error("Error in  UpdateCart");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteCart(int id)
    {
        try
        {
            Log.Information("Deleting cart");
            var deletedCart = await context.CartItems.FirstOrDefaultAsync(x => x.Id == id);
            if (deletedCart == null) return new Responce<string>(HttpStatusCode.NotFound, "CartItem not found");
            context.CartItems.Remove(deletedCart);
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Deleting cart");
            }
            else
            {
                Log.Fatal("Failed to delete cart");
            }
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "CartItem successfully deleted")
                : new Responce<string>(HttpStatusCode.BadRequest, "Error");
        }
        catch (Exception e)
        {
            Log.Error("Error in  DeleteCart");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetCartItemDto>>> GetCartItem(int userId)
    {
        try
        {
            Log.Information("Getting cart");
            var cartItems = await context.CartItems
                .Where(x => x.UserId == userId)
                .ToListAsync();
            if(cartItems.Count == 0) return new Responce<List<GetCartItemDto>>(HttpStatusCode.NotFound, "CartItem not found");
            var dtos = cartItems.Select(x => new GetCartItemDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
            return new Responce<List<GetCartItemDto>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetCartItem");
            return new Responce<List<GetCartItemDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}
