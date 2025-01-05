using eCommerceApp.Application.DTOs.Identity;
using FluentValidation;

namespace eCommerceApp.Application.Validations.Authentication;

public class CreateUserValidation : AbstractValidator<CreateUser>
{
    public CreateUserValidation()
    {
        RuleFor(u => u.FullName)
            .NotEmpty().WithMessage("Full name is required");
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required");
        
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            ;
        
        RuleFor(u => u.ConfirmPassword)
            .Equal(u => u.Password).WithMessage("Passwords do not match");
        
    }

}