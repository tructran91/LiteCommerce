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

        public string GetErrorMessage(string fallback)
        {
            if (Errors != null && Errors.Count > 0)
                return string.Join(", ", Errors.SelectMany(e => e.Value.Select(msg => $"{e.Key}: {msg}")));

            return string.IsNullOrWhiteSpace(Message) ? fallback : Message;
        }
    }
}
