using eCommerceApp.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}