using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace eCommerceApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginUser loginUser)
        {
            LoginResponse result = await _authenticationService.LoginUser(loginUser);
            if(!result.success)
                return BadRequest(result.message);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]CreateUser createUser)
        {
            ServiceResponse result = await _authenticationService.CreateUser(createUser);
            if(!result.success)
                return BadRequest(result.message);
         
            return Ok(result);  
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CreateUser createUser)
        {
            ServiceResponse result  = await _authenticationService.UpdateUser(createUser);
            
            if(!result.success)
                return BadRequest(result.message);
            
            return Ok(result);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ForgetPassword user)
        {
            if(!ModelState.IsValid)   
                return BadRequest(ModelState);

            ServiceResponse result = await _authenticationService.ForgetPassword(user);
            
            if(!result.success)
                return BadRequest(result.message);
            
            return Ok(result);
        }
    }
}
