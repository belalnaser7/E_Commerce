using ECommerce.Application.Responses;
using ECommerce.Application.Result_pattern;

namespace ECommerce.Api.Extentions
{
    public static class ApiResponseMapper
    {
        public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
        {
            return new ApiResponse<T>
            {
                Success = result.IsSuccess,
                Message = result.ErrorMessage,
                StatusCode = MapStatus(result.ErrorType),
                Data = result.Data
            };
        }

        public static ApiResponse<object> ToApiResponse(this Result result)
        {
            return new ApiResponse<object>
            {
                Success = result.IsSuccess,
                Message = result.ErrorMessage,
                StatusCode = MapStatus(result.ErrorType),
                Data = null

            };
        }
        private static int MapStatus(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.NotFound => 404,
                ErrorType.BadRequest => 400,
                ErrorType.Unauthorized => 401,
                ErrorType.Conflict => 409,
                _ => 200
            };
        }
    }
}
