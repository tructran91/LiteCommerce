using LiteCommerce.Shared.Models;

namespace Catalog.Application.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse(bool isSuccess, string message, T data, int statusCode = 200)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public Pagination? Pagination { get; set; }

        public int StatusCode { get; set; }

        public Dictionary<string, List<string>>? Errors { get; set; }

        public static BaseResponse<T> Success(T data, string message = "Request succeeded", int statusCode = 200)
        {
            return new BaseResponse<T>(true, string.Empty, data, statusCode);
        }

        public static BaseResponse<T> Failure(string errors, T data)
        {
            return new BaseResponse<T>(false, errors, data);
        }

        public static BaseResponse<T> Failure(string message, Dictionary<string, List<string>>? errors = null, T? data = default, int statusCode = 400)
        {
            return new BaseResponse<T>(false, message, data, statusCode)
            {
                Errors = errors
            };
        }
    }
}
