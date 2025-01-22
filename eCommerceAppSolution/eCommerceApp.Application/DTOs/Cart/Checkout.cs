namespace eCommerceApp.Application.DTOs.Cart;

public class Checkout
{
    public Guid PaymentId { get; set; }
    public IEnumerable<ProcessCart> Carts { get; set; }
}