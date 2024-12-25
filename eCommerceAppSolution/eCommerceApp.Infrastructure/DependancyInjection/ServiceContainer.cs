using eCommerceApp.Domain.Entites;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Infrastructure.Data;
using eCommerceApp.Infrastructure.Middleware;
using eCommerceApp.Infrastructure.Repositories;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerceApp.Infrastructure.DependancyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = "Default";
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString(connectionString) , b =>
                b.MigrationsAssembly(typeof(ServiceContainer).Assembly.FullName)).UseExceptionProcessor(),
                ServiceLifetime.Scoped);

        services.AddScoped<IGeneric<Product>, GenericRepository<Product>>();
        services.AddScoped<IGeneric<Category>, GenericRepository<Category>>();
        
        return services;
    }

    public static IApplicationBuilder UseInfrastructureService(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}