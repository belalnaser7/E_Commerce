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

                //var method = context.Request.Method;
                //var path = context.Request.Path;
                //var queryString = context.Request.QueryString;


                var userid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                logger.LogError(ex,"Error happened for UserId {UserId} at {Path}", userid,context.Request.Path);




                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    message = "Something went wrong",
                    statusCode = 500,
                    
                    
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

        }
    }
}
