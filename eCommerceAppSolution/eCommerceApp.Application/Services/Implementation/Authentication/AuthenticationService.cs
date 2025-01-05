using System.ComponentModel.DataAnnotations;
using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using eCommerceApp.Application.Services.Interfaces.Logging;
using eCommerceApp.Application.Validations;
using eCommerceApp.Application.Validations.Authentication;
using eCommerceApp.Domain.Entites.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using FluentValidation;

namespace eCommerceApp.Application.Services.Implementation.Authentication;

public class AuthenticationService(IUserManagement userManagement ,
    ITokenManagement tokenManagement ,
    IRoleManagement roleManagement,
    IAppLogger<AuthenticationService> logger,
    IMapper mapper ,
    IValidator<CreateUser> createUserValidator,
    IValidator<LoginUser> loginUserValidator,
    IValidationService validationService) : IAuthenticationService
{
    public async Task<ServiceResponse> CreateUser(CreateUser user)
    {
         var validationResult = await validationService.ValidateAsync(user , createUserValidator);
        if(!validationResult.success)
            return validationResult;
        
        AppUser newUser = mapper.Map<AppUser>(user);
        newUser.PasswordHash = user.Password;
        newUser.UserName = user.FullName;
        
        bool result = await userManagement.CreateUser(newUser); 
        if(!result)
            return new ServiceResponse(message: "User Creation Failed");
        
        IEnumerable<AppUser?> users = await userManagement.GetAllUsers();
        AppUser? _user = await userManagement.GetUserByEmail(newUser!.Email!);

        //this makes the first added User An Admin.
        bool assignedRole = await roleManagement.AddUserToRole(_user, users?.Count() > 0 ? "User" : "Admin");

        if (!assignedRole)
        {
            logger.LogError(new Exception(message: "User Creation Failed") , "User Couldn't be assigned to role");
            return new ServiceResponse( success: false, message: "User Creation Failed");
        }
        
        //verify email
        return new ServiceResponse(success: true , message: "User Creation Successful");
    }

    public Task<LoginResponse> LoginUser(LoginUser user)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponse> ReviveToken(string token)
    {
        throw new NotImplementedException();
    }
}