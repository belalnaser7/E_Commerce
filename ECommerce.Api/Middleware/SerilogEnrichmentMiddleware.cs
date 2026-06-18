using Serilog.Context;
using System.Security.Claims;

namespace ECommerce.Api.Middleware
{
    public class SerilogEnrichmentMiddleware

    {
        private readonly RequestDelegate next;

        public SerilogEnrichmentMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var userId = context.User.Identity.IsAuthenticated
               ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               : "Anonymous";

            using (LogContext.PushProperty("UserId", userId))// ال using scope عشان كل ما لوج تخلص البيانات تتمسح وتفضي عشان ميحصلش تعارض
            using (LogContext.PushProperty("Path", path))
            using (LogContext.PushProperty("Method", method))
            {
                await next(context);

            }

                

        }
    }
}
