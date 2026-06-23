using ECommerce.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Result_pattern
{
    public enum ErrorType
    {
        None = 0,
        BadRequest = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4
    }
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }
        public ErrorType ErrorType { get; protected set; }
        public static Result Success()
        {
            return new Result { IsSuccess = true,ErrorType = ErrorType.None };
        }
        public static Result Fail(string Error, ErrorType type = ErrorType.BadRequest)
        {
            return new Result { IsSuccess = false,ErrorMessage=Error, ErrorType = type };
        }

       
    }
}
