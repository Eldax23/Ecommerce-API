using eCommerceApp.Application.DTOs;
using FluentValidation;
using FluentValidation.Results;

namespace eCommerceApp.Application.Validations;

public interface IValidationService
{
    Task<ServiceResponse> ValidateAsync<T>(T model , IValidator<T> validator);
}