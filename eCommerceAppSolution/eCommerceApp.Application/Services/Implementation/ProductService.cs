using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entites;
using eCommerceApp.Domain.Interfaces;

namespace eCommerceApp.Application.Services.Implementation;

public class ProductService(IGeneric<Product> productInterface , IMapper mapper) : IProductService
{
    public async Task<IEnumerable<GetProduct>> GetAllAsync()
    {
        var products = await productInterface.GetAllAsync();
        if(!products.Any())
            return new List<GetProduct>();

        return mapper.Map<IEnumerable<GetProduct>>(products);
    }

    public async Task<GetProduct> GetByIdAsync(Guid id)
    {
        var product = await productInterface.GetByIdAsync(id);
        if (product == null)
            return new GetProduct();

        return mapper.Map<GetProduct>(product);
    }

    public async Task<ServiceResponse> AddAsync(CreateProduct product)
    {
        Product mappedProduct = mapper.Map<Product>(product);
        int result = await productInterface.AddAsync(mappedProduct);
        
        if(result > 0)
            return new ServiceResponse(true , "Product successfully added");
        
        return new ServiceResponse(false , "Product could not be added");
    }

    public async Task<ServiceResponse> UpdateAsync(UpdateProduct product)
    {
        Product productToUpdate = mapper.Map<Product>(product);
        int result = await productInterface.UpdateAsync(productToUpdate);
        return result > 0 ? new ServiceResponse(true , "Product successfully updated") :
            new ServiceResponse(false, "Product could not be updated");
        
    }

    public async Task<ServiceResponse> DeleteAsync(Guid id)
    {
        int result = await productInterface.DeleteAsync(id);
        return result > 0 ? new ServiceResponse(true , "Product successfully deleted") : new ServiceResponse(false , "Product not found or  could not be deleted");
        
    }
}