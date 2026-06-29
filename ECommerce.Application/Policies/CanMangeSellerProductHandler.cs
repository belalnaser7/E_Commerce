using ECommerce.Domain.Domain_Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerce.Application.Police
{
    public class CanMangeSellerProductHandler : AuthorizationHandler<CanMangeSellerProductRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanMangeSellerProductRequirement requirement, Product resource)
        {
            var userid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
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
