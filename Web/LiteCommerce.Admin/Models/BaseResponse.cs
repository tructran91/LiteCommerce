using System.Net;

namespace LiteCommerce.Admin.Models
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

    public class Pagination
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}
