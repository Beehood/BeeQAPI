using Models;
using System.Net;
using System.Text.Json;

namespace BeeQAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK; 

            var response = new APIGetResponseModel<object>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Something went wrong. Please try again later." },
                Result = null
            };

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}