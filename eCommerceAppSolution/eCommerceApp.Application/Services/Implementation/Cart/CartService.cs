using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using eCommerceApp.Domain.Entites;
using eCommerceApp.Domain.Entites.Cart;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Domain.Interfaces.Cart;

namespace eCommerceApp.Application.Services.Implementation.Cart;

public class CartService(ICart cartInterface , IMapper mapper , IGeneric<Product> productInterface , IPaymentMethodService paymentMethodService
, IPaymentService paymentService) : ICartService
{
    public async Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateArchive> archives)
    {
        IEnumerable<Archive> mappedArchives = mapper.Map<IEnumerable<Archive>>(archives);
        int result =  await cartInterface.SaveCheckoutHistory(mappedArchives);
        return result > 0 ? new ServiceResponse(true , "Checkout Archived!") : 
            new ServiceResponse(false , "Error Occured In Saving.") ;
    }

    public async Task<ServiceResponse> Checkout(Checkout checkout)
    {
        var (cartProducts, totalAmount) = await GetTotalAmount(checkout.Carts);
        IEnumerable<GetPaymentMethod> paymentMethods = await paymentMethodService.GetPaymentMethod();
        if (checkout.PaymentId == paymentMethods.FirstOrDefault().Id)
        {
            return await paymentService.Pay(totalAmount, cartProducts, checkout.Carts);
        }
        
        
        return new ServiceResponse(false , "Invalid Payment Method!") ;
    }

    private async Task<(IEnumerable<Product>, decimal)> GetTotalAmount(IEnumerable<ProcessCart> carts)
    {
        if (!carts.Any())
            return ([], 0);

        IEnumerable<Product> products = await productInterface.GetAllAsync();
        
        if(!products.Any())
            return ([], 0);
        
        List<Product> cartProducts = carts.Select(cartItem => products.FirstOrDefault(prd => prd.Id == cartItem.ProductId))
            .Where(prd => prd != null)
            .ToList();
        
        decimal totalAmount = carts.Where(cartItem => cartProducts.Any(prd => prd.Id == cartItem.ProductId))
            .Sum(cartItem => cartItem.Quantity * cartProducts.First(prd => prd.Id == cartItem.ProductId).Price);

        return (cartProducts , totalAmount);

    }
}