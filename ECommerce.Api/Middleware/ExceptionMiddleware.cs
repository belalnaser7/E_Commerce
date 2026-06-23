using ECommerce.Application.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerce.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                var userid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                logger.LogError(ex,"Error happened for UserId {UserId} at {Path}", userid,context.Request.Path);
                var statusCode = ex switch
                {
                   // NotFoundException => 404,
                   
                    //UnauthorizedAccessException => 401,
                    //ArgumentException => 400,
                    _ => 500
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new 
                {
                    Success=false,
                    Message = "Something went wrong",
                    StatusCode = 500,
                   
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

        }
    }
}
