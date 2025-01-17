using eCommerceApp.Domain.Entites.Cart;
using eCommerceApp.Domain.Interfaces.Cart;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Repositories.Cart;

public class PaymentMethodRepository(AppDbContext context) : IPaymentMethod
{
    public async Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync()
    {
        return await context.PaymentMethods.AsNoTracking().ToListAsync();
    }
}