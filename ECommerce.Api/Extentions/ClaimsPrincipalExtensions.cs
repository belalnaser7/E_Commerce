using ECommerce.Application.Result_pattern;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.Extentions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string?  GetUserid(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public static string? GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string? GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        
    }

    public static class ResultPattern
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            var response = result.ToApiResponse();

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
        public static IActionResult ToActionResult(this Result result)
        {
            var response = result.ToApiResponse();

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
                
            };
        }
        //public static IActionResult ToActionResult(this Result result)
        //{
        //    return result.ErrorType switch
        //    {
        //        ErrorType.NotFound => new NotFoundObjectResult(result.ErrorMessage),
        //        ErrorType.Conflict => new ConflictObjectResult(result.ErrorMessage),
        //        ErrorType.BadRequest => new BadRequestObjectResult(result.ErrorMessage),
        //        ErrorType.Unauthorized => new UnauthorizedObjectResult(result.ErrorMessage),

        //        _ => new OkResult()
        //    };
        //}
    }
}
