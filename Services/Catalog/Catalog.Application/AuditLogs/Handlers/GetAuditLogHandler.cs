using AutoMapper;
using Catalog.Application.AuditLogs.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using LiteCommerce.Shared.Models;
using MediatR;
using System.Net;

namespace Catalog.Application.AuditLogs.Handlers
{
    public class GetAuditLogHandler : IRequestHandler<GetAuditLogQuery, BaseResponse<AuditLogResponse>>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMapper _mapper;

        public GetAuditLogHandler(IAuditLogRepository auditLogRepository, IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<AuditLogResponse>> Handle(GetAuditLogQuery request, CancellationToken cancellationToken)
        {
            var auditLog = await _auditLogRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (auditLog is null)
            {
                return BaseResponse<AuditLogResponse>.Failure("Audit log does not exist.", statusCode: HttpStatusCode.NotFound);
            }

            return BaseResponse<AuditLogResponse>.Success(_mapper.Map<AuditLogResponse>(auditLog));
        }
    }
}
