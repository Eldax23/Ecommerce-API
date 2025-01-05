using eCommerceApp.Application.DTOs.Identity;
using FluentValidation;

namespace eCommerceApp.Application.Validations.Authentication;

public class LoginUserValidation : AbstractValidator<LoginUser>
{
    public LoginUserValidation()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address format");
        
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required");
    }   
}