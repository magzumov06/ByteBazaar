using Domain.DTOs.ProductDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces.IProducts___ICategories;

public interface IProductService
{
    Task<Responce<string>> CreateProduct(CreateProductDto create);
    Task<Responce<string>> UpdateProduct(UpdateProductDto update);
    Task<Responce<string>> DeleteProduct(int id);
    Task<Responce<GetProductDto>>GetProductById(int id);
    Task<PaginationResponce<List<GetProductDto>>>GetProducts(ProductFilter filter);
}