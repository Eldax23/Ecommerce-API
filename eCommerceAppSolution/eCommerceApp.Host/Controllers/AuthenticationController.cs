using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
