using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Getall()
        {
            IEnumerable<GetCategory> categories = await service.GetAllAsync();
            
            return categories.Any() ? Ok(categories) : NotFound(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            GetCategory category = await service.GetByIdAsync(id);
            return category != null ? Ok(category) : NotFound(category);
        }

        [HttpPost]
        public async Task<ActionResult> Add(CreateCategory category)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            ServiceResponse response = await service.AddAsync(category);
            
            return response.success ? Ok(response) : BadRequest(response);
        }
        
        [HttpPut]
        public async Task<ActionResult> Update(UpdateCategory category)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                    
            ServiceResponse response = await service.UpdateAsync(category);
                    
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
