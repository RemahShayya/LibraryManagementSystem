using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LibraryManagementSystem.API.Exceptions
{
    public class GlobalExceptionHandlerMiddleWare : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleWare> _logger;

        public GlobalExceptionHandlerMiddleWare(ILogger<GlobalExceptionHandlerMiddleWare> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "_ WHAT HAVE YOU DONE!!!");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var problemDetails = new ProblemDetails
                {
                    Status = context.Response.StatusCode,
                    Title = "ERROR!",
                    Detail = ex.Message
                };

                string json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
