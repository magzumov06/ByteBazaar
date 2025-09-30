using Domain.DTOs.CategoryDto;
using Domain.Responces;

namespace Infrastructure.Interfaces.IProducts___ICategories;

public interface ICategoryService
{
    Task<Responce<string>> UpdateCategory(UpdateCategoryDto dto);
    Task<Responce<string>> DeleteCategory(int id);
    Task<Responce<string>> CreateCategory(CreateCategoryDto category);
    Task<Responce<List<GetCategoryDto>>> GetCategory();
}