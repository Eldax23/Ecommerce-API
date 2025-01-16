using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;

namespace eCommerceApp.Application.Services.Interfaces.Authentication;

public interface IAuthenticationService
{
  Task<ServiceResponse> CreateUser(CreateUser user);
  Task<ServiceResponse> UpdateUser(CreateUser user);
  
  Task<ServiceResponse> ForgetPassword(ForgetPassword user);
  
  Task<LoginResponse> LoginUser (LoginUser user);
  Task<LoginResponse> ReviveToken(string token);
}