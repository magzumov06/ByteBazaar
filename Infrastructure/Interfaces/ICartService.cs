using Domain.DTOs.CartItemDto;
using Infrastructure.Responces;

namespace Infrastructure.Services;

public interface ICartService
{
    Task<Responce<string>> AddToCart(CreateCartItemDto create);
    Task<Responce<string>> UpdateCart(UpdateCartItemDto update);
    Task<Responce<string>> DeleteCart(int Id);
    Task<Responce<List<GetCartItemDto>>> GetCartItem(Guid UserId);
}