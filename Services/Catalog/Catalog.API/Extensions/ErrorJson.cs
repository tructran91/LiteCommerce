using System.Text.Json;
using System.Text.Json.Serialization;

namespace Catalog.API.Extensions
{
    public static class ErrorJson
    {
        // Error bodies carry no data; omitting nulls lets clients typed BaseResponse<bool> still deserialize them.
        public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}
