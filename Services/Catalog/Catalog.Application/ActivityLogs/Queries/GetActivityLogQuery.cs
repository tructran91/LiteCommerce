using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ActivityLogs.Queries
{
    public class GetActivityLogQuery : IRequest<BaseResponse<ActivityLogResponse>>
    {
        public string Id { get; set; }

        public GetActivityLogQuery(string id)
        {
            Id = id;
        }
    }
}
