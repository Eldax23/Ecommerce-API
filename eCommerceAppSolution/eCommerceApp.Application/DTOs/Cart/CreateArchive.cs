namespace eCommerceApp.Application.DTOs.Cart;

public class CreateArchive
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
}