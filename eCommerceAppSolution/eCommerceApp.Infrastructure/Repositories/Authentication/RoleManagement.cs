using System.Security.Claims;
using eCommerceApp.Domain.Entites.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using Microsoft.AspNetCore.Identity;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class RoleManagement : IRoleManagement
{
    private readonly UserManager<AppUser> _userManager;

    public RoleManagement(UserManager<AppUser> _userManager)
    {
        this._userManager = _userManager;
    }
    
    
    public async Task<string?> GetUserRole(string emailAddress)
    {
        AppUser? user = await _userManager.FindByEmailAsync(emailAddress);
        if(user == null)
            return "No user found";

        IList<string> roles = await _userManager.GetRolesAsync(user);
        string? role = roles.FirstOrDefault();
        if(string.IsNullOrEmpty(role))
            return "No role found";

        return role;
    }

    public async Task<bool> AddUserToRole(AppUser user, string roleName)
    {
        return (await _userManager.AddToRoleAsync(user , roleName)).Succeeded;
    }
}

