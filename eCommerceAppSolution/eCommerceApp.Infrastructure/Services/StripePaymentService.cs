using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using Stripe.Checkout;
using Stripe.Climate;
using Product = eCommerceApp.Domain.Entites.Product;

namespace eCommerceApp.Infrastructure.Services;

public class StripePaymentService : IPaymentService
{
    public async Task<ServiceResponse> Pay(decimal amount, IEnumerable<Product> cartProducts, IEnumerable<ProcessCart> carts)
    {
        try
        {
            List<SessionLineItemOptions> lineItems = new List<SessionLineItemOptions>();
            foreach (Product product in cartProducts)
            {
                int productQuantity = carts.FirstOrDefault(prd => prd.ProductId == product.Id).Quantity;
                lineItems.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = product.Name,
                            Description = product.Description,
                        },
                        UnitAmount = (long)product.Price,
                    },
                    Quantity = productQuantity,

                });
            }

            SessionCreateOptions options = new SessionCreateOptions()
            {
                PaymentMethodTypes = ["usd"],
                LineItems = lineItems,
                SuccessUrl = "https://localhost:5071/payment-success",
                CancelUrl = "https://localhost:5071/payment-cancel",
            };
            SessionService sessionService = new SessionService();
            Session session = await sessionService.CreateAsync(options);
            return new ServiceResponse(true, session.Url);

        }
        catch (Exception ex)
        {
            return new ServiceResponse(false, ex.Message);
        }
   }
}