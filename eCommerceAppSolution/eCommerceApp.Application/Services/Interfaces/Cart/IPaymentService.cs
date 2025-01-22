using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Domain.Entites;

namespace eCommerceApp.Application.Services.Interfaces.Cart;

public interface IPaymentService
{
    Task<ServiceResponse> Pay(decimal amount, IEnumerable<Product> cartProducts, IEnumerable<ProcessCart> carts);
}