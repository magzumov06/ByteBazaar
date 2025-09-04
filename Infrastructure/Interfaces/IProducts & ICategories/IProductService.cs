using Domain.DTOs.ProductDto;
using Domain.Filters;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces;

public interface IProductService
{
    Task<Responce<string>> CreateProduct(CreateProductDto create);
    Task<Responce<string>> UpdateProduct(UpdateProductDto update);
    Task<Responce<string>> DeleteProduct(int Id);
    Task<Responce<GetProductDto>>GetProductById(int Id);
    Task<PaginationResponce<List<GetProductDto>>>GetProducts(ProductFilter filter);
}