namespace eCommerceApp.Application.DTOs.Identity;

public class CreateUser : BaseModel
{
    public string FullName { get; set; }
    public string ConfirmPassword { get; set; }
}