using ECommerce.Application.Police;
using ECommerce.Domain.Domain_Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class CanViewCartHandler
    : AuthorizationHandler<CanViewCartRequirement, Cart>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanViewCartRequirement requirement,
        Cart resource)
    {
        var currentUserId =
            context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Admin يشوف أي Cart
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Customer يشوف Cart نفسه فقط
        if (resource.UserId == currentUserId)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}