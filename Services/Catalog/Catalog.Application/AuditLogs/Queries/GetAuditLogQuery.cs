using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.AuditLogs.Queries
{
    public class GetAuditLogQuery : IRequest<BaseResponse<AuditLogResponse>>
    {
        public string Id { get; set; }

        public GetAuditLogQuery(string id)
        {
            Id = id;
        }
    }
}
