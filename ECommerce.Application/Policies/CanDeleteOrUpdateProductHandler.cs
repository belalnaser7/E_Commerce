using ECommerce.Domain.Domain_Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerce.Application.Police
{
    public class CanDeleteOrUpdateProductHandler : AuthorizationHandler<CanDeleteOrUpdateProductRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanDeleteOrUpdateProductRequirement requirement, Product resource)
        {
            var userid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = context.User.IsInRole("Admin");
            if(isAdmin)
            {
              context.Succeed(requirement);
               
                return Task.CompletedTask;
            }
            var IsSeller = context.User.IsInRole("Seller");
            if (IsSeller&&resource.SellerId==userid)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
