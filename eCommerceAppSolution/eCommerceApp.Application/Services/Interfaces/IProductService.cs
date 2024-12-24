using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Domain.Entites;

namespace eCommerceApp.Application.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<CreateProduct>> GetAllAsync();
    Task<GetProduct> GetByIdAsync(Guid id);
    Task<ServiceResponse> AddAsync(CreateProduct product);
    Task<ServiceResponse> UpdateAsync(UpdateProduct product);
    Task<ServiceResponse> DeleteAsync(Guid id);
    
}