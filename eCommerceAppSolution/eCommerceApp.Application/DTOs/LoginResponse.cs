namespace eCommerceApp.Application.DTOs;

public record LoginResponse
(bool success = false ,
    string message = null! ,
    string Token = null!,
    string refreshToken = null!);