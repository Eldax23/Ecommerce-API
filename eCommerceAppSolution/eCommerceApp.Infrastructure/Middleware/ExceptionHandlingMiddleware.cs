using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate _next)
{
       public async Task InvokeAsync(HttpContext context)
       {
              try
              {
                     await _next(context);
              }
              catch (DbUpdateException exception)
              {
                     if (exception.InnerException is SqlException innerException)
                     {
                            switch (innerException.Number)
                            {
                                   case 2627:
                                          context.Response.StatusCode = StatusCodes.Status409Conflict;
                                          await context.Response.WriteAsync("Unique Constraint violation");
                                          break;
                                   case 515:
                                          context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                          await context.Response.WriteAsync("Cannot Insert NULL");
                                          break;
                                   case 547:
                                          context.Response.StatusCode = StatusCodes.Status409Conflict;
                                          await context.Response.WriteAsync("Foriegn Key violation");
                                          break;
                                   default:
                                          context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                          await context.Response.WriteAsync($"DB Update Error: {exception.Message}");
                                          break;
                            }

                            


                     }
                     else
                     {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync("Error Occured While Saving Changes");
                     }

              }
              catch (Exception exception)
              {
                     context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                     await context.Response.WriteAsync("An Error Occured: " + exception.Message);
              }
       }
}