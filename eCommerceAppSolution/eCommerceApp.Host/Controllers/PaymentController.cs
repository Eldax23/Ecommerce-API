using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentMethodService paymentService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetAllPayments()
        {
            IEnumerable<GetPaymentMethod> paymentMethods = await paymentService.GetPaymentMethod();
            if(!paymentMethods.Any())
                return BadRequest("No payment method found");
            
            return Ok(paymentMethods);
        }
    }
}
