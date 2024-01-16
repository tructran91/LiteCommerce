namespace Catalog.Application.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public static BaseResponse<T> Success(T data)
        {
            return new BaseResponse<T>(true, string.Empty, data);
        }

        public static BaseResponse<T> Failure(string errors, T data)
        {
            return new BaseResponse<T>(false, errors, data);
        }
    }
}
