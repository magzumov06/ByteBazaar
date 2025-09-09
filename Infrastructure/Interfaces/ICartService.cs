using Domain.DTOs.CartItemDto;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces;

public interface ICartService
{
    Task<Responce<string>> AddToCart(CreateCartItemDto create);
    Task<Responce<string>> UpdateCart(UpdateCartItemDto update);
    Task<Responce<string>> DeleteCart(int id);
    Task<Responce<List<GetCartItemDto>>> GetCartItem(Guid userId);
}