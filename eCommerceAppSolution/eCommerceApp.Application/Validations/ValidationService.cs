using eCommerceApp.Application.DTOs;
using FluentValidation;

namespace eCommerceApp.Application.Validations;

public class ValidationService : IValidationService
{
    public async Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            List<string> errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            string errorsString = string.Join(',', errors);
            return new ServiceResponse(message: errorsString);
        }
        
        return new ServiceResponse(success: true);
    }
}