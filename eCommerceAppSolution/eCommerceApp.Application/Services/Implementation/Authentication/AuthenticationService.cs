using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Identity;

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
        
        var newUser = mapper.Map<AppUser>(user);
        newUser.UserName = user.FullName;

        var passwordHasher = new PasswordHasher<AppUser>();
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, user.Password);
        
        bool result = await userManagement.CreateUser(newUser); 
        if(!result)
            return new ServiceResponse(message: "User Creation Service Failed");
        
        IEnumerable<AppUser?> users = await userManagement.GetAllUsers();
        AppUser? _user = await userManagement.GetUserByEmail(newUser!.Email!);

        //this makes the first added User An Admin.
        bool assignedRole = await roleManagement.AddUserToRole(_user, users?.Count() > 0 ? "User" : "Admin");

        if (!assignedRole)
        {
            logger.LogError(new Exception(message: "User Creation Failed") , "User Couldn't be assigned to role");
            return new ServiceResponse( success: false, message: "User Role Assignment Failed");
        }
        
        //verify email
        return new ServiceResponse(success: true , message: "User Creation Successful");
    }
    
    

    public async Task<LoginResponse> LoginUser(LoginUser user)
    {
        ServiceResponse validationResult = await validationService.ValidateAsync(user, loginUserValidator);
        if(!validationResult.success)
            return new LoginResponse(message: validationResult.message);

        AppUser mappedModel = mapper.Map<AppUser>(user);
        mappedModel.PasswordHash = user.Password;
        
        bool loginRes = await userManagement.LoginUser(mappedModel);
        if(!loginRes)
            return new LoginResponse(success: false , message: "Invalid Credentials");
        
        List<Claim> claims = await userManagement.GetUserClaims(mappedModel.Email!);

        string jwtToken = tokenManagement.GenerateToken(claims);

        string refereshToken = tokenManagement.GetToken();
        
        int saveTokenRes = await tokenManagement.AddToken(mappedModel.Id , refereshToken);
        
        return saveTokenRes <= 0 ? new LoginResponse(success: false , message: "Internal Server Error") : new LoginResponse(success: true , Token: jwtToken ,  refreshToken: refereshToken); 
    }

    public async Task<ServiceResponse> ForgetPassword(ForgetPassword user)
    {
        if(string.IsNullOrEmpty(user.NewPassword))
            return new ServiceResponse(message: "Password Cannot  Be Empty");
        
        bool result = await userManagement.ChangePassword(user.EmailAddress , user.NewPassword);
        
        if(!result)
            return new ServiceResponse(message: "Password Change Failed");
        
        return new ServiceResponse(success: true , message: "Password Change Successful");
    }

    public async Task<ServiceResponse> UpdateUser(CreateUser user)
    {
        
        ServiceResponse validationResult = await validationService.ValidateAsync(user , createUserValidator);
        if (!validationResult.success)
            return new ServiceResponse(success: false , message: validationResult.message);

        AppUser mappedUser = mapper.Map<AppUser>(user);
        mappedUser.PasswordHash = user.Password;

        bool result = await userManagement.UpdateUser(mappedUser);

        return result ? new ServiceResponse(success: true, message: "UserUpdatedSuccessfully") :
            new ServiceResponse(success: false, message: "UserUpdateFailed");
    }

    public async Task<LoginResponse> ReviveToken(string token)
    {
        bool validateToken = await tokenManagement.ValidateToken(token);
        if(!validateToken)
            return new LoginResponse(message: "Invalid Token");

        string userId = await tokenManagement.GetUserIdFromToken(token);

        AppUser? user = await userManagement.GetUserById(userId);
        
        string newRefereshToken = tokenManagement.GetToken();

        List<Claim> claims = await userManagement.GetUserClaims(user!.Email!);

        string newJwtToken = tokenManagement.GenerateToken(claims);
        
        await tokenManagement.UpdateToken(userId , newRefereshToken);
        
        return new LoginResponse(success: true ,Token: newJwtToken , refreshToken: newRefereshToken  , message: "User Revival Successful");
         
    }
}