using System.Security.Claims;
using eCommerceApp.Domain.Entites.Identity;

namespace eCommerceApp.Domain.Interfaces.Authentication;

public interface IUserManagement
{
    Task<bool> CreateUser(AppUser user);
    Task<bool> LoginUser(AppUser user);
    Task<AppUser?> GetUserByEmail(string emailAddress);
    Task<AppUser?> GetUserById(string id);
    Task<IEnumerable<AppUser?>> GetAllUsers();
    Task<int> RemoveUserByEmail(string email);
    Task<List<Claim>> GetUserClaims(string emailAddress);
}