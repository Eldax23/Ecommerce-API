
using eCommerceApp.Application.DependancyInjection;
using eCommerceApp.Infrastructure.DependancyInjection;
using Serilog;
using Serilog.Core;

namespace eCommerceApp.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();
            Log.Logger.Information("Application Is Building.....");

            // Add services to the container.
            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureService(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddCors(builder =>
            {
                builder.AddDefaultPolicy(options =>
                {

                    options.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });


            // try
            // {


                var app = builder.Build();
                app.UseCors();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseInfrastructureService();

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                Log.Logger.Information("Application Is Running.....");
                app.Run();
            // }
            // catch (Exception ex)
            // {
            //     Log.Error(ex , "Application Failed To Start");
            // }
            // finally
            // {
            //     Log.CloseAndFlush();   
            // }
        }
    }
}
