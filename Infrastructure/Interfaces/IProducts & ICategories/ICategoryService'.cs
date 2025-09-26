using Domain.DTOs.CategoryDto;
using Domain.Responces;

namespace Infrastructure.Interfaces.IProducts___ICategories;

public interface ICategoryService
{
    Task<Responce<string>> CreateCategory(CreateCategoryDto category);
    Task<Responce<List<GetCategoryDto>>> GetCategory();
}