using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.API.Exceptions
{
    internal sealed class GlobalExceptionHandlerMiddleWare(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleWare> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "_ WHAT HAVE YOU DONE!");

                context.Response.StatusCode = ex switch
                {
                    ApplicationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                await context.Response.WriteAsJsonAsync(
                    new ProblemDetails
                    {
                        Type = ex.GetType().Name,
                        Title = "An error has occurred",
                        Detail = ex.Message
                    }
                    );
            }
        }
    }
}