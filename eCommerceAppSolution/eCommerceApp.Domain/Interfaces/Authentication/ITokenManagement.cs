using System.Security.Claims;
using eCommerceApp.Domain.Entites.Identity;

namespace eCommerceApp.Domain.Interfaces.Authentication;

public interface ITokenManagement
{
    string GetToken();
    string GenerateToken(List<Claim> claims);
    List<Claim> GetUserClaimsFromToken(string token);
    Task<bool> ValidateToken(string token);
    Task<string> GetUserIdFromToken(string token);
    Task<int> AddToken(string userId , string token);
    
    Task<int> UpdateToken(string userId , string token);
}