using System.ComponentModel.DataAnnotations;
using eCommerceApp.Domain.Entites;

namespace eCommerceApp.Domain.Entites
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
