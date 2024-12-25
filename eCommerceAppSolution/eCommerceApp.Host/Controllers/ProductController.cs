using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Services.Interfaces;
using eCommerceApp.Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace eCommerceApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Getall()
        {
            IEnumerable<GetProduct> products = await service.GetAllAsync();
            
            return products.Any() ? Ok(products) : NotFound(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            GetProduct product = await service.GetByIdAsync(id);
            return product != null ? Ok(product) : NotFound(product);
        }

        [HttpPost]
        public async Task<ActionResult> Add(CreateProduct product)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            ServiceResponse response = await service.AddAsync(product);
            
            return response.success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut]
        public async Task<ActionResult> Add(UpdateProduct product)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                    
            ServiceResponse response = await service.UpdateAsync(product);
                    
            return response.success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            ServiceResponse response = await service.DeleteAsync(id);
            return response.success ? Ok(response) : BadRequest(response);
        }
    }
}
