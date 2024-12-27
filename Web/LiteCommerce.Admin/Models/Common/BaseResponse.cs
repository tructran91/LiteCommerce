using System.Net;

namespace LiteCommerce.Admin.Models.Common
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public Pagination? Pagination { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public Dictionary<string, List<string>>? Errors { get; set; }
    }
}
