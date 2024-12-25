using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entites;
using eCommerceApp.Domain.Interfaces;

namespace eCommerceApp.Application.Services.Implementation;

public class CategoryService(IGeneric<Category> categoryInterface , IMapper mapper) : ICategoryService
{
    public async Task<IEnumerable<GetCategory>> GetAllAsync()
    {
        var categories = await categoryInterface.GetAllAsync();
        if(!categories.Any())
            return new List<GetCategory>();

        return mapper.Map<IEnumerable<GetCategory>>(categories);
    }

    public async Task<GetCategory> GetByIdAsync(Guid id)
    {
        Category category = await categoryInterface.GetByIdAsync(id);

        if (category == null)
            return new GetCategory();

        return mapper.Map<GetCategory>(category);
    }

    public async Task<ServiceResponse> AddAsync(CreateCategory category)
    {
        Category categoryDto = mapper.Map<Category>(category);
        int result = await categoryInterface.AddAsync(categoryDto);
        
        return result > 0 ? new ServiceResponse(true , "Category Added Succesfully")
            : new ServiceResponse(false, "Category Add Failed");
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateCategory category)
    {
        Category categoryToUpdate = mapper.Map<Category>(category);
        int result = await categoryInterface.UpdateAsync(categoryToUpdate);
        return result > 0 ? new ServiceResponse(true , "Category successfully updated") :
            new ServiceResponse(false, "Category could not be updated");
        
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        int result = await categoryInterface.DeleteAsync(id);
        return result > 0 ? new ServiceResponse(true , "Category successfully deleted") : new ServiceResponse(false , "Category could not be deleted");
        
    }
}