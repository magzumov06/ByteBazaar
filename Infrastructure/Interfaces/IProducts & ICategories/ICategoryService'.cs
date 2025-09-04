using Domain.DTOs.CategoryDto;
using Infrastructure.Responces;

namespace Infrastructure.Interfaces;

public interface ICategoryService
{
    Task<Responce<string>> CreateCategory(CreateCategoryDto category);
    Task<Responce<List<GetCategoryDto>>> GetCategory();
}