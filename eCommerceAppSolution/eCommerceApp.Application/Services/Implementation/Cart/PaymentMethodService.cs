using AutoMapper;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using eCommerceApp.Domain.Entites.Cart;
using eCommerceApp.Domain.Interfaces.Cart;

namespace eCommerceApp.Application.Services.Implementation.Cart;

public class PaymentMethodService(IPaymentMethod paymentMethod , IMapper mapper) : IPaymentMethodService
{
    public async Task<IEnumerable<GetPaymentMethod>> GetPaymentMethod()
    {
        IEnumerable<PaymentMethod> paymentMethods = await paymentMethod.GetPaymentMethodsAsync();
        if(!paymentMethods.Any())
            return new List<GetPaymentMethod>();
        return mapper.Map<IEnumerable<GetPaymentMethod>>(paymentMethods);
    }
}