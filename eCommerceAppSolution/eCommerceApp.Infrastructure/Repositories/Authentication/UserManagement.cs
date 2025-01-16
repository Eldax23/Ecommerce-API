using System.Security.Claims;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Domain.Entites.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class UserManagement(IRoleManagement roleManagement, UserManager<AppUser> userManager , AppDbContext context  ) : IUserManagement
{
    public async Task<bool> CreateUser(AppUser user)
    {
        AppUser? tempUser = await GetUserByEmail(user.Email!);
        
        // if(tempUser != null) //user exists in the system
        //     return false;

        IdentityResult result = await userManager.CreateAsync(user);

        string errors = string.Empty;
        if (!result.Succeeded ) 
        {
            foreach (var error in result.Errors)
            {
                errors += $"{error.Code}: {error.Description}\n";
            }
        }



        return true;


    }

    public async Task<bool> LoginUser(AppUser user)
    {
        AppUser? tempUser = await GetUserByEmail(user.Email!);
        string? roleName = await roleManagement.GetUserRole(user.Email!);
        if (tempUser is null || roleName is null)
            return false;

        var passwordHasher = new PasswordHasher<AppUser>();
        var result = passwordHasher.VerifyHashedPassword(tempUser, tempUser.PasswordHash!, user.PasswordHash!);

        return await userManager.CheckPasswordAsync(tempUser, user.PasswordHash!);
    }

    public async Task<bool> ChangePassword(string id, string newPassword)
    {
        AppUser? tempUser = await GetUserByEmail(id);
        if (tempUser is null)
            return false;
     
        var passwordHasher = new PasswordHasher<AppUser>();
        tempUser.PasswordHash = passwordHasher.HashPassword(tempUser , newPassword);


        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateUser(AppUser user)
    {
        if (user is null)
            return false;

        if (string.IsNullOrWhiteSpace(user.Email))
            return false;
        
        var passwordHasher = new PasswordHasher<AppUser>();
         user.PasswordHash = passwordHasher.HashPassword(user , user.PasswordHash!);
         context.Users.Update(user);
        return await context.SaveChangesAsync() > 0;
    }



    public async Task<AppUser?> GetUserByEmail(string emailAddress)
        => await userManager.FindByEmailAsync(emailAddress);

    public async Task<AppUser?> GetUserById(string id)
        => await userManager.FindByIdAsync(id);

    public async Task<IEnumerable<AppUser?>> GetAllUsers()
        => await context.Users.ToListAsync(); 

    public async Task<int> RemoveUserByEmail(string email)
    {
        AppUser? user = await GetUserByEmail(email);
        if (user is null)
            return -1;

        context.Users.Remove(user);
        return await context.SaveChangesAsync();
    }

    public async Task<List<Claim>> GetUserClaims(string emailAddress)
    {
        AppUser? user = await userManager.FindByEmailAsync(emailAddress);
        
        string? roleName = await roleManagement.GetUserRole(emailAddress);
        if (user is null || roleName is null)
            return new List<Claim>();

        
        List<Claim> claims = new List<Claim>()
        {
            new Claim("FullName" , user.FullName),
            new Claim(ClaimTypes.NameIdentifier , user.Id),
            new Claim(ClaimTypes.Email , user.Email!),
            new Claim(ClaimTypes.Role , roleName)
        };

        return claims;
    }
}