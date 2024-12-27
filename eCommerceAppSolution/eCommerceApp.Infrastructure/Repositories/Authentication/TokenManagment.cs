using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using eCommerceApp.Domain.Entites.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eCommerceApp.Infrastructure.Repositories.Authentication;

public class TokenManagment(AppDbContext context , IConfiguration config) : ITokenManagement
{
    public string GetToken()
    {
        
        byte[] randomBytes = new byte[64];
        using (RandomNumberGenerator rnd = RandomNumberGenerator.Create())
        {
            rnd.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }

    public string GenerateToken(List<Claim> claims)
    {
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        DateTime expiration = DateTime.UtcNow.AddHours(2);
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: config["JWT:Issuer"],
            audience: config["JWT:Audience"],
            expires: expiration,
            signingCredentials: credentials,
            claims: claims
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public List<Claim> GetUserClaimsFromToken(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);
        if (jwtToken == null)
            return [];
        
        return jwtToken.Claims.ToList();
    }

    public async Task<bool> ValidateToken(string token)
    {
        RefreshToken? refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        return refreshToken != null;
    }

    public async Task<string> GetUserIdFromToken(string token)
    {
        RefreshToken? refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        if(refreshToken == null)
            return string.Empty;
        
        return refreshToken.UserId;
    }

    public async Task<int> AddToken(string userId, string token)
    {
        RefreshToken newToken = new RefreshToken()
        {
            UserId = userId,
            Token = token
        };

        context.RefreshTokens.Add(newToken);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateToken(string userId, string token)
    {
        RefreshToken? oldToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
        if (oldToken is null)
            return -1;
        
        oldToken.Token = token;
        return await context.SaveChangesAsync();
    }
}