using ECommerce.Application.Responses;
using System.Text.Json;

namespace ECommerce.Api.Middleware
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var bodyStream = context.Response.Body;
            using var memory = new MemoryStream();
            context.Response.Body = memory;
            await next(context);
            memory.Position = 0;
            var readingResponseFromMemory = await new StreamReader(memory).ReadToEndAsync();
            context.Response.Body = bodyStream;
            var wrappedResponse = new ApiResponse<object>
            {
                Success = context.Response.StatusCode < 400,
                Message = null,
                StatusCode = context.Response.StatusCode,
                Data = string.IsNullOrWhiteSpace(readingResponseFromMemory) ? null : JsonSerializer.Deserialize<object>(readingResponseFromMemory)
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync( JsonSerializer.Serialize(wrappedResponse));

        }
    }
}
