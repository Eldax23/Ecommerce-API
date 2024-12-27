using System.Security.Claims;
using eCommerceApp.Domain.Entites.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class UserManagement(IRoleManagement roleManagement, UserManager<AppUser> userManager , AppDbContext context) : IUserManagement
{
    public async Task<bool> CreateUser(AppUser user)
    {
        AppUser? tempUser = await GetUserByEmail(user.Email!);
        
        if(tempUser != null) //user exists in the system
            return false;

        return (await userManager.CreateAsync(user, user.PasswordHash!)).Succeeded;

    }

    public async Task<bool> LoginUser(AppUser user)
    {
        AppUser? tempUser = await GetUserByEmail(user.Email!);
        string? roleName = await roleManagement.GetUserRole(user.Email!);
        if (tempUser is null || roleName is null)
            return false;


        return await userManager.CheckPasswordAsync(tempUser, user.PasswordHash!);
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