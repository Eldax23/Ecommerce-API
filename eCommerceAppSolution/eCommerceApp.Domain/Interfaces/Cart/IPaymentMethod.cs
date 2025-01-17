using eCommerceApp.Domain.Entites.Cart;

namespace eCommerceApp.Domain.Interfaces.Cart;

public interface IPaymentMethod
{
    Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync();
}