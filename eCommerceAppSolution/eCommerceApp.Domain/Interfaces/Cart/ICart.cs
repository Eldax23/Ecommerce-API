using eCommerceApp.Domain.Entites.Cart;

namespace eCommerceApp.Domain.Interfaces.Cart;

public interface ICart
{
    Task<int> SaveCheckoutHistory(IEnumerable<Archive> checkouts);
}