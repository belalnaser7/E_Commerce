namespace ECommerce.Application.Result_pattern
{
    public class Result<T>:Result
    {
      public T? Data { get; private set; }
        public static Result<T> Success(T Data)
        {
            return new Result<T> { IsSuccess = true ,Data=Data , ErrorType = ErrorType.None };
        }
        public static Result<T> Fail(string Error, ErrorType type = ErrorType.BadRequest)
        {
            return new Result<T> { IsSuccess = false, ErrorMessage = Error, ErrorType = type };
        }
    }
}
