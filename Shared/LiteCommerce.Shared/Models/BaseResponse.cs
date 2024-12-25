using LiteCommerce.Shared.Models;
using System.Net;

namespace Catalog.Application.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse(bool isSuccess, string message, T data, HttpStatusCode statusCode = HttpStatusCode.OK)
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

        public HttpStatusCode StatusCode { get; set; }

        public Dictionary<string, List<string>>? Errors { get; set; }

        public static BaseResponse<T> Success(T data, string message = "Request succeeded", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new BaseResponse<T>(true, message, data, statusCode);
        }

        public static BaseResponse<T> Failure(string message, Dictionary<string, List<string>>? errors = null, T? data = default, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new BaseResponse<T>(false, message, data, statusCode)
            {
                Errors = errors
            };
        }
    }
}
