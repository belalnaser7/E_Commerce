using ECommerce.Domain.Domain_Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerce.Application.Police
{
    public class AccessUserOnHisOrderHandler : AuthorizationHandler<AccessUserOnHisOrderRequirement, Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessUserOnHisOrderRequirement requirement, Order resource)
        {
            var userid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = context.User.IsInRole("Admin");
            if (isAdmin)
            {
                context.Succeed(requirement);

                return Task.CompletedTask;
            }
            var IsUser = context.User.IsInRole("Customer");

            if (IsUser&& resource.UserId == userid)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
