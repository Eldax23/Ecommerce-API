using eCommerceApp.Domain.Entites.Identity;

namespace eCommerceApp.Domain.Interfaces.Authentication;

public interface IRoleManagement
{
    Task<string?> GetUserRole(string emailAddress);
    Task<bool> AddUserToRole(AppUser user , string roleName);
}